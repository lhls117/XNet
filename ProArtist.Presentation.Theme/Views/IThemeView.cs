using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using XNet.Controls;
using XNet.Presentation;

namespace ProArtist.Presentation.Theme.Views
{
    public interface IThemeView:IView
    {
        // DrawingControl DrawingControl { get; set; }

        Canvas Canvas { get;  }

        event Action<System.Windows. Point> PointChanged;

        event Action<double> FontSizeChanged;


    }
}
