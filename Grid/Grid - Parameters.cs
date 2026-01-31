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

namespace cAlgo
{
    public enum SettingsButtonPosition
    {
        TopLeft,
        TopRight,
        TopCenter,
        BottomLeft,
        BottomRight,
        BottomCenter,
        CenterLeft,
        CenterRight
    }

    public partial class Grid
    {
        // Default values for grid settings (used when no saved settings exist)
        private const double DEFAULT_ZERO_LINE = 0;  // 0 = auto-calculated
        private const int DEFAULT_GRID_SIZE = 30;     // pips

        [Parameter("Maximum lines on screen (then increase grid size)", Group = "Grid", DefaultValue = 40, MinValue = 10)]
        public int MaxLinesOnScreen { get; set; }

        [Parameter("Grid color", Group = "Grid", DefaultValue = "Gray")]
        public Color GridColor { get; set; }

        [Parameter("Settings button position", Group = "UI", DefaultValue = SettingsButtonPosition.TopRight)]
        public SettingsButtonPosition ButtonPosition { get; set; }

        [Parameter("Button offset X (pixels)", Group = "UI", DefaultValue = 5, MinValue = 0, MaxValue = 500)]
        public int ButtonOffsetX { get; set; }

        [Parameter("Button offset Y (pixels)", Group = "UI", DefaultValue = 5, MinValue = 0, MaxValue = 500)]
        public int ButtonOffsetY { get; set; }
    }
}