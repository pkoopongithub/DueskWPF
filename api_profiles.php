<?php
// Datei: api_profiles.php
require_once 'db_config.php';

$conn = connect($host, $user, $pass, $db);

// Header auslesen (verschiedene Methoden)
$userID = '';
$session = '';

// Methode 1: Aus $_SERVER (funktioniert zuverlässiger bei Strato)
$userID = $_SERVER['HTTP_X_USER_ID'] ?? $_SERVER['HTTP_X_USERID'] ?? '';
$session = $_SERVER['HTTP_X_SESSION'] ?? '';

// Methode 2: Aus getallheaders() falls verfügbar
if (empty($userID) && function_exists('getallheaders')) {
    $headers = getallheaders();
    $userID = $headers['X-User-ID'] ?? $headers['X-User-Id'] ?? '';
    $session = $headers['X-Session'] ?? '';
}

// Session validieren
if (!validate_session($conn, $userID, $session)) {
    deconnect($conn);
    echo json_encode(['error' => 'Nicht autorisiert', 'userID' => $userID]);
    exit;
}

$method = $_SERVER['REQUEST_METHOD'];

if ($method == 'GET') {
    if (isset($_GET['id'])) {
        $id = intval($_GET['id']);
        $sql = "SELECT * FROM profil WHERE profilID = $id AND userID = $userID";
        $result = mysqli_query($conn, $sql);
        $profile = mysqli_fetch_assoc($result);
        echo json_encode($profile ?: []);
    } else {
        $sql = "SELECT p.profilID, p.name, g.name as gruppename, g.gruppeID 
                FROM profil p 
                LEFT JOIN gruppe g ON p.gruppeID = g.gruppeID 
                WHERE p.userID = $userID 
                ORDER BY g.name, p.name";
        $result = mysqli_query($conn, $sql);
        $profiles = [];
        while ($row = mysqli_fetch_assoc($result)) {
            $profiles[] = $row;
        }
        echo json_encode($profiles);
    }
} 
elseif ($method == 'POST') {
    $data = json_decode(file_get_contents('php://input'), true);
    
    // Gruppe erstellen falls nötig
    $gruppeID = intval($data['gruppeID'] ?? 0);
    if (!empty($data['namegruppe'])) {
        $namegruppe = mysqli_real_escape_string($conn, $data['namegruppe']);
        $sql = "INSERT INTO gruppe (name, userID) VALUES ('$namegruppe', $userID)";
        if (mysqli_query($conn, $sql)) {
            $gruppeID = mysqli_insert_id($conn);
        }
    }
    
    if ($gruppeID <= 0) {
        echo json_encode(['error' => 'Gruppe erforderlich']);
        deconnect($conn);
        exit;
    }
    
    $name = mysqli_real_escape_string($conn, $data['name'] ?? '');
    
    // INSERT-Statement erstellen
    $sql = "INSERT INTO profil (name, userID, gruppeID";
    $values = "'$name', $userID, $gruppeID";
    
    for ($i = 1; $i <= 36; $i++) {
        $val = intval($data["item$i"] ?? 2);
        $sql .= ", item$i";
        $values .= ", $val";
    }
    for ($i = 1; $i <= 36; $i++) {
        $val = intval($data["feitem$i"] ?? 2);
        $sql .= ", feitem$i";
        $values .= ", $val";
    }
    $sql .= ") VALUES ($values)";
    
    if (mysqli_query($conn, $sql)) {
        echo json_encode(['success' => true, 'profilID' => mysqli_insert_id($conn)]);
    } else {
        echo json_encode(['error' => mysqli_error($conn)]);
    }
}
elseif ($method == 'PUT') {
    $data = json_decode(file_get_contents('php://input'), true);
    $profileID = intval($data['profilID'] ?? 0);
    
    // Gruppe erstellen falls nötig
    $gruppeID = intval($data['gruppeID'] ?? 0);
    if (!empty($data['namegruppe'])) {
        $namegruppe = mysqli_real_escape_string($conn, $data['namegruppe']);
        $sql = "INSERT INTO gruppe (name, userID) VALUES ('$namegruppe', $userID)";
        if (mysqli_query($conn, $sql)) {
            $gruppeID = mysqli_insert_id($conn);
        }
    }
    
    $name = mysqli_real_escape_string($conn, $data['name'] ?? '');
    
    $sql = "UPDATE profil SET name = '$name', gruppeID = $gruppeID";
    
    for ($i = 1; $i <= 36; $i++) {
        $val = intval($data["item$i"] ?? 2);
        $sql .= ", item$i = $val";
    }
    for ($i = 1; $i <= 36; $i++) {
        $val = intval($data["feitem$i"] ?? 2);
        $sql .= ", feitem$i = $val";
    }
    $sql .= " WHERE profilID = $profileID AND userID = $userID";
    
    if (mysqli_query($conn, $sql)) {
        echo json_encode(['success' => true]);
    } else {
        echo json_encode(['error' => mysqli_error($conn)]);
    }
}
elseif ($method == 'DELETE') {
    $profileID = intval($_GET['id'] ?? 0);
    $sql = "DELETE FROM profil WHERE profilID = $profileID AND userID = $userID";
    
    if (mysqli_query($conn, $sql)) {
        echo json_encode(['success' => true]);
    } else {
        echo json_encode(['error' => mysqli_error($conn)]);
    }
}

deconnect($conn);
?>