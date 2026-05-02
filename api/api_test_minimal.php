<?php
// Datei: api_test_minimal.php - MINIMALE TESTVERSION mit db_config.php
error_reporting(E_ALL);
ini_set('display_errors', 1);

require_once 'db_config.php';

echo "<h1>DÜSK API Minimal Test</h1>";

echo "<h2>Test 1: MySQL Verbindung (via db_config.php)</h2>";
$conn = connect($host, $user, $pass, $db);

if (!$conn) {
    echo "<p style='color:red'>FEHLER: " . mysqli_connect_error() . "</p>";
    echo "<p>Bitte überprüfen Sie die Zugangsdaten in db_config.php!</p>";
} else {
    echo "<p style='color:green'>✓ Verbindung erfolgreich!</p>";
    
    echo "<h2>Test 2: Tabellen prüfen</h2>";
    $result = mysqli_query($conn, "SHOW TABLES");
    echo "<ul>";
    while ($row = mysqli_fetch_array($result)) {
        echo "<li>" . $row[0] . "</li>";
    }
    echo "</ul>";
    
    echo "<h2>Test 3: Benutzer</h2>";
    $result = mysqli_query($conn, "SELECT ID, user FROM user LIMIT 5");
    if (mysqli_num_rows($result) > 0) {
        echo "<table border='1' cellpadding='5'>";
        echo "<tr><th>ID</th><th>Benutzername</th></tr>";
        while ($row = mysqli_fetch_assoc($result)) {
            echo "<tr><td>" . $row['ID'] . "</td><td>" . htmlspecialchars($row['user']) . "</td></tr>";
        }
        echo "</table>";
    } else {
        echo "<p>Keine Benutzer gefunden.</p>";
    }
    
    deconnect($conn);
}

echo "<h2>Test 4: Login API (manuell)</h2>";
echo '<form action="api_login.php" method="post" target="_blank">
    <input type="hidden" name="username" value="gast">
    <input type="hidden" name="pass" value="gast">
    <button type="submit">Login API direkt testen</button>
</form>';

echo "<h2>Test 5: Profile API (manuell)</h2>";
echo '<form action="api_profiles.php" method="get" target="_blank">
    <button type="submit">Profile API direkt testen</button>
</form>';

echo "<h2>Test 6: db_config.php Variablen prüfen</h2>";
echo "<ul>";
echo "<li>Host: " . htmlspecialchars($host) . "</li>";
echo "<li>Datenbank: " . htmlspecialchars($db) . "</li>";
echo "<li>Benutzer: " . htmlspecialchars($user) . "</li>";
echo "<li>Passwort: " . str_repeat("*", strlen($pass)) . "</li>";
echo "</ul>";
?>