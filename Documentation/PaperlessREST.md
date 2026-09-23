# PaperlessREST Dokumentation

## 1. Einleitung
Die **PaperlessREST** Solution ist eine auf .NET basierende REST-API-Anwendung zur Verwaltung von Dokumenten und deren Metadaten, sowie zur sicheren Freigabe von Dokumenten (Share Links).

## 2. Architektur
Die Anwendung verwendet eine klassische **N-Tier (Schichten) Architektur**, um eine saubere Trennung der Zuständigkeiten (Separation of Concerns) zu gewährleisten:

*   **API Layer (`PaperlessREST.Api`)**: Behandelt eingehende HTTP-Anfragen mittels .NET Minimal APIs. Zuständig für Routing, Parameter-Binding und Rückgabe entsprechender HTTP-Statuscodes.
*   **Business Logic Layer (`PaperlessREST.Bll`)**: Enthält die Geschäftslogik der Anwendung (`DocumentService`, `ShareService`). Sie verarbeitet die Daten zwischen der API und der Datenzugriffsschicht.
*   **Data Access Layer (`PaperlessREST.Dal`)**: Zuständig für die Datenbankkommunikation mithilfe von **Entity Framework Core**. Verwendet das Repository-Pattern (`DocumentMetaRepository`, `ShareRepository`), um auf die Datenbank (`DbContext`) zuzugreifen.
*   **Models (`PaperlessREST.Models`)**: Definiert die Entitäten / Datenmodelle für die Datenbank und den Datenaustausch.
*   **Tests (`PaperlessREST.Test.*`)**: Für jede Schicht existieren separate Unit-Test-Projekte (Api, Bll, Dal), was eine hohe Testabdeckung und Qualitätssicherung ermöglicht.

## 3. Datenmodelle (Models)

### MetaData
Speichert die grundlegenden Metadaten eines Dokuments in der Datenbank.
*   `Id` (string): Eindeutige Kennung.
*   `Filename` (string): Dateiname des Dokuments.
*   `Description` (string, optional): Beschreibung des Dokuments.
*   `Updated` (DateTime): Zeitstempel der letzten Aktualisierung (Standardmäßig aktuelle UTC-Zeit).
*   `Author` (string, optional): Ersteller oder Autor des Dokuments.

### ShareLink
Verwaltet Freigabelinks für Dokumente, die durch ein Passwort und ein Ablaufdatum geschützt sind.
*   `Guid` (string, Primary Key): Eindeutige ID des Freigabelinks (wird automatisch als GUID generiert).
*   `Password` (string): Passwort zum Schutz des Links.
*   `ExpireDate` (DateTime): Ablaufdatum des Links.
*   `DocumentId` (string): Referenz auf das freigegebene Dokument.

## 4. REST API Endpunkte

Die API-Endpunkte sind gruppiert unter `/api`. Die Endpunkte nutzen DTOs (z. B. `MetaDataDto`) für die sichere Datenübertragung, um die internen Datenbankmodelle abzukapseln.

### Document API (`/api/document`)
Endpunkte zur Verwaltung von Dokumenten.

*   **`POST /api/document/`**
    *   **Beschreibung:** Erstellt ein neues Dokument.
    *   **Parameter:** Erwartet Formulardaten (Form-Data) mit `data` (Dateiinhalt) und `metadata` (JSON-String mit Metadaten).
    *   **Rückgabe:** `201 Created` mit der generierten ID oder `400 Bad Request`.

*   **`GET /api/document/{id}/metadata`**
    *   **Beschreibung:** Ruft die Metadaten eines Dokuments anhand seiner ID ab.
    *   **Rückgabe:** `200 OK` mit den Metadaten oder `404 Not Found`.

*   **`GET /api/document/{id}/content`**
    *   **Beschreibung:** Ruft den eigentlichen Inhalt (die Datei) eines Dokuments ab.
    *   **Status:** ⚠️ *Derzeit nicht implementiert (wirft `NotImplementedException`).*

*   **`PUT /api/document/{id}`**
    *   **Beschreibung:** Aktualisiert die Metadaten eines bestehenden Dokuments.
    *   **Parameter:** Formulardaten mit `data` und `metadata` (JSON-String).
    *   **Rückgabe:** `200 OK` bei Erfolg, `404 Not Found` (wenn Dokument nicht existiert) oder `400 Bad Request`.

*   **`DELETE /api/document/{id}`**
    *   **Beschreibung:** Löscht ein Dokument anhand seiner ID.
    *   **Rückgabe:** `200 OK` bei Erfolg, `404 Not Found` oder `400 Bad Request`.

### Search API (`/api/search`)
Endpunkt für die Suchfunktionalität.

*   **`GET /api/search/?query={Suchbegriff}`**
    *   **Beschreibung:** Sucht nach Dokumenten basierend auf dem übergebenen Query-String.
    *   **Status:** ⚠️ *Derzeit nicht implementiert (wirft `NotImplementedException`).*

### Share API (`/api/share`)
Endpunkte zur Verwaltung und zum Zugriff auf passwortgeschützte Freigabelinks.

*   **`POST /api/share/`**
    *   **Beschreibung:** Erstellt einen neuen sicheren Freigabelink für ein Dokument.
    *   **Body (JSON):** `ShareLink` Objekt (benötigt zwingend `DocumentId`, `Password` und `ExpireDate`).
    *   **Rückgabe:** `200 OK` mit dem generierten Freigabelink-String.

*   **`POST /api/share/{link}/metadata`**
    *   **Beschreibung:** Ruft die Metadaten eines Dokuments über einen Freigabelink ab.
    *   **Body:** Das benötigte Passwort (als reiner String).
    *   **Rückgabe:** `200 OK` mit den Metadaten.

*   **`POST /api/share/{link}/content`**
    *   **Beschreibung:** Ruft den Dokumenteninhalt über einen geschützten Freigabelink ab.
    *   **Body:** Das benötigte Passwort (als reiner String).
    *   **Status:** ⚠️ *Derzeit nicht implementiert (wirft `NotImplementedException`).*

## 5. Offene Punkte & Fehlende Implementierungen (To-Dos)
Während der Großteil der Struktur, Dependency Injection, Routen und Metadaten-Verwaltung erfolgreich implementiert ist, fehlen aktuell die Implementierungen für die eigentliche Verarbeitung der physischen Dateien. Folgende Endpunkte/Methoden werfen derzeit eine `NotImplementedException`:
*   **Abrufen von Dokumenteninhalten** (`GetDocumentContentAsync` im `DocumentService` & `DocumentEndpoint`).
*   **Suche nach Dokumenten** (`SearchDocumentsAsync` im `DocumentService` & `DocumentEndpoint`).
*   **Abrufen von Dokumenteninhalten über Freigabelinks** (`AccessShareLinkContent` im `ShareEndpoint`).

## 6. Verwendete Technologien & Frameworks
*   **Framework:** .NET 10.0 (C#)
*   **Web API:** ASP.NET Core Minimal APIs
*   **ORM (Datenbankanbindung):** Entity Framework Core
*   **Serialisierung:** System.Text.Json
