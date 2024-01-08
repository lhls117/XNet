using System;
using System.Collections.Generic;
using System.Composition;
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


        private TranslateTransform translateTransform = new TranslateTransform();
        private TransformGroup transformGroup = new TransformGroup();
        private ScaleTransform scaleTransform = new ScaleTransform() {  };
        public ThemeView()
        {
            InitializeComponent();


            transformGroup.Children.Add(scaleTransform);
            transformGroup.Children.Add(translateTransform);
            text.RenderTransform = transformGroup;


        }
       
        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if(isLeftDown)
            {
               int count= VisualTreeHelper.GetChildrenCount(this.canvas);
                for(int i=0; i<count; i++)
                {
                   var el= VisualTreeHelper.GetChild(this.canvas, i);
                    if (el is TextBlock)
                    {
                        var p = Mouse.GetPosition(canvas);
                        translateTransform.X += p.X - startPoint.X;
                        translateTransform.Y += p.Y - startPoint.Y;
                        //Canvas.SetLeft((TextBlock)el, p.X);
                        //Canvas.SetTop((TextBlock)el, p.Y);
                    }
                    //if (el is Image)
                    //{
                    //    var image = el as Image;
                    //    //var l= Canvas.GetLeft(image);
                    //    //var t= Canvas.GetTop(image);

                    //    //var h = image.ActualHeight;
                    //    //var w = image.ActualWidth;

                    //    //Point point=new Point(l+w/2, t+h/2);
                    //    var p = Mouse.GetPosition(canvas);
                         
                    //   // Canvas.SetLeft((Image)el, p.X);
                    //    //Canvas.SetTop((Image)el, p.Y);

                    //    //((Image)el).RenderTransform = transformGroup;
                    //    translateTransform.X += p.X - startPoint.X;
                    //    translateTransform.Y += p.Y - startPoint.Y;
                    //    //scaleTransform.ScaleX += p.X - startPoint.X;
                    //    //scaleTransform.ScaleY += p.Y-startPoint.Y;

                        
                       
                    //}

                }

            }
        }

        private bool isLeftDown;

        Point startPoint= new Point();
        Point _mouseDownControlPosition;
        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint=Mouse.GetPosition(canvas);

           
            isLeftDown = true;
        }

        private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isLeftDown = false;
        }
      
        private void canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double scale = e.Delta * 0.002;
            int count = VisualTreeHelper.GetChildrenCount(this.canvas);
            for (int i = 0; i < count; i++)
            {
                var el = VisualTreeHelper.GetChild(this.canvas, i);
                if (el is TextBlock)
                {
                    scaleTransform.ScaleX += scale;
                    scaleTransform.ScaleY += scale;
                    //((TextBlock)el).RenderTransform = scaleTransform;
                    //scaleTransform.ScaleX += scale;
                    //scaleTransform.ScaleY += scale;
                }
                //if (el is Image)
                //{
                //    //((Image)el).RenderTransform = transformGroup;
                //    scaleTransform.ScaleX += scale;
                //    scaleTransform.ScaleY += scale;
                //}
            }
        }


        //public DrawingControl DrawingControl
        //{
        //    get => drawingControl;
        //    set => drawingControl = value;
        //}

        //private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    data
        //}
    }
}
