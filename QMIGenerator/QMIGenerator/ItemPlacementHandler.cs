using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace QMIGenerator
{
    public static class ItemPlacementHandler
    {
        public static Boolean ItemIntersect(Rectangle Rect, UIElementCollection List, Rect Border)
        {
            Canvas.SetLeft(Rect, 150);
            return true;
        }
    }
}
