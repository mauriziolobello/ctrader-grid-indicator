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
//using LibrarySmartMoney;

namespace cAlgo
{
    public partial class Grid
    {
        private void Chart_SizeChanged(ChartSizeEventArgs obj)
        {
            try { DrawGrid(); } catch { }
        }

        private void Chart_ZoomChanged(ChartZoomEventArgs obj)
        {
            try { DrawGrid(); } catch { }
        }

        private void Chart_DragEnd(ChartDragEventArgs obj)
        {
            //DrawGrid();
        }

        private void Chart_ScrollChanged(ChartScrollEventArgs obj)
        {
            try { DrawGrid(); } catch { }
        }

        private void Chart_VisibilityChanged(ChartVisibilityChangedEventArgs obj)
        {
            try { DrawGrid(); } catch { }
        }

    }
}