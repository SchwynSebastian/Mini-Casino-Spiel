# Mini-Casino-Spiel

# Beschreibung des Casino-Spiels

Dieses C#-Programm simuliert ein einfaches Casino mit zwei Spielen: **Roulette** und **Blackjack**. Es verwendet das Observer-Pattern, um den Spieler über Änderungen im Guthaben zu informieren.

---

## Hauptbestandteile des Programms

### 1. **Program-Klasse**
- Einstiegspunkt der Anwendung.
- Erzeugt ein `CasinoGame`-Objekt und einen `PlayerObserver`.
- Verknüpft den Observer mit dem Spiel, um Nachrichten zu erhalten.
- Startet das Casino-Spiel.

### 2. **CasinoGame (Subject)**
- Implementiert das Subject-Interface im Observer-Pattern.
- Verwaltet eine Liste von Beobachtern (`observers`).
- Bietet Methoden zur Beobachterverwaltung (`Attach`, `Detach`, `Notify`).
- Steuert den Spielfluss und die Menüführung.
- Bietet zwei Hauptspiele an: Roulette und Blackjack.

### 3. **BalanceManager (Singleton)**
- Verwaltet das Guthaben des Spielers.
- Singleton-Pattern sorgt für eine einzige, globale Instanz.
- Startguthaben beträgt 1000 Chips.
- Balance wird bei Gewinn oder Verlust aktualisiert.

### 4. **PlayerObserver (Observer)**
- Empfängt Benachrichtigungen vom CasinoGame.
- Gibt Nachrichten zum aktuellen Guthaben im Konsolenfenster aus.

---

## Spielfunktionen im Detail

### Roulette
- Spieler setzt einen Einsatz innerhalb seines Guthabens.
- Verschiedene Wettarten möglich: Farbe (rot/schwarz), Zahlenbereiche (1-12, 13-24, 25-36), gerade/ungerade, einzelne Zahlen etc.
- Gewinn wird abhängig von der Wettart berechnet (z.B. 35x bei Zahl, 2x bei Farbe).
- Zufallszahl (0–36) simuliert das Ergebnis.
- Guthaben wird entsprechend Gewinn oder Verlust angepasst.
- Nach jeder Runde kann der Spieler eine neue Runde starten oder ins Menü zurückkehren.

### Blackjack
- Spieler erhält zwei Karten, Dealer eine offene Karte.
- Spieler kann "Hit" (Karte ziehen) oder "Stand" (bleiben) wählen.
- Kartenwerte sind Zahlen von 1 bis 10.
- Dealer zieht Karten, bis er mindestens 17 Punkte erreicht.
- Gewinner wird anhand der Punktzahl bestimmt (höchstens 21 Punkte).
- Guthaben wird entsprechend angepasst.
- Spieler kann nach einer Runde erneut spielen oder ins Menü zurückkehren.

---

## Architektur und Designmuster

- **Observer-Pattern:** Trennt Spiel-Logik und UI-Feedback. Observer zeigt Guthabenänderungen sofort an.
- **Singleton-Pattern:** `BalanceManager` garantiert eine zentrale, konsistente Verwaltung des Spielguthabens.
- **Klar strukturierte Menüführung:** Erlaubt einfache Auswahl der Spiele oder Beenden.
- **Einfache Spielmechaniken:** Zufallsgeneratoren bestimmen Ergebnisse, keine komplexe Kartensimulation oder erweiterte Regeln.

---

## Fazit

Das Programm ist ein einfaches, konsolenbasiertes Casino mit zwei beliebten Spielen. Es demonstriert Designmuster wie Observer und Singleton sowie grundlegende Spiellogik in C#. Es eignet sich als Lernprojekt für Spieleprogrammierung und Entwurfsmuster in der Softwareentwicklung.
