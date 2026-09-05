# BlueprintEditor2 – für Wine (Linux)
BLUEPRINT EDITOR 2 FÜR LINUX. ES FUNKTIONIERT.
(Diese ReadMe ist auf deutsch und sollte nur als Ergänzugn der englischen Haupt-ReadMe verstanden werden.)

# Building
Die folgenden Buildhinweise sind für das Builden unter Linux:
- Aufsetzen einer Windows (10) VM, weil der Build unter Windows erfolgen muss.
- Installation des MS Visual Studio Installers __2017__ Community Edition.
- Mit dem Installer werden Visual Studio 2017 sowie .NET4 Abhängigkeiten installiert (Alles was wichtig aussieht). Besonders wichtig ist dabei das `.NET4.0 Framework targeting pack` (oder so ähnlich oder irgendwie auf deutsch mit _Zielframework_ oder so).
- Unter Windows muss evtl. noch MSVC Redist 2010 x86 installiert werden (Ich glaube aber nur wenn man den Blueprinteditor unter Windows ausführen will).
- Klonen des Repos unter Windows. 
- Öffnen einer MSVS2017 Developer Console (weil der Compiler nicht im normalen Path ist).
- Mit der Konsole ins Repository wechseln.
- `msbuild BlueprintEditor2.csproj /t:Clean` (zum Beseitigen potentieller Artefakte) und `msbuild BlueprintEditor2.csproj` (zum Builden). Es sollte 28 Warnungen geben.
- Die generierte EXE befindet sich unter `/bin/Debug`.

Die generierte EXE kann unter Linux mit Wine im RailWorks-Ordner ausgeführt werden. Dort sollten alle .dlls bereits vorhanden sein. Um eine Verwechselung vorzubeugen wird empfohlen, die EXE in `BlueprintEditor2.5.exe` umzubenennen.

# Verwendung
Anleitung für die Verwendung mit Proton:
- Starten des Protontricks GUIs.
- Auswählen des Train Simulators (dies dauert eventuell einen Augenblick).
- Option `Standart Wine Prefix auswählen` auswählen.
- `Explorer starten` oder `Wine Cmd starten` auswählen.
- Ins RailWorks-Verzeichnis navigieren (Im Explorer werden Ordner mit `.` am Anfang nicht angezeigt. Um in diese zu Navigieren muss der Ordner in der Adresszeile hinzugefügt werden. Z.B. `\.local`).
- Starten der BlueprintEditor Executable.
- Profit
