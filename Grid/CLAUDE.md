# Grid Indicator v1.5 - cTrader

## Descrizione Generale

Questo indicatore per cTrader disegna una griglia di linee orizzontali sul grafico di trading, fornendo riferimenti visivi per i livelli di prezzo a intervalli regolari misurati in pips.

## Funzionalità Principali

### 1. Griglia Dinamica
- Disegna linee orizzontali sul grafico a intervalli regolari definiti dall'utente
- La griglia si adatta automaticamente ai cambiamenti di zoom, scroll e dimensioni del grafico
- Calcolo intelligente del numero di linee per evitare sovraccarico visivo

### 2. Parametri Configurabili

**Gruppo Grid:**
- **Maximum lines on screen**: Numero massimo di linee da visualizzare (default: 40, min: 10)
  - Se il numero di linee supera questo limite, la grid size viene aumentata automaticamente
- **Grid color**: Colore delle linee della griglia (default: Gray)

**Gruppo UI:**
- **Settings button position**: Posizione del pulsante di impostazioni sul grafico
  - Opzioni: TopLeft, TopRight, TopCenter, BottomLeft, BottomRight, BottomCenter, CenterLeft, CenterRight
- **Button offset X/Y**: Offset in pixel del pulsante dalla posizione scelta

### 3. Impostazioni Persistenti

L'indicatore salva le impostazioni in modo persistente usando LocalStorage con chiave univoca:
- **Broker**: Nome del broker
- **Asset**: Simbolo dello strumento finanziario
- **TimeFrame**: Timeframe del grafico

Questo permette di avere impostazioni diverse per ogni combinazione di asset e timeframe.

### 4. Interfaccia Utente Interattiva

**Pulsante Settings:**
- Mostra la dimensione corrente della griglia (es. "Grid Settings\n(30p)")
- Cliccandolo si apre un dialog per modificare le impostazioni

**Dialog Impostazioni:**
- **Zero Line**: Linea di riferimento da cui parte la griglia
  - 0 = auto-calcolata (usa il prezzo medio del grafico visibile)
  - Valore specifico = linea fissa a quel prezzo
- **Grid Size**: Distanza tra le linee in pips (default: 30)
- Pulsanti Save/Cancel per confermare o annullare le modifiche

### 5. Propagazione Cross-Timeframe

Funzionalità intelligente che propaga la ZeroLine tra timeframe diversi dello stesso asset:
- Se si cambia timeframe sullo stesso simbolo, la ZeroLine viene copiata automaticamente
- Questo mantiene coerenza visiva quando si analizzano diversi timeframe dello stesso strumento

### 6. Calcolo Adattivo della Grid Size

Il metodo `CalcGridSize()` (Grid - Indicator.cs:28) calcola automaticamente la dimensione ottimale della griglia:
- Controlla quante linee sarebbero visibili con la grid size attuale
- Se superano `MaxLinesOnScreen`, aumenta automaticamente la grid size
- Usa il metodo `FindClosestRoundNumber()` per arrotondare a valori "puliti"

### 7. Gestione Eventi del Grafico

L'indicatore si aggiorna automaticamente in risposta a (Grid - Events.cs):
- `Chart_SizeChanged`: Ridimensionamento della finestra
- `Chart_ZoomChanged`: Cambio di zoom
- `Chart_ScrollChanged`: Scroll orizzontale
- `Chart_VisibilityChanged`: Cambio di visibilità del grafico

## Struttura del Codice

Il codice è organizzato in file parziali (partial class) per separare le responsabilità:

- **Grid.cs**: File principale, interfacce IInstrument/IMarketInfo, metodo Calculate
- **Grid - BASE.cs**: Classe base (attualmente vuota)
- **Grid - Parameters.cs**: Definizione parametri e enum
- **Grid - Init.cs**: Metodi di inizializzazione e stampa informazioni
- **Grid - Indicator.cs**: Logica di disegno della griglia
- **Grid - UI.cs**: Interfaccia utente (pulsante e dialog)
- **Grid - Events.cs**: Gestori eventi del grafico
- **Grid - Storage.cs**: Persistenza delle impostazioni

