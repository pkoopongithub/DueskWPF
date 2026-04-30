<?php
// Datei: api_login.php
require_once 'db_config.php';

$conn = connect($host, $user, $pass, $db);
$response = [];

$data = json_decode(file_get_contents('php://input'), true);
$username = $data['username'] ?? '';
$password = $data['password'] ?? '';

if (empty($username) || empty($password)) {
    $response = ['error' => 'Benutzername und Passwort erforderlich'];
} else {
    // Benutzer suchen (wie im Original)
    $sql = "SELECT ID FROM user WHERE user = '$username' AND pass = '$password'";
    $result = mysqli_query($conn, $sql);
    
    if ($result && mysqli_num_rows($result) > 0) {
        $row = mysqli_fetch_assoc($result);
        $userID = $row['ID'];
    } else {
        // Neuen Benutzer erstellen (wie im Original)
        $sql = "INSERT INTO user (user, pass) VALUES ('$username', '$password')";
        if (mysqli_query($conn, $sql)) {
            $userID = mysqli_insert_id($conn);
        } else {
            $response = ['error' => 'Fehler beim Erstellen des Benutzers: ' . mysqli_error($conn)];
            deconnect($conn);
            echo json_encode($response);
            exit;
        }
    }
    
    // Session erstellen (wie im Original mit setze_session)
    $session = md5(uniqid(rand(), true));
    $sql = "INSERT INTO anmeldung (userID, session) VALUES ($userID, '$session')";
    
    if (mysqli_query($conn, $sql)) {
        $response = [
            'success' => true,
            'userID' => $userID,
            'session' => $session
        ];
    } else {
        $response = ['error' => 'Fehler beim Erstellen der Session: ' . mysqli_error($conn)];
    }
}

deconnect($conn);
echo json_encode($response);
?>