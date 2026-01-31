using System;
using System.Text.Json;
using cAlgo.API;
using LibraryExtensions;

namespace cAlgo
{
    public partial class Grid
    {
        /// <summary>
        /// Settings stored per asset/timeframe
        /// </summary>
        private class GridSettings
        {
            public double ZeroLine { get; set; }
            public int GridSize { get; set; }
        }

        // Actual values used for grid (loaded from storage or defaults)
        private double _actualZeroLine;
        private int _actualGridSize;

        // Track previous asset and ZeroLine for cross-timeframe propagation
        private static string _previousAsset;
        private static double _previousZeroLine;

        /// <summary>
        /// Generates a unique storage key for Broker+Asset+TimeFrame
        /// </summary>
        private string GetStorageKey()
        {
            string broker = Account.BrokerName ?? "Unknown";
            string asset = Symbol.Name ?? "Unknown";
            string timeFrame = TimeFrame.Name ?? "Unknown";

            return $"Grid {broker} {asset} {timeFrame}";
        }

        /// <summary>
        /// Loads settings from LocalStorage or uses parameter defaults
        /// </summary>
        private void LoadStoredSettings()
        {
            try
            {
                string key = GetStorageKey();
                Log($"Loading from key: '{key}'");

                string json = LocalStorage.GetString(key, LocalStorageScope.Type);

                if (!string.IsNullOrEmpty(json))
                {
                    Log($"Found stored JSON: {json}");
                    var settings = JsonSerializer.Deserialize<GridSettings>(json);
                    if (settings != null)
                    {
                        _actualZeroLine = settings.ZeroLine;
                        _actualGridSize = settings.GridSize;
                        Log($"Settings LOADED from storage for {Symbol.Name}/{TimeFrame.Name}: ZeroLine={_actualZeroLine}, GridSize={_actualGridSize}");
                    }
                }
                else
                {
                    Log($"No stored settings found for key '{key}'");
                    // Fallback to default constants
                    _actualZeroLine = DEFAULT_ZERO_LINE;
                    _actualGridSize = DEFAULT_GRID_SIZE;
                    Log($"Using default values for {Symbol.Name}/{TimeFrame.Name}: ZeroLine={_actualZeroLine}, GridSize={_actualGridSize}");
                }

                // Cross-timeframe ZeroLine propagation
                // If same asset but different timeframe, and ZeroLine is 0 or different from previous, use previous ZeroLine
                if (!string.IsNullOrEmpty(_previousAsset) &&
                    _previousAsset == Symbol.Name &&
                    _previousZeroLine != 0)
                {
                    // If current ZeroLine is 0 (default/auto) or different from previous, propagate previous ZeroLine
                    if (_actualZeroLine == 0 || _actualZeroLine != _previousZeroLine)
                    {
                        double oldZeroLine = _actualZeroLine;
                        _actualZeroLine = _previousZeroLine;

                        Log($"PROPAGATING ZeroLine from previous timeframe: {oldZeroLine} → {_actualZeroLine}");

                        // Save the propagated value so it persists for this timeframe
                        SaveStoredSettings(_actualZeroLine, _actualGridSize);
                    }
                }

                // Update tracking variables for next timeframe change
                _previousAsset = Symbol.Name;
                _previousZeroLine = _actualZeroLine;
            }
            catch (Exception ex)
            {
                Log($"Error loading settings: {ex.Message}");

                // Fallback to default constants
                _actualZeroLine = DEFAULT_ZERO_LINE;
                _actualGridSize = DEFAULT_GRID_SIZE;
                Log($"Using default values for {Symbol.Name}/{TimeFrame.Name}: ZeroLine={_actualZeroLine}, GridSize={_actualGridSize}");
            }
        }

        /// <summary>
        /// Saves current settings to LocalStorage
        /// </summary>
        private void SaveStoredSettings(double zeroLine, int gridSize, string broker = null, string asset = null, string timeFrame = null)
        {
            try
            {
                // Use provided values or fall back to current Symbol values
                broker = broker ?? Account.BrokerName;
                asset = asset ?? Symbol.Name;
                timeFrame = timeFrame ?? TimeFrame.Name;

                var settings = new GridSettings
                {
                    ZeroLine = zeroLine,
                    GridSize = gridSize
                };

                string key = $"Grid {broker} {asset} {timeFrame}";
                string json = JsonSerializer.Serialize(settings);

                Log($"Saving to key: '{key}' - ZeroLine={zeroLine}, GridSize={gridSize}");

                LocalStorage.SetString(key, json, LocalStorageScope.Type);
                LocalStorage.Flush(LocalStorageScope.Type);

                _actualZeroLine = zeroLine;
                _actualGridSize = gridSize;

                // Update tracking variables so propagation works after manual save
                _previousAsset = asset;
                _previousZeroLine = zeroLine;

                Log($"Settings SAVED successfully for {asset}/{timeFrame}");
            }
            catch (Exception ex)
            {
                Log($"Error saving settings: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the actual zero line value (from storage or calculated)
        /// </summary>
        private double GetActualZeroLine()
        {
            return _actualZeroLine == 0 ? Chart.MiddlePrice() : _actualZeroLine;
        }

        /// <summary>
        /// Gets the actual grid size (from storage)
        /// </summary>
        private int GetActualGridSize()
        {
            return _actualGridSize;
        }
    }
}
