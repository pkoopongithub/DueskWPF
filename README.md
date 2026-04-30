# DÜSK - WPF Desktop App für Windows

**DÜSK** ist eine professionelle Desktop-Anwendung für Windows zur Erfassung und Auswertung von Selbst- und Fremdeinschätzungen von Schülerkompetenzen. Die App wurde mit **.NET 8.0** und **WPF (Windows Presentation Foundation)** entwickelt und bietet eine moderne, flüssige Benutzeroberfläche mit nativer Windows-Integration.

---

## 📋 Inhaltsverzeichnis

- [Überblick](#-überblick)
- [Features](#-features)
- [Technologie-Stack](#-technologie-stack)
- [API-Dokumentation](#-api-dokumentation)
- [Datenbankstruktur](#-datenbankstruktur)
- [Installation](#-installation)
- [Projektstruktur](#-projektstruktur)
- [Berechnungslogik](#-berechnungslogik)
- [Konfiguration](#-konfiguration)
- [Build & Deployment](#-build--deployment)
- [Entwicklung](#-entwicklung)
- [Fehlerbehandlung](#-fehlerbehandlung)


---

## 🎯 Überblick

DÜSK (Düsseldorfer Schülerinventar) ist ein diagnostisches Instrument zur Erfassung von Schülerkompetenzen in sechs Bereichen:

| Bereich | Beschreibung |
|---------|--------------|
| 1. **Arbeitsverhalten** | Zuverlässigkeit, Arbeitstempo, Planung, Organisation |
| 2. **Lernverhalten** | Selbstständigkeit, Belastbarkeit, Konzentration, Merkfähigkeit |
| 3. **Sozialverhalten** | Teamfähigkeit, Hilfsbereitschaft, Kommunikation, Konfliktfähigkeit |
| 4. **Fachkompetenz** | Schreiben, Lesen, Mathematik, Naturwissenschaften, Fremdsprachen |
| 5. **Personale Kompetenz** | Kreativität, Problemlösung, Abstraktion, Reflexion |
| 6. **Methodenkompetenz** | Präsentation, PC-Kenntnisse, fächerübergreifendes Denken |

Die Anwendung ermöglicht den Vergleich von **Selbsteinschätzung (SE)** und **Fremdeinschätzung (FE)** mit Normtabellen für Hauptschulen (HS) und Förderschulen (FS).

---

## ✨ Features

### Kernfunktionen
- ✅ **Login/Logout** mit persistenter Session-Speicherung
- ✅ **CRUD-Operationen** für Schülerprofile (Erstellen, Lesen, Aktualisieren, Löschen)
- ✅ **36 Items** pro Einschätzung (4-stufige Likert-Skala: 1-4)
- ✅ **Automatische Kompetenzberechnung** aus 72 Items
- ✅ **Normwertvergleich** (HS/FS Normtabellen)
- ✅ **Profilansicht** mit Tabellen, interaktiven Diagrammen und Textinterpretation

### Erweiterte Funktionen
- 📊 **Zeitreihenanalyse** für Gruppenentwicklung über Zeit
- 📈 **Vergleichsdiagramme** (SE vs. FE mit Pearson-Korrelation)
- 📐 **Korrelationsberechnung** mit statistischer Interpretation
- 🔍 **Such- und Filterfunktionen** (nach Namen und Gruppen)
- 📁 **Gruppenverwaltung** (CRUD mit Umbenennung)
- 🌙 **Modernes Fluent Design** (Windows 11 optimiert)
- 💾 **Persistente Speicherung** (Session, Fensterposition)
- ⌨️ **Tastatur-Shortcuts** (Enter für Login, etc.)
- 🖥️ **Native Windows-Integration** (Taskbar, Fensterverwaltung)

---

## 🛠 Technologie-Stack

### WPF App

| Komponente | Technologie | Version |
|------------|-------------|---------|
| Framework | .NET 8.0 | 8.0 |
| Sprache | C# | 12.0 |
| UI-Framework | WPF | - |
| HTTP-Client | HttpClient | .NET native |
| JSON-Parser | Newtonsoft.Json | 13.0.3 |
| Charts | LiveCharts.Wpf | 0.9.7 |
| Behaviors | Microsoft.Xaml.Behaviors.Wpf | 1.1.77 |
| Build-Tool | MSBuild | - |
| Mindest-Windows | Windows 10 | 1809+ |

### Server (PHP API)

| Komponente | Technologie |
|------------|-------------|
| Backend | PHP 7.4+ |
| Datenbank | MySQL 5.7+ |
| Webserver | Apache/Nginx |
| Format | JSON |

---

## 📡 API-Dokumentation

Die REST-API ist unter `https://paul-koop.org/api/` verfügbar.

### Authentifizierung

#### POST `/api_login.php`

```json
// Request
{
    "username": "gast",
    "password": "gast"
}

// Response (Success)
{
    "success": true,
    "userID": "12345",
    "session": "abc123def456789"
}

// Response (Error)
{
    "success": false,
    "error": "Invalid credentials"
}
```

### Profile-Endpunkte

> **Wichtig:** Alle Profile-Endpunkte benötigen die Header:
> - `X-User-ID`: Benutzer-ID aus Login
> - `X-Session`: Session-Token aus Login

#### GET `/api_profiles.php`

**Response**: Array aller Profile des Benutzers

```json
[
    {
        "profilID": "1",
        "name": "Max Mustermann",
        "gruppename": "Klasse 8a",
        "gruppeID": "5",
        "created_at": "2024-01-15 10:30:00",
        "item1": 4, "item2": 3, "item36": 2,
        "feitem1": 3, "feitem2": 4, "feitem36": 3
    }
]
```

#### GET `/api_profiles.php?id={profileId}`

**Response**: Einzelnes Profil

#### POST `/api_profiles.php`

**Request Body**: Vollständiges Profil-Objekt (alle 72 Items)
**Response**: `200 OK` bei Erfolg

#### PUT `/api_profiles.php`

**Request Body**: Aktualisiertes Profil-Objekt
**Response**: `200 OK` bei Erfolg

#### DELETE `/api_profiles.php?id={profileId}`

**Response**: `200 OK` bei Erfolg

### Gruppen-Endpunkte

#### GET `/api_groups.php`

**Response**:
```json
[
    {"gruppeID": 1, "name": "Klasse 8a"},
    {"gruppeID": 2, "name": "Klasse 8b"},
    {"gruppeID": 3, "name": "Klasse 9a"}
]
```

#### POST `/api_groups.php`

**Request Body**: `{"name": "Neue Gruppe"}`
**Response**: `200 OK` bei Erfolg

#### DELETE `/api_groups.php?id={groupId}`

**Response**: `200 OK` bei Erfolg

---

## 🗄 Datenbankstruktur

### ER-Diagramm

```
┌─────────────┐     ┌─────────────┐     ┌─────────────┐
│    users    │     │   groups    │     │  profiles   │
├─────────────┤     ├─────────────┤     ├─────────────┤
│ userID (PK) │────<│ userID (FK) │     │ profilID(PK)│
│ username    │     │ gruppeID(PK)│<────│ userID (FK) │
│ password    │     │ name        │     │ gruppeID(FK)│
│ session     │     │ created_at  │     │ name        │
│ created_at  │     └─────────────┘     │ item1-36    │
└─────────────┘                          │ feitem1-36  │
                                         │ created_at  │
                                         │ updated_at  │
                                         └─────────────┘
```

### SQL-Schema

```sql
-- Users Tabelle
CREATE TABLE users (
    userID INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(50) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    session_token VARCHAR(255),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Groups Tabelle
CREATE TABLE groups (
    gruppeID INT AUTO_INCREMENT PRIMARY KEY,
    userID INT NOT NULL,
    name VARCHAR(255) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (userID) REFERENCES users(userID) ON DELETE CASCADE,
    UNIQUE KEY unique_group_per_user (userID, name)
);

-- Profiles Tabelle
CREATE TABLE profiles (
    profilID INT AUTO_INCREMENT PRIMARY KEY,
    userID INT NOT NULL,
    name VARCHAR(255) NOT NULL,
    gruppeID INT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    -- 36 SE-Items (Werte 1-4)
    item1 TINYINT DEFAULT 2, item2 TINYINT DEFAULT 2, ..., item36 TINYINT DEFAULT 2,
    -- 36 FE-Items (Werte 1-4)
    feitem1 TINYINT DEFAULT 2, feitem2 TINYINT DEFAULT 2, ..., feitem36 TINYINT DEFAULT 2,
    FOREIGN KEY (userID) REFERENCES users(userID) ON DELETE CASCADE,
    FOREIGN KEY (gruppeID) REFERENCES groups(gruppeID) ON DELETE SET NULL
);
```

---

## 💻 Installation

### Voraussetzungen

- **Windows 10** oder **Windows 11** (64-Bit)
- **.NET 8.0 Desktop Runtime** ([Download](https://dotnet.microsoft.com/en-us/download/dotnet/8.0))
- **Visual Studio 2022** (für Entwicklung, mindestens Community Edition)

### Schritt-für-Schritt Installation

```bash
# 1. Repository klonen
git clone https://github.com/pkoopongithub/duesk-wpf.git
cd duesk-wpf

# 2. Projekt in Visual Studio öffnen
# Doppelklick auf DueskWPF.sln

# 3. NuGet-Pakete wiederherstellen
dotnet restore

# 4. App starten (Debug)
dotnet run

# 5. Release-Build erstellen
dotnet publish -c Release -o ./publish
```

### Ausführbare Datei erstellen

```bash
# Einzelne ausführbare Datei erstellen
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o ./publish
```

---

## 📁 Projektstruktur

```
duesk-wpf/
├── DueskWPF.csproj                  # Projektdatei mit NuGet-Abhängigkeiten
├── DueskWPF.sln                     # Visual Studio Projektmappe
├── App.xaml                         # Anwendungsressourcen & Styles
├── App.xaml.cs                      # App-Lifecycle (Startup, Exit)
├── Models/
│   ├── LoginResponse.cs             # API Response Model für Login
│   ├── Profile.cs                   # Profil-Datenmodell (72 Items)
│   ├── Group.cs                     # Gruppen-Datenmodell
│   └── Norms.cs                     # Normtabellen & Konstanten (HS/FS)
├── Services/
│   ├── SessionManager.cs            # Session & persistente Speicherung
│   ├── ApiService.cs                # HTTP-API-Kommunikation
│   └── Calculator.cs                # Berechnungslogik (Sums, Korrelation)
├── Views/
│   ├── LoginWindow.xaml             # Login-Fenster
│   ├── LoginWindow.xaml.cs
│   ├── MainWindow.xaml              # Hauptfenster mit Sidebar & Toolbar
│   ├── MainWindow.xaml.cs
│   ├── ProfileDetailWindow.xaml     # Profildetails mit Charts & Tabs
│   ├── ProfileDetailWindow.xaml.cs
│   ├── ProfileEditWindow.xaml       # Profil erstellen/bearbeiten
│   ├── ProfileEditWindow.xaml.cs
│   ├── GroupManagerWindow.xaml      # Gruppenverwaltung (CRUD)
│   ├── GroupManagerWindow.xaml.cs
│   ├── TimeSeriesWindow.xaml        # Zeitreihenanalyse mit Chart
│   └── TimeSeriesWindow.xaml.cs
├── Converters/
│   └── BoolToVisibilityConverter.cs # Wertkonverter für UI
├── Resources/
│   └── Styles.xaml                  # Globale UI-Styles (Buttons, TextBoxes)
└── README.md
```

---

## 🧮 Berechnungslogik

### Item-zu-Kompetenz-Zuordnung

```csharp
// Kompetenz 1: Arbeitsverhalten (Items 1-10)
for (var i = 0; i < 10; i++) sums[1] += items[i];

// Kompetenz 2: Lernverhalten (Items 11-20)
for (var i = 10; i < 20; i++) sums[2] += items[i];

// Kompetenz 3: Sozialverhalten (Items 21-28 + Items 9-10)
for (var i = 20; i < 28; i++) sums[3] += items[i];
sums[3] += items[8] + items[9];

// Kompetenz 4: Fachkompetenz (Items 29-36)
for (var i = 28; i < 36; i++) sums[4] += items[i];

// Kompetenz 5: Personale Kompetenz (spezifische Items)
sums[5] = items[0] + items[1] + items[5] + items[6] + items[7] +
          items[8] + items[9] + items[11] + items[12] + items[13] + items[14];

// Kompetenz 6: Methodenkompetenz (spezifische Items)
sums[6] = items[2] + items[3] + items[4] + items[8] + items[9] +
          items[10] + items[16] + items[17];
```

### Profilwertberechnung

```csharp
public static int[] CalculateProfileValues(int[] sums, double[][] norm)
{
    var values = new int[6];
    for (var k = 1; k <= 6; k++)
    {
        var value = 5;
        for (var p = 0; p < 5; p++)
        {
            if (sums[k] < norm[k - 1][p])
            {
                value = p + 1;
                break;
            }
        }
        values[k - 1] = value;
    }
    return values;
}
```

### Korrelationsberechnung (Pearson)

```csharp
public static double CalculateCorrelation(int[] seValues, int[] feValues)
{
    var n = seValues.Length;
    double sumSE = 0, sumFE = 0, sumSEFE = 0, sumSE2 = 0, sumFE2 = 0;
    
    for (var i = 0; i < n; i++)
    {
        sumSE += seValues[i];
        sumFE += feValues[i];
        sumSEFE += seValues[i] * feValues[i];
        sumSE2 += seValues[i] * seValues[i];
        sumFE2 += feValues[i] * feValues[i];
    }
    
    var numerator = n * sumSEFE - sumSE * sumFE;
    var denominator = Math.Sqrt((n * sumSE2 - sumSE * sumSE) * 
                                 (n * sumFE2 - sumFE * sumFE));
    
    return Math.Abs(denominator) < 0.0001 ? 0 : numerator / denominator;
}
```

### Normtabellen (Auszug)

| Kompetenz | HS SE Grenzwerte | HS FE Grenzwerte |
|-----------|------------------|------------------|
| Arbeitsverhalten | 21.33, 25.33, 29.33, 33.32, 37.32 | 12.66, 18.16, 23.66, 29.16, 34.66 |
| Lernverhalten | 20.87, 24.95, 29.03, 33.13, 37.18 | 13.33, 18.42, 23.51, 28.60, 33.69 |
| Sozialverhalten | 17.93, 21.37, 24.80, 28.23, 31.67 | 10.75, 15.41, 20.07, 24.73, 29.39 |
| Fachkompetenz | 13.98, 17.71, 21.44, 25.17, 28.90 | 14.22, 15.30, 16.38, 17.46, 18.54 |
| Personale Kompetenz | 24.60, 28.55, 33.04, 37.53, 42.01 | 14.12, 20.21, 26.30, 32.39, 38.48 |
| Methodenkompetenz | 15.53, 18.97, 22.40, 25.83, 29.27 | 10.53, 14.51, 18.49, 22.47, 26.45 |

*Für FS (Förderschule) existieren separate Normtabellen.*

---

## ⚙️ Konfiguration

### API Base URL ändern

```csharp
// Services/ApiService.cs
private const string BaseUrl = "https://your-server.com/api/";
```

### Session-Speicherort

Die Session wird als `session.json` im Ausführungsverzeichnis gespeichert.

### Fensterposition speichern

Die Fenstergröße und -position wird automatisch im `session.json` gespeichert.

---

## 🚀 Build & Deployment

### Entwicklung

```bash
# Build ausführen
dotnet build

# App starten
dotnet run

# Release-Build
dotnet build -c Release

# Publish als Einzeldatei
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o ./publish
```

### Systemanforderungen

- **Betriebssystem:** Windows 10 / Windows 11 (64-Bit)
- **.NET Runtime:** .NET 8.0 Desktop Runtime
- **RAM:** mindestens 256 MB
- **Festplatte:** etwa 50 MB

---

## 🐛 Fehlerbehandlung

### HTTP-Statuscodes

| Code | Bedeutung | Behandlung |
|------|-----------|------------|
| 200 | Erfolg | Daten verarbeiten |
| 400 | Bad Request | Validierungsfehler prüfen |
| 401 | Unauthorized | Neu anmelden |
| 403 | Forbidden | Berechtigung prüfen |
| 404 | Not Found | Ressource existiert nicht |
| 500 | Server Error | Erneut versuchen, Support kontaktieren |

### Typische Fehler und Lösungen

```csharp
try
{
    var profiles = await _apiService.GetProfilesAsync();
}
catch (HttpRequestException ex)
{
    if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
    {
        SessionManager.Instance.Clear();
        var loginWindow = new LoginWindow();
        loginWindow.Show();
        Close();
    }
    else
    {
        MessageBox.Show($"Fehler: {ex.Message}", "Fehler", 
            MessageBoxButton.OK, MessageBoxImage.Error);
    }
}
catch (TaskCanceledException)
{
    MessageBox.Show("Verbindung zeitüberschritten", "Fehler", 
        MessageBoxButton.OK, MessageBoxImage.Error);
}
```

---

## 🔄 Vergleich der Implementierungen

| Feature | WPF (.NET) | Electron | Flutter | React Native | Swift (iOS) | Java (Swing) |
|---------|------------|----------|---------|--------------|-------------|--------------|
| Windows | ✅ Exzellent | ✅ Gut | ✅ Gut | ❌ | ❌ | ✅ Gut |
| macOS | ❌ | ✅ Gut | ✅ Gut | ❌ | ✅ Exzellent | ✅ Gut |
| Linux | ❌ | ✅ Gut | ✅ Gut | ❌ | ❌ | ✅ Gut |
| iOS | ❌ | ❌ | ✅ Gut | ✅ Gut | ✅ Exzellent | ❌ |
| Android | ❌ | ❌ | ✅ Gut | ✅ Gut | ❌ | ❌ |
| Web | ❌ | ❌ | ✅ Gut | ❌ | ❌ | ❌ |
| Performance | Exzellent | Gut | Exzellent | Gut | Exzellent | Exzellent |
| Installationsgröße | ~15 MB | ~80 MB | ~25 MB | ~30 MB | ~10 MB | ~15 MB |


