# SummitSeeker

# Links
 - Planung: https://app.milanote.com/1TWnhO1ABvU27u/summitseekers?p=OZxuH37wndh
 - [Notion](https://www.notion.so/Startseite-148bd82e1ce981cda604d8646e280fa9?pvs=13)

# Project Setup
- Unity Hub: Version 3.10.0 ++
- Unity Editor: Version Unity6 (6000.0.28f1 ++) inkl. __Modules:__ Windows Build Support (IL2CPP), Documentation
- VSCode: Einrichtung entsprechend [Video](https://www.youtube.com/watch?v=ihVAKiJdd40&t=282s)
- Blender: Version 4.3 ++
- Gimp / Paint.net
- GitHub Desktop

# Workflows
## Tipps
- PackageMode rausnehmen, damit ganzes Projekt getestet wird
- AssemblyFilters entfernen, damit codeCoverage Artifakt erzeugt werden kann

# Development
## Coding
### Codeformatter
Bei dem Projekt SummitSeeker verwenden wir den Codeformatter [csharpier](https://csharpier.com).
Die Einhaltung dieser Formatregeln wird auch mittels eines workflows geprüft.
## Level Design
### Rules
Wird durch ein Usecase die Erstellung eines neuen Levels gefordert, so wird dieses Level zunächst in einer seperaten Szene gebaut und getestet. Dies ist darin begründet, dass gleichzeitig an einander angrenzenden Leveln gearbeitet wird und somit der Fall möglich ist, dass ein höheres Level vor einem niedrigeren fertiggestellt wird. 
Damit jedoch die korrekte Reihenfolge eingehalten wird, werden alle 5? Level in einem neuen Issue die offenen Level in der Hauptszene, __identisch__ zur Bauweise in den seperaten Level, zusammengeführt.

# Testing
[Example Tutorial](https://www.kodeco.com/38240193-introduction-to-unity-unit-testing)
## Rules
Der durch ein Usecase vom Developer neu hinzugefügte Code muss eine Code Coverage von mindestens 85% haben. Dies ist eine gute Mitte zwischen ausreichend getestet und angemessener Zeitaufwand. 

## Automatisierte Tests
Zum Testen des in C# geschriebenen Codes verwenden wir das “Unity Test Framework”.

Dieses Framework unterteilt die Tests in **Edit Mode** - und **Play Mode** - Tests.

Die Tests im **Edit-Mode** laufen im Unity-Editor und haben Zugriff auf den Editor und den Code des Spiels. Dies bedeutet, dass die benutzerdefinierten Editor-Erweiterungen getestet oder Tests verwenden können, um Einstellungen im Editor zu ändern und den Abspielmodus aufzurufen, der nützlich ist, um Inspektorwerte anzupassen und dann automatisierte Tests mit vielen verschiedenen Einstellungen durchzuführen.
Wie der Name schon vermuten lässt, sind diese Art von Tests also allgemein eher um Funktionalität des Unity Editors bzw. Inspektors z.B. eines benutzerdefinierten Inspektors zu testen und nicht geeignet um den eigentlichen Code des Spiels automatisiert zu testen.


Mit den Tests im **Play-Mode** kann der Spielcode zur Laufzeit getestet werden. Tests werden in der Regel als Coroutine unter Verwendung des Attributs **[UnityTest]** ausgeführt. So kann der Code getestet werden, der über mehrere Frames hinweg ausgeführt werden kann. Standardmäßig werden die Tests im Play-Mode im Editor ausgeführt, aber sie können auch in einem eigenständigen Player-Build für verschiedene Zielplattformen ausführen werden.

## Tipps / übliche Problem
Viele Methoden einer Klasse werden als private deklariert. Dies ist gut um die Sichtbarkeit nur auf diese Klasse zu beschränken. Jedoch führt es zu Problemen beim Testen, da die Methode dadurch auch nicht in der Testklasse sichtbar ist. Um diesem Problem zu entkommen, werden die Methoden statt mit private mit internals deklariert und über der Klassendefinition ein ''[assembly: InternalsVisibleTo("Tests")]'' hinzugefügt, wobei der Inhalt in Anführungsstrichen, der Name der Assembly ist in dem der Test liegt, der die Methode verwenden möchte.

Wenn Press() zur Simulation einer Tasteneingabe verwendet wird, ist zu beachten das diese Taste solange simuliert gedrückt wird, bis Release() verwendet wird. Nur Press() könnte zu ungewünschten Ergebnisse führen.

GameManager ist ein Singleton, damit einher geht, dass wenn ein neuer GameManager erstellt wird, es bereits aber eine static Instance davon gibt, sich der neue GameManager wieder selbst zerstört. Kann gerade bei Setup und TearDown bei Tests zu Fehler führen.
