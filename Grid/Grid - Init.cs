using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;
using Library;
using LibraryExtensions;

namespace cAlgo
{
    public partial class Grid
    {
        private void PrintInfo()
        {
            try
            {
                Print($"{_label}, version {_version}");
                Print($"Symbol: {Symbol.Name} - Pip size: {Symbol.PipSize} - Actual Pip value={Symbol.PipValue.ToString("F8")}");

                /*
                 * Carica più dati, ma solo se voglio visualizzare le statistiche
                 */

                //if (ShowTextualStats) this.Bars.LoadMoreHistory();

#if DEBUG
                for (int i = 0; i < Symbol.MarketHours.Sessions.Count; i++)
                {
                    TradingSession ts = Symbol.MarketHours.Sessions[i];

                    Print($"Session {i}: Start Day={ts.StartDay}, End Day={ts.EndDay}, Start Hour={ts.StartTime}, End Hour={ts.EndTime}");
                }

                Print($"Trading Sessions: {Symbol.MarketHours.Sessions.Count}");
#endif
            }
            catch (Exception ex)
            {
                Log($"{_label}, PrintInfo - {ex.Message}");
            }
        }

        private void InitializeLibrary()
        {
            try
            {
            }
            catch (Exception ex)
            {
                Log($"{ex.Message}");
            }
        }

        private void InitializeData()
        {
            try
            {

                double screenHeightInPixels = Chart.Height;
                double screenHeightInPips = Chart.HeightInPips();
                double middlePrice = Chart.MiddlePrice().Round(Symbol.Digits);

                Log($"Height of the chart area in pixels: {screenHeightInPixels}");
                Log($"Height of the chart area in pips: {screenHeightInPips}");
                Log($"Middle price: {middlePrice}");



                Chart.SizeChanged += Chart_SizeChanged;
                Chart.ZoomChanged += Chart_ZoomChanged;
                Chart.VisibilityChanged += Chart_VisibilityChanged;
                Chart.ScrollChanged += Chart_ScrollChanged;
                Chart.DragEnd += Chart_DragEnd;
            }
            catch (Exception ex)
            {
                Log($"Grid, InitializeData - {ex.Message}");
            }
        }
    }
}