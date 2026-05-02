<?php
// Datei: api_groups.php
require_once 'db_config.php';

$conn = connect($host, $user, $pass, $db);

// Header auslesen
$userID = $_SERVER['HTTP_X_USER_ID'] ?? $_SERVER['HTTP_X_USERID'] ?? '';
$session = $_SERVER['HTTP_X_SESSION'] ?? '';

if (!validate_session($conn, $userID, $session)) {
    deconnect($conn);
    echo json_encode(['error' => 'Nicht autorisiert']);
    exit;
}

$method = $_SERVER['REQUEST_METHOD'];

if ($method == 'GET') {
    $sql = "SELECT gruppeID, name FROM gruppe WHERE userID = $userID ORDER BY name";
    $result = mysqli_query($conn, $sql);
    
    $groups = [];
    while ($row = mysqli_fetch_assoc($result)) {
        $groups[] = [
            'gruppeID' => (int)$row['gruppeID'],
            'name' => $row['name']
        ];
    }
    echo json_encode($groups);
}
elseif ($method == 'POST') {
    $data = json_decode(file_get_contents('php://input'), true);
    $groupName = mysqli_real_escape_string($conn, trim($data['name'] ?? ''));
    
    if (empty($groupName)) {
        echo json_encode(['error' => 'Gruppenname darf nicht leer sein']);
        deconnect($conn);
        exit;
    }
    
    $sql = "INSERT INTO gruppe (name, userID) VALUES ('$groupName', $userID)";
    if (mysqli_query($conn, $sql)) {
        echo json_encode([
            'success' => true,
            'gruppeID' => mysqli_insert_id($conn),
            'name' => $groupName
        ]);
    } else {
        echo json_encode(['error' => mysqli_error($conn)]);
    }
}

deconnect($conn);
?>