using ProArtist.Domain;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XNet.Controls;
using static System.Formats.Asn1.AsnWriter;

namespace ProArtist.Presentation.Theme.Views
{
    /// <summary>
    /// ThemeView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IThemeView))]
    public partial class ThemeView : IThemeView
    {
        public Canvas Canvas => this.canvas;


        private IController selectedController;
        private FrameworkElement sControl;
    

        public event Action<System.Windows.Point> PointChanged;
        public event Action<double> FontSizeChanged;

        public ThemeView()
        {
            InitializeComponent();

        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isLeftDown)
            {
                if (sControl != null)
                {
                    if (sControl.Name == selectedController.Name)
                    {
                        var p = Mouse.GetPosition(canvas);
                        var dp = p - startPoint;
                        startPoint.X = p.X;
                        startPoint.Y = p.Y;
                        var group = sControl.RenderTransform as TransformGroup;
                        var translate = (TranslateTransform)group.Children[0];
                        //translate.X = _mouseDownControlPosition.X + dp.X;
                        //translate.Y = _mouseDownControlPosition.Y + dp.Y;
                        translate.X  += dp.X;
                        translate.Y += dp.Y;
                        PointChanged?.Invoke(new Point(translate.X, translate.Y));
                    }
                }


            }
        }

        private void canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double scale = e.Delta * 0.01;
            if (sControl != null)
            {
                if(sControl is TextBlock)
                {
                    var textBlock = (TextBlock)sControl;
                    textBlock.FontSize += scale;
                    FontSizeChanged?.Invoke(textBlock.FontSize);
                }


                //使用缩放会导致平移时xy有问题
                //var group = sControl.RenderTransform as TransformGroup;
                //var scal = (ScaleTransform)group.Children[1];
                //scal.ScaleX =scal.ScaleX+ scale;
                //scal.ScaleY =scal.ScaleY+ scale;
            }
        }

        private bool isLeftDown;

        Point startPoint = new Point();
        Point _mouseDownControlPosition;

       

        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = Mouse.GetPosition(canvas);

            var group = sControl.RenderTransform as TransformGroup;
            _mouseDownControlPosition = new Point(((TranslateTransform)group.Children[0]).X, ((TranslateTransform)group.Children[0]).Y);

            isLeftDown = true;
        }

        private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isLeftDown = false;
        }

      

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ListBox)sender).SelectedItem != null)
            {
                selectedController = (IController)((ListBox)sender).SelectedItem;

                int count = VisualTreeHelper.GetChildrenCount(this.canvas);
                for (int i = 0; i < count; i++)
                {
                    var child = VisualTreeHelper.GetChild(this.canvas, i);
                    var c = child as FrameworkElement;
                    if (c != null && c.Name == selectedController.Name)
                    {
                        sControl = (FrameworkElement)c;
                        var group = sControl.RenderTransform as TransformGroup;
                        if (group == null)
                        {
                            group = new TransformGroup();
                            group.Children.Add(new TranslateTransform());
                            group.Children.Add(new ScaleTransform());
                            sControl.RenderTransform = group;
                        }
                        
                    }
                }
            }
        }



    }
}
