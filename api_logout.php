<?php
// Datei: api_logout.php
require_once 'db_config.php';

$conn = connect($host, $user, $pass, $db);

$userID = $_SERVER['HTTP_X_USER_ID'] ?? $_SERVER['HTTP_X_USERID'] ?? '';
$session = $_SERVER['HTTP_X_SESSION'] ?? '';

$response = ['success' => false];

if ($userID && $session) {
    $sql = "DELETE FROM anmeldung WHERE userID = $userID AND session = '$session'";
    if (mysqli_query($conn, $sql)) {
        $response = ['success' => true];
    } else {
        $response = ['error' => mysqli_error($conn)];
    }
}

deconnect($conn);
echo json_encode($response);
?>