# Grid Indicator for cTrader

[![Version](https://img.shields.io/badge/version-1.5.1-blue.svg)](CHANGELOG.md)
[![Platform](https://img.shields.io/badge/platform-cTrader-green.svg)](https://ctrader.com)
[![Framework](https://img.shields.io/badge/.NET-6.0-purple.svg)](https://dotnet.microsoft.com)

Grid indicator for cTrader platform that displays horizontal grid lines at regular pip intervals on trading charts.

![License](https://img.shields.io/badge/license-MIT-orange.svg)

## Features

- **Dynamic Grid**: Horizontal lines at configurable pip intervals
- **Interactive UI**: Settings button and dialog for easy configuration
- **Persistent Settings**: Saves settings per Broker+Asset+TimeFrame combination
- **Cross-Timeframe Sync**: Zero line propagates automatically between timeframes
- **Adaptive Sizing**: Automatically adjusts grid spacing to avoid clutter
- **Auto-Update**: Refreshes on zoom, scroll, and resize events

## Installation

### Via cTrader Automate

1. Open cTrader
2. Go to Automate → Indicators
3. Click "New" → "Import"
4. Select the `Grid.cs` file (or copy all `Grid - *.cs` files)
5. Click "Build"

### Manual Installation

1. Clone this repository:
   ```bash
   git clone https://github.com/mauriziolobello/ctrader-grid-indicator.git
   ```

2. Copy all `Grid - *.cs` files to your cTrader Indicators folder:
   ```
   %USERPROFILE%\Documents\cAlgo\Sources\Indicators\Grid\
   ```

3. Open cTrader Automate and the indicator will appear in the list

## Usage

1. Add the indicator to your chart
2. Configure parameters:
   - **Maximum lines on screen**: Maximum grid lines before auto-scaling (default: 40)
   - **Grid color**: Color of grid lines (default: Gray)
   - **Settings button position**: Where to place the settings button
   - **Button offsets**: Fine-tune button position in pixels

3. Click the "Grid Settings" button on the chart to adjust:
   - **Zero Line**: Reference price (0 = auto-calculated from chart center)
   - **Grid Size**: Distance between lines in pips (default: 30)

## Configuration

### Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| Maximum lines on screen | int | 40 | Max lines before auto-scaling grid |
| Grid color | Color | Gray | Color of grid lines |
| Settings button position | Enum | TopRight | Button placement on chart |
| Button offset X | int | 5 | Horizontal offset in pixels |
| Button offset Y | int | 5 | Vertical offset in pixels |

### Runtime Settings (Dialog)

- **Zero Line**: The price level from which the grid starts (0 = auto)
- **Grid Size**: Spacing between lines in pips

Settings are saved automatically and persist across sessions.

## Architecture

The project follows SOLID principles and is organized into partial classes:

- `Grid.cs`: Main class, interfaces, Calculate method
- `Grid - BASE.cs`: Base class structure
- `Grid - Parameters.cs`: Parameter definitions
- `Grid - Init.cs`: Initialization logic
- `Grid - Indicator.cs`: Grid drawing logic
- `Grid - UI.cs`: User interface components
- `Grid - Events.cs`: Chart event handlers
- `Grid - Storage.cs`: Settings persistence

See [CLAUDE.md](Grid/CLAUDE.md) for detailed architecture documentation.

## Changelog

See [CHANGELOG.md](Grid/CHANGELOG.md) for version history.

## Development

### Requirements

- Visual Studio 2019 or later
- .NET 6.0 SDK
- cTrader platform

### Building

Open `Grid.sln` in Visual Studio and build the solution.

### Contributing

Contributions are welcome! Please read [CLAUDE.md](Grid/CLAUDE.md) for coding standards and architectural principles.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Author

**Maurizio Lobello**
- GitHub: [@mauriziolobello](https://github.com/mauriziolobello)

## Acknowledgments

- Built with cTrader Automate API
- Co-developed with Claude Sonnet 4.5

## Support

For issues and feature requests, please use the [GitHub Issues](https://github.com/mauriziolobello/ctrader-grid-indicator/issues) page.
