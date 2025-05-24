# SummitSeeker

# Links
 - [Notion](https://www.notion.so/Startseite-148bd82e1ce981cda604d8646e280fa9?pvs=13)

# Project Setup
- Unity Hub: Version 3.10.0 ++
- Unity Editor: Version Unity6 (6000.0.28f1 ++) inkl. __Modules:__ Windows Build Support (IL2CPP), Documentation
- VSCode: Einrichtung entsprechend [Video](https://www.youtube.com/watch?v=ihVAKiJdd40&t=282s)
- Blender: Version 4.3 ++
- Gimp / Paint.net
- GitHub Desktop

# Development
## Planing
### Main Menu
Um im GameManager nur die globalen Spielzustände und -abläufe zu erfassen und gleichzeitig volle Kontrolle über die Zustände im Hauptmenu zu behalten, befindet sich im Bereich des Hauptmenüs ein weiterer Manager - der __MenuManager__. Dieser erfasst ausdrücklich __nur__ diejenigen Zustände, die im Hauptmenu möglich sind, keine globalen Spielzustände und übernimmt im Moment, wenn sich der GameManager im State MainMenuGameState befindet, die Kontrolle. Führt eine Aktion im Hauptmenu durch die Änderung des MenuStates dazu, dass dieses verlassen wird, ändert der MenuManager den GameState beim GameManager von MainMenuGameState zu einem anderen und übergibt somit die Kontrolle zurück an den GameManager.

#### Kontextdiagramm für das MainMenu
![MainMenuGameStateDiagram drawio](https://github.com/user-attachments/assets/8128edbe-3529-47a7-82ea-4d644496f459)


## Coding
### Codeformatter
Bei dem Projekt SummitSeeker verwenden wir den Codeformatter [csharpier](https://csharpier.com).
Die Einhaltung dieser Formatierungsregeln wird auch mittels eines Workflows geprüft.
## Level Design
### Rules
Potentiell ist es möglich, dass bereits an einem höheren Level gearbeitet bzw. fertig gestellt wird, bevor das niedrigere Level fertig und komplett in der Hauptspielscene, in der sich alle Level befinden, integriert wurde. Wird also durch einen Usecase die Erstellung eines neuen Levels gefordert, so wird dieses Level zunächst in einer seperaten Szene gebaut und getestet, um die möglichen Konflikte mit einem anderen Level zu verhindern.
Damit jedoch die korrekte Reihenfolge eingehalten wird, werden alle 5? Level in einem neuen Issue die offenen Level in der Hauptszene, __identisch__ zur Bauweise in den seperaten Level, zusammengeführt.

# Testing
## Rules
Der durch ein Usecase vom Developer neu hinzugefügte Code muss eine Code Coverage von mindestens 85% haben. Dies ist eine gute Mitte zwischen ausreichend getestet und angemessener Zeitaufwand. 

Um schnell mitzubekommen bei welcher Assertion ein Test fehlschlägt, wird der Methoden-Overload verwendet, bei dem eine Message in Form eines Strings mitgegeben werden kann. Diese Nachricht wird **aussagekräftig** gewählt.

## Automatisierte Tests
Zum Testen des in C# geschriebenen Codes verwenden wir das [Unity Test Framework](https://docs.unity3d.com/Packages/com.unity.test-framework@1.1/manual/index.html).
Beschreibung in [Dokumentation](https://summitseekerdevs.github.io/Dokumentation/developer/unity/automatisierte_tests/)

## Tipps / übliche Probleme
Sobald die Unity Engine für einen Test verwendet werden muss, beispielsweise wenn Physik, Update-Methoden oder das InputSystem etc. benötigt werden, dann muss **[UnityTest]** verwendet werden. Geht es andernfalls nur um reine Logiktests, wo die Unity Engine nicht nötig für ist, dann kann **[Test]** verwendet werden

Viele Methoden einer Klasse werden als private deklariert. Dies ist gut um die Sichtbarkeit nur auf diese Klasse zu beschränken. Jedoch führt es zu Problemen beim Testen, da die Methode dadurch auch nicht in der Testklasse sichtbar ist. Um diesem Problem zu entkommen, werden die Methoden statt mit private mit internals deklariert und über der Klassendefinition ein ''[assembly: InternalsVisibleTo("Tests")]'' hinzugefügt, wobei der Inhalt in Anführungsstrichen, der Name der Assembly ist in dem der Test liegt, der die Methode verwenden möchte.

Wenn Press() zur Simulation einer Tasteneingabe verwendet wird, ist zu beachten das diese Taste solange simuliert gedrückt wird, bis Release() verwendet wird. Nur Press() könnte zu ungewünschten Ergebnisse führen.

GameManager ist ein Singleton, damit einher geht, dass wenn ein neuer GameManager erstellt wird, es bereits aber eine static Instance davon gibt, sich der neue GameManager wieder selbst zerstört. Kann gerade bei Setup und TearDown bei Tests zu Fehler führen.
