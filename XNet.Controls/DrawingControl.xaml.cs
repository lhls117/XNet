using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace XNet.Controls
{
    /// <summary>
    /// DrawingControl.xaml 的交互逻辑
    /// </summary>
    public partial class DrawingControl : UserControl
    {

        #region DependecyProperty
        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(DrawingControl));

        public ImageSource ImageSource
        {
            get => (ImageSource)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }
        public DrawingCanvas DrawingCanvas => drawingCanvas;
        #endregion
        public DrawingControl()
        {
            InitializeComponent();
        }
    }
}
