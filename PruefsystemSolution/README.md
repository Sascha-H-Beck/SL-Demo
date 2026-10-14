# SL-Demo — PruefsystemSolution (WPF)

Eine kleine WPF-Anwendung zur Demonstration und zum Spannungs-/Mess-Testing.

## Was das Projekt ist

- Eine Windows Presentation Foundation (WPF) Desktop-Anwendung mit einer einfachen Benutzeroberfläche für Spannungsmessungen.
- Das Projekt ist mindestens in zwei Teilprojekte aufgeteilt: `App.UI` (die WPF-Oberfläche) und `App.Core` (Kernlogik / Bibliothek).

## Technologien

- .NET 8 (`net8.0-windows`)
- WPF
- C#
- `ScottPlot.WPF` (für Plot-Darstellungen)

## Projektstruktur (relevante Dateien)

- `App.UI/` — WPF-Anwendungsprojekt
  - `App.xaml` — Anwendung XAML
  - `App.xaml.cs` — Partial-Klasse `App` (Code-Behind)
  - `Program.cs` — expliziter Entry-Point (`Main`) in diesem Repository
- `App.Core/` — referenziertes Projekt mit Kernlogik

## Voraussetzungen

- Windows 10 oder 11
- .NET 8 SDK
- Visual Studio 2022/2023 (mit .NET Desktop Development / WPF-Workload) oder das `dotnet` CLI

## Bauen und Ausführen

Mit Visual Studio

1. Öffne die Lösung `PruefsystemSolution.sln` in Visual Studio.
2. Setze `App.UI` als Startprojekt.
3. Clean und Rebuild der Lösung, danach Ausführen (F5).

Mit `dotnet` CLI

1. Vom Lösungs-Verzeichnis aus:

   ```bash
   dotnet restore
   dotnet build
   dotnet run --project App.UI/App.UI.csproj
   ```

## Hinweise und Fehlerbehebung

- WPF erfordert, dass der UI-Thread im STA-Modus läuft. Das Projekt verwendet eine explizite `Program.cs` mit `[STAThread]`, um korrektes Startup sicherzustellen.
- Falls Probleme auftreten, bei denen in der generierten `App.g.cs` eine vollqualifizierte `App.UI.App`-Instanziierung steht und die Anwendung nicht startet, versuche:
  - Die `bin/`- und `obj/`-Verzeichnisse der betroffenen Projekte zu löschen und neu zu bauen.
  - Sicherzustellen, dass in `App.xaml` das Attribut `x:Class="App.UI.App"` korrekt gesetzt ist.
  - Zu prüfen, dass keine weiteren Klassen mit dem Namen `App` in referenzierten Projekten vorhanden sind, die Namenskonflikte verursachen könnten.
  - Zu überprüfen, dass `RootNamespace` in `App.UI/App.UI.csproj` auf `App.UI` gesetzt ist.

## Lizenz

Keine Lizenz angegeben.