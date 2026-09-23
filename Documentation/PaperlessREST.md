# PaperlessREST Dokumentation

## Architektur
Die Anwendung verwendet eine klassische **N-Tier (Schichten) Architektur**, um eine saubere Trennung der Zuständigkeiten (Separation of Concerns) zu gewährleisten:

*   **API Layer (`PaperlessREST.Api`)**: Behandelt eingehende HTTP-Anfragen mittels .NET Minimal APIs. Zuständig für Routing, Parameter-Binding und Rückgabe entsprechender HTTP-Statuscodes.
*   **Business Logic Layer (`PaperlessREST.Bll`)**: Enthält die Geschäftslogik der Anwendung (`DocumentService`, `ShareService`). Sie verarbeitet die Daten zwischen der API und der Datenzugriffsschicht. Inkludiert sowohl Dokumenthandling, als auch Share Link Management.
*   **Data Access Layer (`PaperlessREST.Dal`)**: Zuständig für die Datenbankkommunikation mithilfe von **Entity Framework Core**. Verwendet das Repository-Pattern (`DocumentMetaRepository`, `ShareRepository`), um auf die Datenbank (`DbContext`) zuzugreifen.
*   **Models (`PaperlessREST.Models`)**: Definiert die Entitäten / Datenmodelle für die Datenbank und den Datenaustausch.
*   **Tests (`PaperlessREST.Test.*`)**: Für jede Schicht existieren separate Unit-Test-Projekte (Api, Bll, Dal), was eine hohe Testabdeckung und Qualitätssicherung ermöglicht.

## Datenmodelle (Models)

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

## Endpoints

Siehe den [OpenAPI Spec](./PaperlessRESTApi.yaml).

## Offene Punkte & Fehlende Implementierungen (To-Dos)
Während der Großteil der Struktur, Dependency Injection, Routen und Metadaten-Verwaltung erfolgreich implementiert ist, fehlen aktuell die Implementierungen für die eigentliche Verarbeitung der physischen Dateien. Folgende Endpunkte/Methoden werfen derzeit eine `NotImplementedException`:
*   **Abrufen von Dokumenteninhalten** (`GetDocumentContentAsync` im `DocumentService` & `DocumentEndpoint`), weil der Dokumenteninhalt nicht in der PostgreSQL-Datenbank gespeichert wird.
*   **Abrufen von Dokumenteninhalten über Freigabelinks** (`AccessShareLinkContent` im `ShareEndpoint`), siehe oben.
*   **Suche nach Dokumenten** (`SearchDocumentsAsync` im `DocumentService` & `DocumentEndpoint`), weil das zukünftig über ein externes Service geaddet wird.
*   **Einige Security Features** (z.B. salting von Passwörtern damit sie nicht in Logs vorkommen)
*   **Clean up der Configurations** (z.B. `appsettings.json`), derzeit sind manche Konfigurationen noch für einfachere dev environments eingetragen
