# SummitSeeker

# Links
- [Game Documentation](https://summitseekerdevs.github.io/Dokumentation/)

# Project Setup
- Unity Hub: Version 3.10.0 ++
- Unity Editor: Version Unity6 (6000.0.28f1 ++) inkl. __Modules:__ Windows Build Support (IL2CPP), Documentation
- VSCode: Einrichtung entsprechend [Video](https://www.youtube.com/watch?v=ihVAKiJdd40&t=282s)
- Blender: Version 4.3 ++
- Gimp / Paint.net
- GitHub Desktop

## Frameworks
Das Projekt verwendet Extenject (ehemals Zenject) als DI-Framework (Dependency Injection), sodass der Code einfach, skalierbar und flexibel geschrieben als auch wiederverwendet, refaktoriesiert und getestet werden kann. Extenject ist ein Fork von Zenject, mit dem Ziel auch weiterhin unterstützt zu werden.
Da eine Erläuterung der Funktionsweise und Nutzung von Extenject an dieser Stelle zu weit führen würde, kann dies auf der Frameworkeigenen Github-Page von [Extenject](https://github.com/Mathijs-Bakker/Extenject?tab=readme-ov-file#what-is-dependency-injection) nachgelesen werden.

Durch das schreiben von lose gekoppeltem Code mithilfe von DI, gelingt es leichter, konkrete Bereiche der Codebasis für automatisierte Tests zu isolieren, ohne das gesamte Projekt dabei starten zu müssen. Um eine Klasse unabhängig von ihren Abhängigkeiten testen zu können, lassen sich diese durch Mocks ersetzen, sodass die Klasse isoliert getestet werden kann.
Um das Schreiben sämtlicher Mocking-Klassen zu vermeiden, ermöglicht Extenject die Automatisierung dieses Prozesses mithilfe einer Mocking-Library, welche diese Arbeit übernimmt. In diesem Projekt wurde sich für die Mocking-Library [__Moq__](https://github.com/devlooped/moq) entschieden.

# Development
## Planing
### Main Menu
Um im GameManager nur die globalen Spielzustände und -abläufe zu erfassen und gleichzeitig die volle Kontrolle über die Zustände im Hauptmenü zu behalten, befindet sich im Bereich des Hauptmenüs ein weiterer Manager - der __MenuManager__. Dieser erfasst ausdrücklich __nur__ diejenigen Zustände, die im Hauptmenu möglich sind, keine globalen Spielzustände und übernimmt im Moment, wenn sich der GameManager im State MainMenuGameState befindet, die Kontrolle. Führt eine Aktion im Hauptmenu durch die Änderung des MenuStates dazu, dass dieses verlassen wird, ändert der MenuManager den GameState beim GameManager von MainMenuGameState zu einem anderen und übergibt somit die Kontrolle zurück an den GameManager.

## Coding
### Codeformatter
Bei dem Projekt SummitSeeker verwenden wir den Codeformatter [csharpier](https://csharpier.com).
Die Einhaltung dieser Formatierungsregeln wird auch mittels eines Workflows geprüft.

### Extenject (Zenject)
Es ist ein Dependency-Injection Framework welches in Unity nutzbar ist. Das Ziel der Verwendung von Extenject ist weniger manuelles Instanziieren und Zuweisen von Objekten, dafür eben mehr Modularität, Testbarkeit und sauberen Code. Extenject sorgt dafür, dass Klassen beim Erstellen automatisch ihre benötigten Abhängigkeiten bekommen (z.B. über den Konstruktor - nur bei __nicht__-monobehaviour Klassen - oder beispielsweise über [__Method-Injection__](https://github.com/modesttree/Zenject?tab=readme-ov-file#injection) bei der eine Funktion der monobehaviour-Klasse mit der Annotation [Inject] versehen wird.
In der Kernidee beschreibt man in sogenannten [__Installern__](https://github.com/modesttree/Zenject?tab=readme-ov-file#installers), wie Objekte, innerhalb eines [DI-Containers](https://github.com/modesttree/Zenject?tab=readme-ov-file#installers) erstellt und verbunden werden sollen. Der Container ist prinzipiell ein Objekt, welches ein Dictionary mit allen Registrierungen enthält. Registrierungen werden in Extenject __Bindings__ genannt, da sie die Bindung von einem abstrakten zu einem konkreten Typ erstellen.

Der sogenannte __Kontext__, in dem dieser __Installer__ aufgerufen wird, bestimmt dabei den Lebensbereicht der Abhängigkeiten und wann sie verfügbar sind:
  - __Projekt-Kontext__: globale Abhängigkeiten für das ganze Projekt (also Objekte die jederzeit von theoretisch jedem anderen Objekt abrufbar sein müssen wie z.B. der GameManager)
  - __Szenen-Kontext__: Abhängigkeiten, die nur in einer bestimmten Szene gelten (z.B. das eigentliche Spielerobjekt -> Soll ja nicht im Hauptmenu sein, beispielsweise aber in der InGame-Szene)
  - __Gameobject-Kontext__: Abhängigkeiten für ein einzelnes Objekt oder Prefab (z.B. bei der Katapult-Platform, bei der alle notwendigen Klassen und Hilfsklassen nur genau einmal in dessen Kontext existieren und deren Funktionen verwendbar sein sollen)

Trotz der Verwendung von Extenject, werden wir bei Abhängigkeiten die eine Unity-spezifische Komponente (z.B. Rigidbody oder Transform etc.) betreffen und an einem Gameobjekt hängen, weiterhin die für die Auflösung dieser Komponenten-Abhängigkeiten vorgesehenen Unity-Methoden wie GetComponent verwenden.

#### Wann Unity-Lebenszyklus und wann Extenject-Lebenszyklus
Durch die Verwendung von Monobehaviour wird der Lebenszyklus einer Klasse/Objekt durch Unity gesteuert (Awake, Start, OnDestroy, Update etc.). Dabei muss diese Klasse/Komponente an einem Gameobjekt hängen und hat direkten Zugriff auf Unity-spezifische Dinge wie transform, gameObject, StartCoroutine etc. Das ist praktisch, wenn die Logik __untrennbar mit einem GameObject verbunden__ ist (z.B. Bewegung des Spielers mit einem Rigidbody)

Andererseits gibt es aber auch die Möglichkeit den Lebenszyklus einer Klasse durch Extenject steuern zu lassen, beispielsweise durch die Verwendung der Intefaces [IInitializable, IDisposable oder ITickable](https://github.com/modesttree/Zenject?tab=readme-ov-file#installers). Hierbei wird kein Gameobject benötigt und die Klasse kann völlig unabhängig von Unity-Komponenten existieren. Diese Verwendung ist praktisch, wenn die Klasse reine Spiellogik enthält, die nicht von Unity-Objekten abhängig ist, beispielsweise der GameManager.

#### Signals
Extenject bietet die Möglichkeit der Verwendung von sogenannten [__Signals__](https://github.com/modesttree/Zenject/blob/master/Documentation/Signals.md#signals), welche ein __Event-System innerhalb von Extenject__ (Observer-Pattern) sind. Dies dient zum __Entkoppeln__ von Sender und Empfänger ohne dass direkte Referenzen zwischen ihnen bestehen müssen. Zum Beispiel kann ein Gameobject ein monobehaviour-Skript als Komponente besitzen in dem bei dem Eintritt einer Kollision ein spezifisches Extenject-Signal verschickt wird, auf welches wiederum die verschiedensten Klassen/Komponenten hören und darauf reagieren können (z.B. auslösen, dass die Callable-Platform anfängt sich zu ihrer Zielposition zu bewegen). Ein großer Vorteil der hieraus entsteht, ist weniger enge Kopplung.

## Level Design
### Rules
Potentiell ist es möglich, dass bereits an einem höheren Level gearbeitet bzw. fertig gestellt wird, bevor das niedrigere Level fertig und komplett in der Hauptspielscene, in der sich alle Level befinden, integriert wurde. Wird also durch einen Usecase die Erstellung eines neuen Levels gefordert, so wird dieses Level zunächst in einer seperaten Szene gebaut und getestet, um die möglichen Konflikte mit einem anderen Level zu verhindern.
Damit jedoch die korrekte Reihenfolge eingehalten wird, werden alle Level in einem neuen Issue die offenen Level in der Hauptszene, __identisch__ zur Bauweise in den seperaten Level, zusammengeführt.

# Testing
## Rules
Der durch ein Usecase vom Developer neu hinzugefügte Code muss eine Code Coverage von mindestens 85% haben. Dies ist eine gute Mitte zwischen ausreichend getestet und angemessener Zeitaufwand. 

Um schnell mitzubekommen bei welcher Assertion ein Test fehlschlägt, wird der Methoden-Overload verwendet, bei dem eine Message in Form eines Strings mitgegeben werden kann. Diese Nachricht wird **aussagekräftig** gewählt.

## Automatisierte Tests
Zum Testen des in C# geschriebenen Codes verwenden wir das [Unity Test Framework](https://docs.unity3d.com/Packages/com.unity.test-framework@1.1/manual/index.html).
Beschreibung in [Dokumentation](https://summitseekerdevs.github.io/Dokumentation/developer/unity/automatisierte_tests/)

## Tipps / übliche Probleme
Sobald die Unity Engine für einen Test verwendet werden muss, beispielsweise wenn Physik, Update-Methoden oder das InputSystem etc. benötigt werden, dann muss **[UnityTest]** verwendet werden. Geht es andernfalls nur um reine Logiktests, wo die Unity Engine nicht nötig für ist, dann kann **[Test]** verwendet werden

Die privaten Methoden einer Klasse sollten statt mit `private` mit `internals` deklariert und über die Klassendefinition ein ''[assembly: InternalsVisibleTo("Tests")]'' hinzugefügt werden, um sie für die Tests "sichtbar" zu machen. Der Inhalt in Anführungsstrichen ist der Name der Assembly, in dem der Test liegt, der die Methode verwenden möchte.

Wenn `Press()` zur Simulation einer Tasteneingabe verwendet wird, ist zu beachten, dass diese Taste solange simuliert gedrückt wird, bis Release() verwendet wird. Nur `Press()` könnte zu ungewünschten Ergebnissen führen.

GameManager ist ein Singleton, damit einher geht, dass wenn ein neuer GameManager erstellt wird, es bereits aber eine static Instance davon gibt, sich der neue GameManager wieder selbst zerstört. Kann gerade bei Setup und TearDown bei Tests zu Fehler führen.
