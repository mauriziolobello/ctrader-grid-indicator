using System;
using System.Collections.Generic;
using System.Data;
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
//using LibrarySmartMoney;

namespace cAlgo
{
    public partial class Grid
    {
        /// <summary>
        /// Removes all grid lines from the chart
        /// </summary>
        public void ClearGrid()
        {
            // Materialize the query first to avoid modifying collection during iteration
            foreach (ChartObject o in Chart.Objects.Where(w => w.Name.Contains("Grid")).ToList())
            {
                Chart.RemoveObject(o.Name);
            }
        }

        private int CalcGridSize()
        {
            int actualGridSize = GetActualGridSize();
            int lines = (int)(Chart.HeightInPips() / actualGridSize);

            double calcGridSize = (Chart.HeightInPips() / MaxLinesOnScreen).FindClosestRoundNumber(1, Chart.HeightInPips());

            return lines <= MaxLinesOnScreen ? actualGridSize : (int)calcGridSize;
        }

        public void DrawGrid()
        {
            try
            {
                // Verifica che il grafico sia pronto
                if (Chart.Height <= 0 || double.IsNaN(Chart.TopY) || double.IsNaN(Chart.BottomY))
                {
                    Log("DrawGrid: Chart not ready yet, skipping.");
                    return;
                }

                ClearGrid();

                int effectiveGridSize = CalcGridSize();

                if (effectiveGridSize > Chart.HeightInPips())
                {
                    Chart.DrawStaticText("GridInfoMessage", "Grid size is larger than the visible area", VerticalAlignment.Bottom, HorizontalAlignment.Center, GridColor);
                    return;
                }

                double watershedPrice = GetActualZeroLine();
                double gridStepPrice = effectiveGridSize * Symbol.PipSize;

                // Calcola i limiti visibili del grafico
                double bottomY = Chart.BottomY;
                double topY = Chart.TopY;

            // Se ZeroLine � 0 (calcolata), verifica che sia nel range visibile
            if (_actualZeroLine == 0)
            {
                if (!watershedPrice.IsInRange(bottomY, topY))
                {
                    Log($"Zero line ({watershedPrice}) is not within the visible range ({bottomY}/{topY})");
                    return;
                }
            }

            // Calcola la prima linea visibile SOTTO il topY partendo dalla watershedPrice
            // Trova quante linee di grid ci sono tra watershedPrice e topY
            double distanceToTop = topY - watershedPrice;
            int linesAboveWatershed = (int)Math.Ceiling(distanceToTop / gridStepPrice);
            double firstVisibleLineAbove = watershedPrice + (linesAboveWatershed * gridStepPrice);

            // Calcola la prima linea visibile SOPRA il bottomY partendo dalla watershedPrice
            double distanceToBottom = watershedPrice - bottomY;
            int linesBelowWatershed = (int)Math.Ceiling(distanceToBottom / gridStepPrice);
            double firstVisibleLineBelow = watershedPrice - (linesBelowWatershed * gridStepPrice);

            // Disegna le linee dall'alto verso il basso nell'area visibile
            int lineCount = 0;
            int maxLines = 100; // Limite di sicurezza

            // Parti dalla linea pi� alta visibile e scendi
            double currentLine = firstVisibleLineAbove;
            
            while (currentLine >= bottomY && lineCount < maxLines)
            {
                if (currentLine <= topY) // Disegna solo se � nell'area visibile
                {
                    string lineName = currentLine == watershedPrice ? "Grid0" : $"GridLine{lineCount}";
                    Chart.DrawHorizontalLine(lineName, currentLine, GridColor, 1, LineStyle.Lines);
                }
                
                currentLine -= gridStepPrice;
                lineCount++;
            }

            // Log di debug (rimuovere dopo il test)
            // Log($"DrawGrid: Drawn {lineCount} lines, gridStep={gridStepPrice}, range={bottomY:F2}-{topY:F2}");
            }
            catch (Exception ex)
            {
                Log($"DrawGrid error: {ex.Message}");
            }
        }
    }
}