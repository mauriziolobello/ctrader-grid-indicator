using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;
using Library;
using LibraryLog;

namespace cAlgo
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public partial class Grid : Indicator, IInstrument, IMarketInfo
    {
        private const string _version = "1.5.1";
        private const string _label = "Grid v1.5.1";

        #region IMarketInfo
        public double PipSize { get => Symbol.PipSize; }

        public int BarsCount { get => Bars.Count; }

        public string StrategyName { get => _label; }
        #endregion

        private ILogger _logger;

        protected override void Initialize()
        {
            _logger = new CTraderLogger((s, m) => Print(m));

            Log($">>> Initialize called");

            PrintInfo();

            InitializeLibrary();

            // Load stored settings or use defaults
            LoadStoredSettings();

            // Create UI button
            CreateSettingsButton();

            InitializeData();

            DrawGrid();
        }

        public override void Calculate(int index)
        {
        }

        public void Log(string message)
        {
            _logger?.LogInformation("", message);
        }

        public void Log(string message, params object[] parameters)
        {
            if (parameters != null && parameters.Length > 0)
            {
                _logger?.LogInformation("", string.Format(message, parameters));
            }
            else
            {
                _logger?.LogInformation("", message);
            }
        }

        /// <summary>
        /// Called when the indicator is stopped
        /// </summary>
        protected override void OnDestroy()
        {
            if (_isDialogOpen)
            {
                CloseSettingsDialog();
            }
        }
    }
}