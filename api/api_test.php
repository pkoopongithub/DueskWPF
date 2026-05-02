<?php
// Datei: api_test.php - Verwendet db_config.php
error_reporting(E_ALL);
ini_set('display_errors', 1);

require_once 'db_config.php';

echo '<!DOCTYPE html>
<html>
<head>
    <title>DÜSK API Test</title>
    <meta charset="utf-8">
    <style>
        body { font-family: monospace; margin: 20px; background: #f5f5f5; }
        .success { color: green; font-weight: bold; }
        .error { color: red; font-weight: bold; }
        pre { background: #fff; padding: 10px; border: 1px solid #ddd; overflow-x: auto; }
    </style>
</head>
<body>
    <h1>DÜSK API Test</h1>';

// Verbindung mit db_config.php Funktionen
$conn = connect($host, $user, $pass, $db);

if (!$conn) {
    echo '<p class="error">✗ VERBINDUNGSFEHLER: ' . mysqli_connect_error() . '</p>';
    echo '<p>Bitte überprüfen Sie die Zugangsdaten in db_config.php!</p>';
    echo '</body></html>';
    exit;
}

echo '<p class="success">✓ Verbindung erfolgreich!</p>';

// Tabellen prüfen
echo '<h2>2. Tabellen prüfen</h2>';
$tables = ['user', 'anmeldung', 'gruppe', 'profil', 'normSEhs', 'normFEhs', 'normSEfs', 'normFEfs'];
foreach ($tables as $table) {
    $result = mysqli_query($conn, "SHOW TABLES LIKE '$table'");
    if ($result && mysqli_num_rows($result) > 0) {
        echo '<p class="success">✓ Tabelle ' . $table . ' existiert</p>';
    } else {
        echo '<p class="error">✗ Tabelle ' . $table . ' fehlt!</p>';
    }
}

// Datensätze zählen
echo '<h2>3. Datensätze</h2>';
$result = mysqli_query($conn, "SELECT COUNT(*) as cnt FROM user");
$row = mysqli_fetch_assoc($result);
echo '<p>Benutzer: ' . $row['cnt'] . '</p>';

$result = mysqli_query($conn, "SELECT COUNT(*) as cnt FROM profil");
$row = mysqli_fetch_assoc($result);
echo '<p>Profile: ' . $row['cnt'] . '</p>';

$result = mysqli_query($conn, "SELECT COUNT(*) as cnt FROM anmeldung");
$row = mysqli_fetch_assoc($result);
echo '<p>Aktive Sessions: ' . $row['cnt'] . '</p>';

deconnect($conn);

// JavaScript für API-Tests
echo '
<h2>4. API-Tests (JavaScript)</h2>
<p>Klicken Sie auf die Buttons, um die API-Endpunkte zu testen:</p>

<div style="margin: 10px 0;">
    <button onclick="testLogin()" style="padding:10px; margin:5px;">1. Login testen</button>
    <button onclick="testProfiles()" style="padding:10px; margin:5px;" id="btnProfiles" disabled>2. Profile abrufen</button>
    <button onclick="testNorms(\'hs\')" style="padding:10px; margin:5px;">3. Normen HS laden</button>
    <button onclick="testNorms(\'fs\')" style="padding:10px; margin:5px;">4. Normen FS laden</button>
</div>

<pre id="result" style="background:#fff; padding:15px; border:1px solid #ddd; margin-top:20px; white-space:pre-wrap; word-wrap:break-word;">
Hier erscheinen die Testergebnisse...
</pre>

<script>
let currentUserID = null;
let currentSession = null;
let apiBaseUrl = window.location.href.substring(0, window.location.href.lastIndexOf("/") + 1);

function log(message, isError = false) {
    const resultDiv = document.getElementById("result");
    const prefix = isError ? "❌ " : "✅ ";
    resultDiv.textContent += prefix + message + "\\n";
    resultDiv.scrollTop = resultDiv.scrollHeight;
}

async function testLogin() {
    log("Login wird durchgeführt...");
    
    try {
        const response = await fetch(apiBaseUrl + "api_login.php", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ username: "gast", password: "gast" })
        });
        
        const data = await response.json();
        log("Antwort erhalten: " + JSON.stringify(data, null, 2));
        
        if (data.success === true || data.userID) {
            currentUserID = data.userID;
            currentSession = data.session;
            log("✓ Login erfolgreich! UserID: " + currentUserID);
            log("Session: " + currentSession);
            document.getElementById("btnProfiles").disabled = false;
        } else {
            log("✗ Login fehlgeschlagen: " + JSON.stringify(data), true);
        }
    } catch (error) {
        log("✗ Fehler: " + error.message, true);
    }
}

async function testProfiles() {
    if (!currentUserID || !currentSession) {
        log("Bitte zuerst einloggen!", true);
        return;
    }
    
    log("Profile werden abgerufen...");
    
    try {
        const response = await fetch(apiBaseUrl + "api_profiles.php", {
            method: "GET",
            headers: {
                "X-User-ID": currentUserID,
                "X-Session": currentSession
            }
        });
        
        const data = await response.json();
        log("Antwort: " + JSON.stringify(data, null, 2));
        
        if (Array.isArray(data)) {
            log("✓ " + data.length + " Profile gefunden");
        } else if (data.error) {
            log("✗ Fehler: " + data.error, true);
        }
    } catch (error) {
        log("✗ Fehler: " + error.message, true);
    }
}

async function testNorms(type) {
    log("Normen (" + type.toUpperCase() + ") werden geladen...");
    
    try {
        const response = await fetch(apiBaseUrl + "api_norms.php?type=" + type);
        const data = await response.json();
        
        if (data.error) {
            log("✗ Fehler: " + data.error, true);
        } else {
            log("✓ Normen " + type.toUpperCase() + " geladen");
            if (data.normSE && data.normSE[1]) {
                log("Kompetenz 1 Werte: " + JSON.stringify(data.normSE[1]));
            }
        }
    } catch (error) {
        log("✗ Fehler: " + error.message, true);
    }
}
</script>

</body>
</html>';
?>