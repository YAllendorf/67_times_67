# 67_times_67

		/////////////////
		///  Inhalt  ///
		/////////////////

Durch das Klicken in das eingerahmte Feld können Zellen aktiviert und deaktiviert werden. Die Farbe der Zelle wird durch das Farbkreuz rechts bestimmt. Durch den RGB Regler kann
eine ausgewählte Farbe verändert werden. Bestehende Zellen werden dabei mitverändert.
Oben rechts kann mit dem ersten Slider die Bewegung gestartet, beschleunigt und verlangsamt werden. Ist die Bewegung gestartet, bewegt sich jede Zelle einer Farbe entsprechend
ihrer Position im Farbkreuz. Bewegen sich 2 Zellen in einem Tick in die gleiche Zelle, wird diese zelle weiß. Farbige Zellen, die in eine weiße Zelle wandern, werden zerstört.
Durch klicken auf Speichern wird der aktuelle Zustand als (sehr sehr sehr langer) Text in die Zwischenablage kopiert. Noch ist dafür kein visuelles Feedback implementiert, aber
es funktioniert. Genauso kann durch klicken auf Import die Zwischenablage importiert werden. Auch hier ist noch kein Feedback implementiert.

In examples können die Beispiele kopiert und durch Import aus dem Clipboard eingefügt werden. Es dürfen keine Zeichen außer den Zahlen kopiert werden.

		///////////////////////
		///Eigenanteil///
		//////////////////////

Der Aufbau der Zellen in einem Array bestehend aus einer x und y Komponente durch zweidimensionale For-Schleifen, das PostRendern der Umrandung des Feldes, die
SetStatus-Funktion in Cell.cs und die Funktionen ClearCells(), RandomAliveCell() und RandomCellColor() sind ursprünglich aus folgendem Tutorial:

https://www.youtube.com/watch?v=hXWVDDCSiaA

Das Tutorial habe ich für ein erstes Nachbauen meiner Inspirationsquelle Conway's Game of Life benutzt. Mein Projekt baut darauf auf, wodurch die oben beschriebenen Reste des
Codes aus dem Tutorial noch enthalten sind.



		/////////////////
		///Aufbau///
		/////////////////

In Unity selbst bestehen zu Beginn der GameManager, auf dem das GameManager.cs liegt, einer Kamera und den UI Elementen. Die UI Elemente kommunizieren über Events mit dem
GameManager.cs. Die zellen werden aus einem Prefab erzeugt.


Cell.cs:
Klasse, aus der jeder Pixel besteht. Enthält Informationen zum SpriteRenderer und die entsprechende Farbe. Beim Start wird jeder Zelle ein Array aus 8 Nachbarn zugewiesen.
Anhand dieses Arrays kann jede Zelle für sich die Änderung feststellen. Der Zustand jeder Zelle wird über "SetStatus()" festgelegt. Der Ablauf eines Ticks ist in 3 Phasen
(StartTick, MidTick, EndTick) eingeteilt.
In StartTick zählt jede Zelle die Anzahl ihrer lebenden nachbarn.
In MidTick ermittelt jede Zelle aus den folgenden Variablen ihren Zustand im nächsten Tick:
- ist die Zelle selbst lebend
- welche Farbe hat die Zelle selbst
- lebt die Zelle oben, rechts, unten oder links von ihr
- welche Farben haben die um sie lebenden Zellen

In EndTick wird der ermittelte Zustand umgesetzt.

Nach jeder Phase meldet jede Zelle an den gameManager, dass die Phase vorbei ist. Nur wenn alle Zellen eine Meldung abgegeben haben, startet der GameManager den nächsten Tick.

In UpdateColor() werden Farbänderungen durch die UI umgesetzt.

CheckNeighbours() wird einmalig beim Start ausgeführt und dient der besseren leserlichkeit des Codes. Dadurch kann beispielsweise statt "neighbours[6]" "links" geschrieben
werden.


GameManager:

in Update() werden anhand der Tickgeschwindigkeit Ticks gestartet.
SetupCells() wird einmalig beim Start ausgeführt und baut die Zellen durch eine zweidimensionale for-Schleife auf. Für die Zellen an den Kanten und Ecken des Feldes sind
Ausnahmen für das Ermitteln der Nachbarn geschrieben, damit Zellen wie in Snake über die Feldränder loopen.
In Input() sind die Inputs wie kommentiert hinterlegt.
Außerdem koordiniert der GameManager das löschen aller Zellen, Randomizer für Debugging, das Speichern und Laden eines zustandes und alle UI Events.

GridManager:

Ist für das PostRendern der Umrandung des Feldes zuständig.