## Logica di Disegno (Grid - Indicator.cs)

Il metodo `DrawGrid()` (Grid - Indicator.cs:38) implementa la logica principale:

1. Verifica che il grafico sia pronto (altezza > 0, TopY e BottomY validi)
2. Cancella la griglia precedente con `ClearGrid()`
3. Calcola la grid size effettiva con `CalcGridSize()`
4. Determina la linea zero (watershedPrice)
5. Calcola i limiti visibili del grafico (topY, bottomY)
6. Disegna le linee dall'alto verso il basso nell'area visibile
7. Usa un limite di sicurezza di 100 linee massime

## Valori di Default

Definiti come costanti (Grid - Parameters.cs:30):
- `DEFAULT_ZERO_LINE = 0` (auto-calcolata)
- `DEFAULT_GRID_SIZE = 30` pips

## Dipendenze

L'indicatore utilizza librerie esterne:
- `Library`: Funzionalità generiche
- `LibraryLog`: Sistema di logging
- `LibraryExtensions`: Metodi di estensione per Chart (es. `HeightInPips()`, `MiddlePrice()`)

## Note Tecniche

- Versione: 1.5
- TimeZone: UTC
- AccessRights: None
- IsOverlay: true (si sovrappone al grafico dei prezzi)
- Target Framework: .NET 6.0

## 🏗️ Principi Architetturali

### Organizzazione Classi

1. **Classi Separate per Entità**
   - Ogni entità ha la propria classe dedicata
   - File separato per ogni classe
   - Favorisce testabilità, manutenibilità e separazione delle responsabilità

2. **Partial Class solo per la Classe Principale**
   - La classe dell'indicatore può essere suddivisa in partial class

3. **Non usare Partial Class per Entità aggiuntive**

Questo approccio:
- Elimina duplicazione di codice
- Facilita l'aggiunta di nuove formazioni (Ledge)
- Mantiene consistenza visiva tra formazioni simili

### Namespace

