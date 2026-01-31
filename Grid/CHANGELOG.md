# Changelog - Grid Indicator

Tutte le modifiche significative a questo progetto saranno documentate in questo file.

Il formato è basato su [Keep a Changelog](https://keepachangelog.com/it/1.0.0/),
e questo progetto aderisce al [Semantic Versioning](https://semver.org/lang/it/).

---

## [1.5.1] - 2026-01-31

### Fixed
- **ClearGrid()**: Risolto bug che impediva la rimozione completa delle linee precedenti quando si modificava la grid size
  - Problema: Modificare la collezione `Chart.Objects` durante l'iterazione causava il salto di alcuni elementi
  - Soluzione: Materializzazione della query con `.ToList()` prima dell'iterazione
  - Impatto: Eliminata la sovrapposizione di linee quando si passa da grid size piccola a grande

---

## [1.5.0] - Data precedente

### Features
- Griglia dinamica di linee orizzontali sul grafico
- Parametri configurabili: grid size, colore, numero massimo linee
- Interfaccia utente interattiva con pulsante e dialog impostazioni
- Persistenza impostazioni per Broker+Asset+TimeFrame
- Propagazione cross-timeframe della ZeroLine
- Calcolo adattivo della grid size in base al numero massimo di linee
- Aggiornamento automatico della griglia su eventi del grafico (zoom, scroll, resize)
