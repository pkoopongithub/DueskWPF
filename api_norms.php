<?php
// Datei: api_norms.php - KEINE Session-Validierung nötig
require_once 'db_config.php';

$conn = connect($host, $user, $pass, $db);

// KEINE Session-Validierung hier - Normen sind öffentlich
$norm_type = $_GET['type'] ?? 'hs';
if (!in_array($norm_type, ['hs', 'fs'])) {
    $norm_type = 'hs';
}

$tableSE = "normSE" . $norm_type;
$tableFE = "normFE" . $norm_type;

$response = ['normSE' => [], 'normFE' => []];

// Normen für Selbsteinschätzung laden
$result = mysqli_query($conn, "SELECT kompetenzID, p1, p2, p3, p4, p5 FROM $tableSE ORDER BY kompetenzID");
if ($result) {
    while ($row = mysqli_fetch_assoc($result)) {
        $kompetenzID = $row['kompetenzID'];
        $response['normSE'][$kompetenzID] = [
            (float)$row['p1'],
            (float)$row['p2'],
            (float)$row['p3'],
            (float)$row['p4'],
            (float)$row['p5']
        ];
    }
    mysqli_free_result($result);
}

// Normen für Fremdeinschätzung laden
$result = mysqli_query($conn, "SELECT kompetenzID, p1, p2, p3, p4, p5 FROM $tableFE ORDER BY kompetenzID");
if ($result) {
    while ($row = mysqli_fetch_assoc($result)) {
        $kompetenzID = $row['kompetenzID'];
        $response['normFE'][$kompetenzID] = [
            (float)$row['p1'],
            (float)$row['p2'],
            (float)$row['p3'],
            (float)$row['p4'],
            (float)$row['p5']
        ];
    }
    mysqli_free_result($result);
}

deconnect($conn);
echo json_encode($response);
?>