1. **Classe Principale in `cAlgo`**
   - Se crei classi (oltre quella dell'indicatore), definisci un namespace separato da `cAlgo` (richiesto da cTrader)
   - Tutte le classi aggiuntive useranno il nuovo il namespace
   - Questo separa la logica di dominio dal framework cTrader
   - Importare con `using <namespace>;` nei file che ne hanno bisogno

### SOLID Principles

Il progetto segue rigorosamente i principi SOLID:

1. **Single Responsibility Principle (SRP)**
2. **Open/Closed Principle**
3. **Liskov Substitution Principle**
4. **Interface Segregation Principle**
5. **Dependency Inversion Principle**

---

## 🎯 Regole di Codifica

### Naming Conventions

- **Campi privati:** `_camelCase` (es. `_congestionZones`)
- **Proprietà pubbliche:** `PascalCase` (es. `CongestionColor`)
- **Metodi:** `PascalCase` (es. `ProcessBar()`)
- **Parametri:** `PascalCase` nei `[Parameter]`, `camelCase` nei metodi
- **Costanti:** `UPPER_CASE` (es. `MIN_BARS_IN_CONGESTION`)

### Documentazione

- **Tutti i metodi** devono avere `/// <summary>` XML documentation
- **Parametri complessi** devono avere descrizioni dettagliate
- **Logica non ovvia** deve essere commentata inline

### Gestione Oggetti Grafici

**IMPORTANTE:** Tutti gli oggetti grafici (`Chart.Draw*`) devono essere tracciati:

```csharp
// ✅ CORRETTO
zone.Rectangle = Chart.DrawRectangle(...);

// ❌ SBAGLIATO (memory leak)
Chart.DrawRectangle(...);
```

**Cleanup obbligatorio:**
```csharp
if (zone.Rectangle != null)
{
    Chart.RemoveObject(zone.Rectangle.Name);
    zone.Rectangle = null;
}
```

---

## 🔢 Versioning

### Formato Semantico: `major.minor.fix`

- **major:** Cambiamenti architetturali importanti
- **minor:** Nuove funzionalità
- **fix:** Bug fix e piccoli miglioramenti

### Regole di Aggiornamento

**SEMPRE** aggiornare versione quando:
- ✅ Aggiungi nuove funzionalità
- ✅ Correggi bug
- ✅ Modifichi comportamento esistente
- ✅ Aggiungi parametri configurabili

**Aggiorna il changelog in un file separato dal nome CHANGELOG.md:**
```csharp
/// Changelog:
/// 1.1.0 - Refactoring: classe base RangeFormation, rendering unificato
/// 1.0.0 - Implementazione iniziale: Congestion e Trading Range
```

---

## 📝 Unicode e Logging

### Caratteri Speciali

**SEMPRE** usare escape sequences `\uxxxx`:

```csharp
// ✅ CORRETTO
Print("\u2554\u2550\u2557"); // ╔═╗

// ❌ SBAGLIATO (problemi rendering)
Print("╔═╗");
```

### Box Drawing Characters

- Top-left: `\u2554` (╔)
- Top-right: `\u2557` (╗)
- Bottom-left: `\u255A` (╚)
- Bottom-right: `\u255D` (╝)
- Horizontal: `\u2550` (═)
- Vertical: `\u2551` (║)

---

## 🔧 cTrader Specifics

### Vincoli Piattaforma

- **AccessRights:** `None` (no file system, no network)
- **LocalStorage:** Disponibile per persistenza dati
- **No reflection:** Limitazioni su runtime type inspection

### Best Practices

1. **Evita `Thread.Sleep()`** → Blocca UI
2. **Non rimuovere oggetti grafici** se non necessario
3. **Traccia tutti gli oggetti** creati dinamicamente

### Gestione Indici Candele

**IMPORTANTE:** cTrader reindicizza periodicamente le candele durante refresh/reload del grafico. Gli indici delle barre **NON sono stabili** nel tempo.

**Regola fondamentale:**
- ❌ **NON memorizzare** indici delle candele (`int index`)
- ✅ **Memorizzare** DateTime delle candele (`DateTime barTime`)

```csharp
// ❌ SBAGLIATO - L'indice può cambiare dopo refresh
public int MeasuringBarIndex { get; }

// ✅ CORRETTO - Il DateTime è stabile
public DateTime MeasuringBarTime { get; }
```

**Quando serve l'indice**, ricavarlo al momento:
```csharp
int index = Bars.OpenTimes.GetIndexByTime(barTime);
```

Questo garantisce che l'indicatore funzioni correttamente anche dopo lunghi periodi di esecuzione.

---

## ⚠️ Common Pitfalls

### Memory Leaks

```csharp
// ❌ MEMORY LEAK
for (int i = 0; i < 1000; i++)
{
    Chart.DrawLine(...); // Non tracciato!
}

// ✅ CORRETTO
private List<ChartObject> _lines = new List<ChartObject>();
_lines.Add(Chart.DrawLine(...));
```

### Index Out of Bounds

```csharp
// ❌ PERICOLOSO
int idx = zone.MeasuringBarIndex + 2;
var time = Bars.OpenTimes[idx]; // Può crashare!

// ✅ SICURO
if (zone.MeasuringBarIndex + 2 < Bars.Count)
{
    var time = Bars.OpenTimes[zone.MeasuringBarIndex + 2];
}
```

### Performance

```csharp
// ❌ LENTO (ogni tick)
foreach (var zone in AllZones)
{
    Chart.RemoveObject(...);
    Chart.DrawRectangle(...);
}

// ✅ VELOCE (solo se necessario)
if (zone.NeedsUpdate)
{
    UpdateRectangle(zone);
}
```

---

## 📖 Riferimenti

- [cAlgo API Reference](https://help.ctrader.com/ctrader-algo/api-reference/)
- [cTrader API Documentation](https://help.ctrader.com/ctrader-algo/)
- [SOLID Principles](https://en.wikipedia.org/wiki/SOLID)
- [C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)

---

**Nota:** Questo documento deve essere aggiornato ad ogni cambiamento significativo nell'architettura o nelle convenzioni del progetto.