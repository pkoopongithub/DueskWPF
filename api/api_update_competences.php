<?php
// Datei: api_update_competences.php
require_once 'db_config.php';

$conn = connect($host, $user, $pass, $db);

$userID = $_SERVER['HTTP_X_USER_ID'] ?? $_SERVER['HTTP_X_USERID'] ?? '';
$session = $_SERVER['HTTP_X_SESSION'] ?? '';

if (!validate_session($conn, $userID, $session)) {
    deconnect($conn);
    echo json_encode(['error' => 'Nicht autorisiert']);
    exit;
}

$data = json_decode(file_get_contents('php://input'), true);
$profileID = intval($data['profilID'] ?? 0);
$competences = $data['competences'] ?? [];

if ($profileID <= 0) {
    echo json_encode(['error' => 'Ungültige Profil-ID']);
    deconnect($conn);
    exit;
}

if (count($competences) != 6) {
    echo json_encode(['error' => 'Es werden genau 6 Kompetenzwerte benötigt']);
    deconnect($conn);
    exit;
}

$sql = "UPDATE profil SET 
        kompetenz1 = {$competences[0]}, 
        kompetenz2 = {$competences[1]}, 
        kompetenz3 = {$competences[2]}, 
        kompetenz4 = {$competences[3]}, 
        kompetenz5 = {$competences[4]}, 
        kompetenz6 = {$competences[5]} 
        WHERE profilID = $profileID AND userID = $userID";

if (mysqli_query($conn, $sql)) {
    echo json_encode(['success' => true]);
} else {
    echo json_encode(['error' => mysqli_error($conn)]);
}

deconnect($conn);
?>