using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNet.Controls
{
    public enum DrawingType
    {
        [Description("Line")]
        Line,
        [Description("PolyLine")]
        PolyLine,
        [Description("Rectangle")]
        Rectangle,
        [Description("Ellipse")]
        Ellipse,
        [Description("Polygon")]
        Polygon,
        [Description("Classification")]
        Classification,
    }
}
