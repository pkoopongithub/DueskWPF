<?php
// Datei: api_test.php - OHNE JSON-Header, reine HTML-Seite
error_reporting(E_ALL);
ini_set('display_errors', 1);

// KEIN JSON-Header hier! Nur HTML
?>
<!DOCTYPE html>
<html>
<head>
    <title>DÜSK API Test</title>
    <meta charset="utf-8">
    <style>
        body { font-family: monospace; margin: 20px; background: #f5f5f5; }
        .success { color: green; font-weight: bold; }
        .error { color: red; font-weight: bold; }
        pre { background: #fff; padding: 10px; border: 1px solid #ddd; overflow-x: auto; }
        button { padding: 10px; margin: 5px; cursor: pointer; }
        .section { background: white; padding: 15px; margin: 20px 0; border-radius: 5px; }
    </style>
</head>
<body>
    <h1>DÜSK API Test Suite</h1>
    
    <?php
    // Direkte Datenbankverbindung für die Status-Anzeige (OHNE db_config.php)
    $host = "rdbms.strato.de";     // Ihr MySQL Host
    $user = "U3517771";               // Ihr Benutzername
    $pass = "dBkNpI1000Jn";       // Ihr Passwort
    $db   = "DB3517771";             // Ihr Datenbankname
    
    $conn = mysqli_connect($host, $user, $pass, $db);
    if ($conn) {
        echo '<div class="section">';
        echo '<h2>📊 Datenbank-Status</h2>';
        echo '<p class="success">✓ Verbindung erfolgreich</p>';
        
        $result = mysqli_query($conn, "SELECT COUNT(*) as cnt FROM user");
        $row = mysqli_fetch_assoc($result);
        echo "<p>Benutzer: " . $row['cnt'] . "</p>";
        
        $result = mysqli_query($conn, "SELECT COUNT(*) as cnt FROM profil");
        $row = mysqli_fetch_assoc($result);
        echo "<p>Profile: " . $row['cnt'] . "</p>";
        
        mysqli_close($conn);
        echo '</div>';
    } else {
        echo '<p class="error">✗ Datenbankverbindung fehlgeschlagen</p>';
    }
    ?>
    
    <div class="section">
        <h2>🧪 API-Endpunkt Tests</h2>
        <p>Klicken Sie auf die Buttons, um die API zu testen:</p>
        
        <button onclick="testLogin()">1. Login testen</button>
        <button onclick="testProfiles()" id="btnProfiles" disabled>2. Profile abrufen</button>
        <button onclick="testNorms('hs')">3. Normen HS laden</button>
        <button onclick="testNorms('fs')">4. Normen FS laden</button>
        <button onclick="testGroups()" id="btnGroups" disabled>5. Gruppen abrufen</button>
        
        <pre id="result" style="margin-top:20px; background:#1e1e1e; color:#d4d4d4; padding:15px; border-radius:5px; overflow-x:auto;">
Hier erscheinen die Testergebnisse...
        </pre>
    </div>
    
    <script>
    let currentUserID = null;
    let currentSession = null;
    let apiBaseUrl = window.location.href.substring(0, window.location.href.lastIndexOf("/") + 1);
    
    function log(message, isError = false) {
        const resultDiv = document.getElementById("result");
        const prefix = isError ? "❌ " : "✅ ";
        const timestamp = new Date().toLocaleTimeString();
        resultDiv.textContent += `[${timestamp}] ${prefix}${message}\n`;
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
            log("Antwort: " + JSON.stringify(data, null, 2));
            
            if (data.success === true || data.userID) {
                currentUserID = data.userID;
                currentSession = data.session;
                log("Login erfolgreich! UserID: " + currentUserID);
                document.getElementById("btnProfiles").disabled = false;
                document.getElementById("btnGroups").disabled = false;
            } else {
                log("Login fehlgeschlagen: " + JSON.stringify(data), true);
            }
        } catch (error) {
            log("Fehler: " + error.message, true);
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
                log(data.length + " Profile gefunden");
            } else if (data.error) {
                log("Fehler: " + data.error, true);
            }
        } catch (error) {
            log("Fehler: " + error.message, true);
        }
    }
    
    async function testNorms(type) {
        log("Normen (" + type.toUpperCase() + ") werden geladen...");
        
        try {
            const response = await fetch(apiBaseUrl + "api_norms.php?type=" + type);
            const data = await response.json();
            
            if (data.error) {
                log("Fehler: " + data.error, true);
            } else {
                log("Normen " + type.toUpperCase() + " geladen");
                if (data.normSE && data.normSE[1]) {
                    log("Kompetenz 1: " + JSON.stringify(data.normSE[1]));
                }
            }
        } catch (error) {
            log("Fehler: " + error.message, true);
        }
    }
    
    async function testGroups() {
        if (!currentUserID || !currentSession) {
            log("Bitte zuerst einloggen!", true);
            return;
        }
        
        log("Gruppen werden abgerufen...");
        
        try {
            const response = await fetch(apiBaseUrl + "api_groups.php", {
                method: "GET",
                headers: {
                    "X-User-ID": currentUserID,
                    "X-Session": currentSession
                }
            });
            
            const data = await response.json();
            log("Antwort: " + JSON.stringify(data, null, 2));
        } catch (error) {
            log("Fehler: " + error.message, true);
        }
    }
    </script>
</body>
</html>