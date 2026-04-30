<?php
// Datei: db_config.php
header('Content-Type: application/json; charset=utf-8');
header('Access-Control-Allow-Origin: *');
header('Access-Control-Allow-Methods: GET, POST, PUT, DELETE, OPTIONS');
header('Access-Control-Allow-Headers: Content-Type, X-Session, X-User-ID');

if ($_SERVER['REQUEST_METHOD'] == 'OPTIONS') {
    exit(0);
}

// Datenbankkonfiguration - HIER IHRE ZUGANGSDATEN EINTRAGEN!
$host = " ";             // Ihr MySQL Host
$user = " ";            // Ihr Benutzername
$pass = " ";            // Ihr Passwort
$db   = " ";             // Ihr Datenbankname

// MySQL connect (wie im Original)
function connect($host, $user, $pass, $db) {
    $conn = mysqli_connect($host, $user, $pass, $db);
    if (!$conn) {
        die(json_encode(['error' => 'MySQL-Verbindung fehlgeschlagen: ' . mysqli_connect_error()]));
    }
    return $conn;
}

function deconnect($conn) {
    return mysqli_close($conn);
}

// Session validieren (angepasst an Ihre Tabellenstruktur)
function validate_session($conn, $userID, $session) {
    if (empty($userID) || empty($session)) {
        return false;
    }
    $stmt = mysqli_prepare($conn, "SELECT sessionID FROM anmeldung WHERE userID = ? AND session = ?");
    mysqli_stmt_bind_param($stmt, "is", $userID, $session);
    mysqli_stmt_execute($stmt);
    mysqli_stmt_store_result($stmt);
    $count = mysqli_stmt_num_rows($stmt);
    mysqli_stmt_close($stmt);
    return $count > 0;
}
?>