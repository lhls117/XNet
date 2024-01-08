using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;

namespace XNet.Controls
{
    public class ShapeAdorner : Adorner
    {
        private Rect strokeBounds = Rect.Empty;
        private bool changedStart = true;
        private Thumb all;
        private Thumb topLeft;
        private Thumb topMid;
        private Thumb topRight;
        private Thumb midLeft;
        private Thumb midRight;
        private Thumb bottomLeft;
        private Thumb bottomMid;
        private Thumb bottomRight;
        private Path outLine;
        private Point topLeftPnt;
        private Point topMidPnt;
        private Point topRightPnt;
        private Point midLeftPnt;
        private Point midRightPnt;
        private Point bottomLeftPnt;
        private Point bottomMidPnt;
        private Point bottomRightPnt;
        private VisualCollection visualChildren;
        private TranslateTransform trans;
        private ScaleTransform scale;
        private double zoomScale;
        private bool maskCheck;

        public ShapeAdorner(UIElement adornedElement, double scale)
            : base(adornedElement)
        {
            this.visualChildren = new VisualCollection((Visual)this);
            this.zoomScale = scale <= 1.0 ? 1.0 : scale;
            this.MakeCornerThumb(ref this.topLeft, Cursors.SizeNWSE);
            this.MakeCornerThumb(ref this.topMid, Cursors.SizeNS);
            this.MakeCornerThumb(ref this.topRight, Cursors.SizeNESW);
            this.MakeCornerThumb(ref this.midLeft, Cursors.SizeWE);
            this.MakeCornerThumb(ref this.midRight, Cursors.SizeWE);
            this.MakeCornerThumb(ref this.bottomLeft, Cursors.SizeNESW);
            this.MakeCornerThumb(ref this.bottomMid, Cursors.SizeNS);
            this.MakeCornerThumb(ref this.bottomRight, Cursors.SizeNWSE);
            this.topLeft.DragDelta += new DragDeltaEventHandler(this.StrokeResize_TopLeft_Start);
            this.topLeft.DragCompleted += new DragCompletedEventHandler(this.StrokeResize_TopLeft_End);
            this.topMid.DragDelta += new DragDeltaEventHandler(this.StrokeResize_TopMid_Start);
            this.topMid.DragCompleted += new DragCompletedEventHandler(this.StrokeResize_TopMid_End);
            this.topRight.DragDelta += new DragDeltaEventHandler(this.StrokeResize_TopRight_Start);
            this.topRight.DragCompleted += new DragCompletedEventHandler(this.StrokeResize_TopRight_End);
            this.midLeft.DragDelta += new DragDeltaEventHandler(this.StrokeResize_MidLeft_Start);
            this.midLeft.DragCompleted += new DragCompletedEventHandler(this.StrokeReisze_MidLeft_End);
            this.midRight.DragDelta += new DragDeltaEventHandler(this.StrokeResize_MidRight_Start);
            this.midRight.DragCompleted += new DragCompletedEventHandler(this.StrokeResize_MidRight_End);
            this.bottomLeft.DragDelta += new DragDeltaEventHandler(this.StrokeResize_BottomLeft_Start);
            this.bottomLeft.DragCompleted += new DragCompletedEventHandler(this.StrokeResize_BottomLeft_End);
            this.bottomMid.DragDelta += new DragDeltaEventHandler(this.StrokeResize_BottomMid_Start);
            this.bottomMid.DragCompleted += new DragCompletedEventHandler(this.StrokeResize_BottomMid_End);
            this.bottomRight.DragDelta += new DragDeltaEventHandler(this.StrokeResize_BottomRight_Start);
            this.bottomRight.DragCompleted += new DragCompletedEventHandler(this.StrokeResize_BottomRight_End);
            this.outLine = new Path();
            this.outLine.Stroke = (Brush)Brushes.DarkTurquoise;
            this.outLine.StrokeThickness = 1.0;
            this.outLine.StrokeDashArray = new DoubleCollection()
            {
                5.0,
                5.0
            };
            this.outLine.StrokeDashCap = PenLineCap.Round;
            this.visualChildren.Add((Visual)this.outLine);
            this.strokeBounds = this.AdornedStrokes.GetBounds();
            this.MakeMoveThumb();
            //if (this.AdornedStrokes.Count == 1 && this.AdornedStrokes[0].GetType() == typeof(ROI))
            //    return;
            this.InitContextmenu();
        }

        //private int StrokeInit()
        //{
        //    //if (ApplicationData.Instance.SelectedTabIndex != 0)
        //    //    return 3;
        //    var num1 = 0;
        //    var num2 = 0;
        //    foreach (var adornedStroke in (Collection<Stroke>)this.AdornedStrokes)
        //    {
        //        if (adornedStroke.GetType() == typeof(Mask))
        //        {
        //            if ((adornedStroke as LabelStroke).isAllApplied)
        //                ++num1;
        //            else
        //                ++num2;
        //        }
        //    }

        //    if (num1 == this.AdornedStrokes.Count)
        //        return 0;
        //    return num2 == this.AdornedStrokes.Count ? 1 : 2;
        //}

        private void InitContextmenu()
        {
            if (this.ContextMenu == null)
                this.ContextMenu = new ContextMenu();
            if (this.ContextMenu != null && this.ContextMenu.Items.Count > 0)
                this.ContextMenu.Items.Clear();
            //foreach (object adornedStroke in (Collection<Stroke>)this.AdornedStrokes)
            //{
            //    if (adornedStroke.GetType() == typeof(ROI))
            //    {
            //        this.ContextMenu = (ContextMenu)null;
            //        return;
            //    }
            //}

            var menuItem1 = new MenuItem();
            menuItem1.Header = (object)"Copy (Ctrl + C)";
            menuItem1.Foreground = (Brush)Brushes.White;
            menuItem1.Click -= new RoutedEventHandler(this.CopyItem_Click);
            menuItem1.Click += new RoutedEventHandler(this.CopyItem_Click);
            var menuItem2 = new MenuItem();
            menuItem2.Header = (object)"Paste (Ctrl + V)";
            menuItem2.Foreground = (Brush)Brushes.DarkGray;
            menuItem2.IsEnabled = false;
            var menuItem3 = new MenuItem();
            //if (ApplicationData.Instance.SelectedTabIndex == 0)
            //{
            //    menuItem3.Header = (object)"Apply all";
            //    menuItem3.Foreground = (Brush)Brushes.White;
            //    menuItem3.Click -= new RoutedEventHandler(this.Item3_Click);
            //    menuItem3.Click += new RoutedEventHandler(this.Item3_Click);
            //}
            //else
            //{
            //    menuItem3.Header = (object)"Class change";
            //    menuItem3.Foreground = (Brush)Brushes.White;
            //    if (ApplicationData.Instance.ClassList != null && ApplicationData.Instance.ClassList.Count > 0)
            //    {
            //        foreach (var classBaseModel in (Collection<ClassBaseModel>)ApplicationData.Instance
            //            .ClassList)
            //        {
            //            var menuItem4 = new MenuItem();
            //            menuItem4.Header = (object)classBaseModel.ClassName;
            //            menuItem4.Foreground =
            //                (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString(classBaseModel.Color));
            //            menuItem4.Click -= new RoutedEventHandler(this.Child_Click);
            //            menuItem4.Click += new RoutedEventHandler(this.Child_Click);
            //            menuItem3.Items.Add((object)menuItem4);
            //        }
            //    }
            //}

            this.ContextMenu.Items.Add((object)menuItem1);
            this.ContextMenu.Items.Add((object)menuItem2);
            this.ContextMenu.Items.Add((object)menuItem3);
            //switch (this.StrokeInit())
            //{
            //    case 0:
            //        var menuItem5 = new MenuItem();
            //        menuItem5.Header = (object)"Cancel apply all";
            //        menuItem5.Foreground = (Brush)Brushes.White;
            //        menuItem5.Click -= new RoutedEventHandler(this.Item4_Click);
            //        menuItem5.Click += new RoutedEventHandler(this.Item4_Click);
            //        this.ContextMenu.Items.Remove((object)menuItem3);
            //        this.ContextMenu.Items.Add((object)menuItem5);
            //        break;
            //    case 1:
            //        menuItem3.IsEnabled = true;
            //        break;
            //    case 2:
            //        menuItem3.IsEnabled = false;
            //        menuItem3.Foreground = (Brush)Brushes.DarkGray;
            //        break;
            //}
        }

        private void Item4_Click(object sender, RoutedEventArgs e)
        {
            //if ((uint)ApplicationData.Instance.SelectedTabIndex > 0U)
            //    return;
            //var maskModelList = new List<MaskModel>();
            //foreach (var adornedStroke in (Collection<Stroke>)this.AdornedStrokes)
            //{
            //    var mask = adornedStroke as Mask;
            //    mask.GetBounds();
            //    maskModelList.Add(new MaskModel()
            //    {
            //        PrimaryKey = mask.PrimaryKey
            //    });
            //}

            //Messenger.Default.Send<AdornerCommuteBefore>(new AdornerCommuteBefore()
            //{
            //    ID = "All_Resolve",
            //    ClassName = "Mask",
            //    Value = (object)maskModelList
            //});
        }

        //private void Child_Click(object sender, RoutedEventArgs e)
        //{
        //    if (e.Source.GetType() != typeof(MenuItem))
        //        return;
        //    var source = e.Source as MenuItem;
        //    Messenger.Default.Send<AdornerCommuteBefore>(new AdornerCommuteBefore()
        //    {
        //        ID = "Class_Change",
        //        ClassName = source.Header.ToString(),
        //        Value = (object)this.AdornedStrokes.Clone()
        //    });
        //}

        //private void Item3_Click(object sender, RoutedEventArgs e)
        //{
        //    if ((uint)ApplicationData.Instance.SelectedTabIndex > 0U)
        //        return;
        //    var maskModelList = new List<MaskModel>();
        //    foreach (var adornedStroke in (Collection<Stroke>)this.AdornedStrokes)
        //    {
        //        var mask = adornedStroke as Mask;
        //        var bounds = mask.GetBounds();
        //        maskModelList.Add(new MaskModel()
        //        {
        //            PrimaryKey = mask.PrimaryKey,
        //            Position = new Point(bounds.X, bounds.Y),
        //            Size = bounds.Size,
        //            IsAllApplied = true,
        //            Shape = new ShapeModel()
        //            {
        //                Shape = mask.shapeString,
        //                Points = mask.GetPoints(),
        //                MaxScore = 0.0,
        //                StrokeThickness = mask.pen == null ? 0.0 : mask.pen.Thickness
        //            }
        //        });
        //    }

        //    Messenger.Default.Send<AdornerCommuteBefore>(new AdornerCommuteBefore()
        //    {
        //        ID = "Mask_Apply_All",
        //        ClassName = "Mask",
        //        Value = (object)maskModelList
        //    });
        //}

        private void CopyItem_Click(object sender, RoutedEventArgs e)
        {
            //switch (ApplicationData.Instance.SelectedTabIndex)
            //{
            //    case 0:
            //        StrokeCopyAndPaste.GetInstance().SaveBaseData(this.AdornedStrokes.Clone());
            //        break;
            //    case 1:
            //        StrokeCopyAndPaste.GetInstance().SaveTrainData(this.AdornedStrokes.Clone());
            //        break;
            //    case 2:
            //        StrokeCopyAndPaste.GetInstance().SaveTestData(this.AdornedStrokes.Clone());
            //        break;
            //}
        }

        public void SetScale(double scaleValue)
        {
            //if (scaleValue > 1.0)
            //    this.zoomScale = scaleValue;
            //else
            //    this.zoomScale = 1.0;
        }

        private void PointRenew()
        {
            this.topLeftPnt = new Point(this.strokeBounds.X, this.strokeBounds.Y);
            this.topMidPnt = new Point(this.strokeBounds.X + this.strokeBounds.Width / 2.0, this.strokeBounds.Y);
            this.topRightPnt = new Point(this.strokeBounds.X + this.strokeBounds.Width, this.strokeBounds.Y);
            this.midLeftPnt = new Point(this.strokeBounds.X, this.strokeBounds.Y + this.strokeBounds.Height / 2.0);
            this.midRightPnt = new Point(this.strokeBounds.X + this.strokeBounds.Width,
                this.strokeBounds.Y + this.strokeBounds.Height / 2.0);
            this.bottomLeftPnt = new Point(this.strokeBounds.X, this.strokeBounds.Y + this.strokeBounds.Height);
            this.bottomMidPnt = new Point(this.strokeBounds.X + this.strokeBounds.Width / 2.0,
                this.strokeBounds.Y + this.strokeBounds.Height);
            this.bottomRightPnt = new Point(this.strokeBounds.X + this.strokeBounds.Width,
                this.strokeBounds.Y + this.strokeBounds.Height);
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (this.strokeBounds.IsEmpty)
                return finalSize;
            this.PointRenew();
            var finalRect1 = new Rect(this.topLeftPnt.X - 5.0 / this.zoomScale,
                this.topLeftPnt.Y - 5.0 / this.zoomScale, 10.0, 10.0);
            var finalRect2 = new Rect(this.topMidPnt.X - 5.0 / this.zoomScale, this.topMidPnt.Y - 5.0 / this.zoomScale,
                10.0, 10.0);
            var finalRect3 = new Rect(this.topRightPnt.X - 5.0 / this.zoomScale,
                this.topRightPnt.Y - 5.0 / this.zoomScale, 10.0, 10.0);
            var finalRect4 = new Rect(this.midLeftPnt.X - 5.0 / this.zoomScale,
                this.midLeftPnt.Y - 5.0 / this.zoomScale, 10.0, 10.0);
            var finalRect5 = new Rect(this.midRightPnt.X - 5.0 / this.zoomScale,
                this.midRightPnt.Y - 5.0 / this.zoomScale, 10.0, 10.0);
            var finalRect6 = new Rect(this.bottomLeftPnt.X - 5.0 / this.zoomScale,
                this.bottomLeftPnt.Y - 5.0 / this.zoomScale, 10.0, 10.0);
            var finalRect7 = new Rect(this.bottomMidPnt.X - 5.0 / this.zoomScale,
                this.bottomMidPnt.Y - 5.0 / this.zoomScale, 10.0, 10.0);
            var finalRect8 = new Rect(this.bottomRightPnt.X - 5.0 / this.zoomScale,
                this.bottomRightPnt.Y - 5.0 / this.zoomScale, 10.0, 10.0);
            this.all.Arrange(this.strokeBounds);
            this.topLeft.Arrange(finalRect1);
            this.topMid.Arrange(finalRect2);
            this.topRight.Arrange(finalRect3);
            this.midLeft.Arrange(finalRect4);
            this.midRight.Arrange(finalRect5);
            this.bottomLeft.Arrange(finalRect6);
            this.bottomMid.Arrange(finalRect7);
            this.bottomRight.Arrange(finalRect8);
            this.outLine.Data = (Geometry)new RectangleGeometry(this.strokeBounds);
            this.outLine.StrokeThickness = 1.0 / this.zoomScale;
            this.outLine.Arrange(new Rect(finalSize));
            this.topLeft.RenderTransform = (Transform)new ScaleTransform(1.0 / this.zoomScale, 1.0 / this.zoomScale);
            this.topMid.RenderTransform = (Transform)new ScaleTransform(1.0 / this.zoomScale, 1.0 / this.zoomScale);
            this.topRight.RenderTransform = (Transform)new ScaleTransform(1.0 / this.zoomScale, 1.0 / this.zoomScale);
            this.midLeft.RenderTransform = (Transform)new ScaleTransform(1.0 / this.zoomScale, 1.0 / this.zoomScale);
            this.midRight.RenderTransform = (Transform)new ScaleTransform(1.0 / this.zoomScale, 1.0 / this.zoomScale);
            this.bottomLeft.RenderTransform =
                (Transform)new ScaleTransform(1.0 / this.zoomScale, 1.0 / this.zoomScale);
            this.bottomRight.RenderTransform =
                (Transform)new ScaleTransform(1.0 / this.zoomScale, 1.0 / this.zoomScale);
            this.bottomMid.RenderTransform = (Transform)new ScaleTransform(1.0 / this.zoomScale, 1.0 / this.zoomScale);
            return finalSize;
        }

        private void MakeCornerThumb(ref Thumb cornerThumb, Cursor customizeCursors)
        {
            if (cornerThumb != null)
                return;
            cornerThumb = new Thumb();
            cornerThumb.Cursor = customizeCursors;
            ResourceDictionary rd = new ResourceDictionary() { Source = GetPackUri("Themes/Generic.xaml", typeof(ShapeAdorner).Assembly) };
            cornerThumb.Style = (Style)rd.FindName("AdornerThumbStyle");
            this.visualChildren.Add(cornerThumb);
        }

        public static Uri GetPackUri(string resourcePath, Assembly resourceAssembly)
        {
            return new Uri("pack://application:,,,/" + resourceAssembly.GetName().Name + ";Component/" + resourcePath);
        }

        public void KeyInputMove(double x, double y)
        {
            this.trans = new TranslateTransform(x, y);
            var transformMatrix = new Matrix();
            transformMatrix.TranslatePrepend(this.trans.X, this.trans.Y);
            this.AdornedStrokes.Transform(transformMatrix, false);
            this.strokeBounds = this.AdornedStrokes.GetBounds();
            this.outLine.RenderTransform = (Transform)this.trans;
            this.InvalidateArrange();
            this.outLine.RenderTransform = (Transform)null;
        }
        #region MoveThumb

        private void MakeMoveThumb()
        {
            var strokeBounds = this.strokeBounds;
            if (this.strokeBounds == Rect.Empty || this.visualChildren == null || this.all != null)
                return;
            var thumb = new Thumb();
            thumb.Cursor = Cursors.SizeAll;
            thumb.Height = this.strokeBounds.Height;
            thumb.Width = this.strokeBounds.Width;
            thumb.Opacity = 0.0;
            thumb.Background = (Brush)Brushes.Transparent;
            this.all = thumb;
            this.visualChildren.Add((Visual)this.all);
            this.all.DragDelta += new DragDeltaEventHandler(this.Stroke_Move_Start);
            this.all.DragCompleted += new DragCompletedEventHandler(this.Stroke_Move_Completed);
        }

        private void Stroke_Move_Start(object sender, DragDeltaEventArgs e)
        {
            if (this.changedStart)
            {
                this.SendMessageToDrawingTool();
                this.changedStart = false;
            }

            this.trans = new TranslateTransform(e.HorizontalChange, e.VerticalChange);
            this.outLine.RenderTransform = (Transform)this.trans;
        }

        private void Stroke_Move_Completed(object sender, DragCompletedEventArgs e)
        {
            this.changedStart = true;
            if (this.trans == null)
                return;
            var transformMatrix = new Matrix();
            transformMatrix.TranslatePrepend(this.trans.X, this.trans.Y);
            this.AdornedStrokes.Transform(transformMatrix, false);
            this.strokeBounds = this.AdornedStrokes.GetBounds();
            this.outLine.RenderTransform = (Transform)null;
            this.InvalidateArrange();
        }

        #endregion
        private void SendMessageToDrawingTool()
        {
            //Messenger.Default.Send<StrokeChangeCommute>(new StrokeChangeCommute()
            //{  
            //    ID = "DoTaskChange",
            //    BeforeTarget = (object)this.AdornedStrokes
            //});
        }
        #region StrokeResize

        private void StrokeResize_TopLeft_End(object sender, DragCompletedEventArgs e)
        {
            this.changedStart = true;
            if (this.scale == null)
                return;
            this.outLine.RenderTransform = (Transform)null;
            this.InvalidateArrange();
        }

        private void StrokeResize_TopLeft_Start(object sender, DragDeltaEventArgs e)
        {
            if (this.changedStart)
            {
                this.SendMessageToDrawingTool();
                this.changedStart = false;
            }

            var position = Mouse.GetPosition((IInputElement)this);
            this.scale = new ScaleTransform(
                (this.strokeBounds.Width + (this.topLeftPnt.X - position.X)) / this.strokeBounds.Width,
                (this.strokeBounds.Height + (this.topLeftPnt.Y - position.Y)) / this.strokeBounds.Height,
                this.topLeftPnt.X + this.strokeBounds.Width, this.topLeftPnt.Y + this.strokeBounds.Height);
            var transformMatrix = new Matrix();
            transformMatrix.ScaleAtPrepend(this.scale.ScaleX, this.scale.ScaleY,
                this.topLeftPnt.X + this.strokeBounds.Width, this.strokeBounds.Height + this.topLeftPnt.Y);
            this.AdornedStrokes.Transform(transformMatrix, false);
            this.strokeBounds = this.AdornedStrokes.GetBounds();
            if (this.strokeBounds.Height > 5.0)
                this.all.Height = this.strokeBounds.Height - 5.0;
            else
                this.all.Height = this.strokeBounds.Height;
            if (this.strokeBounds.Width > 5.0)
                this.all.Width = this.strokeBounds.Width - 5.0;
            else
                this.all.Width = this.strokeBounds.Width;
            this.outLine.RenderTransform = (Transform)this.scale;
            this.InvalidateArrange();
        }

        private void StrokeResize_TopMid_End(object sender, DragCompletedEventArgs e)
        {
            this.changedStart = true;
            if (this.scale == null)
                return;
            this.outLine.RenderTransform = (Transform)null;
            this.InvalidateArrange();
        }

        private void StrokeResize_TopMid_Start(object sender, DragDeltaEventArgs e)
        {
            if (this.changedStart)
            {
                this.SendMessageToDrawingTool();
                this.changedStart = false;
            }

            this.scale = new ScaleTransform((this.strokeBounds.Width + 0.0) / this.strokeBounds.Width,
                (this.strokeBounds.Height + (this.topMidPnt.Y - Mouse.GetPosition((IInputElement)this).Y)) /
                this.strokeBounds.Height, this.topMidPnt.X, this.topMidPnt.Y + this.strokeBounds.Height);
            var transformMatrix = new Matrix();
            transformMatrix.ScaleAtPrepend(this.scale.ScaleX, this.scale.ScaleY, this.topMidPnt.X,
                this.strokeBounds.Height + this.topMidPnt.Y);
            this.AdornedStrokes.Transform(transformMatrix, false);
            this.strokeBounds = this.AdornedStrokes.GetBounds();
            if (this.strokeBounds.Height > 5.0)
                this.all.Height = this.strokeBounds.Height - 5.0;
            else
                this.all.Height = this.strokeBounds.Height;
            if (this.strokeBounds.Width > 5.0)
                this.all.Width = this.strokeBounds.Width - 5.0;
            else
                this.all.Width = this.strokeBounds.Width;
            this.outLine.RenderTransform = (Transform)this.scale;
            this.InvalidateArrange();
        }

        private void StrokeResize_TopRight_End(object sender, DragCompletedEventArgs e)
        {
            this.changedStart = true;
            if (this.scale == null)
                return;
            this.outLine.RenderTransform = (Transform)null;
            this.InvalidateArrange();
        }

        private void StrokeResize_TopRight_Start(object sender, DragDeltaEventArgs e)
        {
            if (this.changedStart)
            {
                this.SendMessageToDrawingTool();
                this.changedStart = false;
            }

            var position = Mouse.GetPosition((IInputElement)this);
            this.scale = new ScaleTransform(
                (this.strokeBounds.Width + (position.X - this.topRightPnt.X)) / this.strokeBounds.Width,
                (this.strokeBounds.Height + (this.topRightPnt.Y - position.Y)) / this.strokeBounds.Height,
                this.topRightPnt.X - this.strokeBounds.Width, this.topRightPnt.Y + this.strokeBounds.Height);
            var transformMatrix = new Matrix();
            transformMatrix.ScaleAtPrepend(this.scale.ScaleX, this.scale.ScaleY,
                this.topRightPnt.X - this.strokeBounds.Width, this.strokeBounds.Height + this.topRightPnt.Y);
            this.AdornedStrokes.Transform(transformMatrix, false);
            this.strokeBounds = this.AdornedStrokes.GetBounds();
            if (this.strokeBounds.Height > 5.0)
                this.all.Height = this.strokeBounds.Height - 5.0;
            else
                this.all.Height = this.strokeBounds.Height;
            if (this.strokeBounds.Width > 5.0)
                this.all.Width = this.strokeBounds.Width - 5.0;
            else
                this.all.Width = this.strokeBounds.Width;
            this.outLine.RenderTransform = (Transform)this.scale;
            this.InvalidateArrange();
        }

        private void StrokeReisze_MidLeft_End(object sender, DragCompletedEventArgs e)
        {
            this.changedStart = true;
            if (this.scale == null)
                return;
            this.outLine.RenderTransform = (Transform)null;
            this.InvalidateArrange();
        }

        private void StrokeResize_MidLeft_Start(object sender, DragDeltaEventArgs e)
        {
            if (this.changedStart)
            {
                this.SendMessageToDrawingTool();
                this.changedStart = false;
            }

            this.scale = new ScaleTransform(
                (this.strokeBounds.Width + (this.midLeftPnt.X - Mouse.GetPosition((IInputElement)this).X)) /
                this.strokeBounds.Width, (this.strokeBounds.Height + 0.0) / this.strokeBounds.Height,
                this.midLeftPnt.X + this.strokeBounds.Width, this.midLeftPnt.Y);
            var transformMatrix = new Matrix();
            transformMatrix.ScaleAtPrepend(this.scale.ScaleX, this.scale.ScaleY,
                this.midLeftPnt.X + this.strokeBounds.Width, this.midLeftPnt.Y);
            this.AdornedStrokes.Transform(transformMatrix, false);
            this.strokeBounds = this.AdornedStrokes.GetBounds();
            if (this.strokeBounds.Height > 5.0)
                this.all.Height = this.strokeBounds.Height - 5.0;
            else
                this.all.Height = this.strokeBounds.Height;
            if (this.strokeBounds.Width > 5.0)
                this.all.Width = this.strokeBounds.Width - 5.0;
            else
                this.all.Width = this.strokeBounds.Width;
            this.outLine.RenderTransform = (Transform)this.scale;
            this.InvalidateArrange();
        }

        private void StrokeResize_MidRight_End(object sender, DragCompletedEventArgs e)
        {
            this.changedStart = true;
            if (this.scale == null)
                return;
            this.outLine.RenderTransform = (Transform)null;
            this.InvalidateArrange();
        }

        private void StrokeResize_MidRight_Start(object sender, DragDeltaEventArgs e)
        {
            if (this.changedStart)
            {
                this.SendMessageToDrawingTool();
                this.changedStart = false;
            }

            this.scale = new ScaleTransform(
                (this.strokeBounds.Width + (Mouse.GetPosition((IInputElement)this).X - this.midRightPnt.X)) /
                this.strokeBounds.Width, (this.strokeBounds.Height + 0.0) / this.strokeBounds.Height,
                this.midRightPnt.X - this.strokeBounds.Width, this.midRightPnt.Y);
            var transformMatrix = new Matrix();
            transformMatrix.ScaleAtPrepend(this.scale.ScaleX, this.scale.ScaleY,
                this.midRightPnt.X - this.strokeBounds.Width, this.midRightPnt.Y);
            this.AdornedStrokes.Transform(transformMatrix, false);
            this.strokeBounds = this.AdornedStrokes.GetBounds();
            if (this.strokeBounds.Height > 5.0)
                this.all.Height = this.strokeBounds.Height - 5.0;
            else
                this.all.Height = this.strokeBounds.Height;
            if (this.strokeBounds.Width > 5.0)
                this.all.Width = this.strokeBounds.Width - 5.0;
            else
                this.all.Width = this.strokeBounds.Width;
            this.outLine.RenderTransform = (Transform)this.scale;
            this.InvalidateArrange();
        }

        private void StrokeResize_BottomLeft_End(object sender, DragCompletedEventArgs e)
        {
            this.changedStart = true;
            if (this.scale == null)
                return;
            this.outLine.RenderTransform = (Transform)null;
            this.InvalidateArrange();
        }

        private void StrokeResize_BottomLeft_Start(object sender, DragDeltaEventArgs e)
        {
            if (this.changedStart)
            {
                this.SendMessageToDrawingTool();
                this.changedStart = false;
            }

            var position = Mouse.GetPosition((IInputElement)this);
            this.scale = new ScaleTransform(
                (this.strokeBounds.Width + (this.bottomLeftPnt.X - position.X)) / this.strokeBounds.Width,
                (this.strokeBounds.Height + (position.Y - this.bottomLeftPnt.Y)) / this.strokeBounds.Height,
                this.bottomLeftPnt.X + this.strokeBounds.Width, this.bottomLeftPnt.Y - this.strokeBounds.Height);
            var transformMatrix = new Matrix();
            transformMatrix.ScaleAtPrepend(this.scale.ScaleX, this.scale.ScaleY,
                this.bottomLeftPnt.X + this.strokeBounds.Width, this.bottomLeftPnt.Y - this.strokeBounds.Height);
            this.AdornedStrokes.Transform(transformMatrix, false);
            this.strokeBounds = this.AdornedStrokes.GetBounds();
            if (this.strokeBounds.Height > 5.0)
                this.all.Height = this.strokeBounds.Height - 5.0;
            else
                this.all.Height = this.strokeBounds.Height;
            if (this.strokeBounds.Width > 5.0)
                this.all.Width = this.strokeBounds.Width - 5.0;
            else
                this.all.Width = this.strokeBounds.Width;
            this.outLine.RenderTransform = (Transform)this.scale;
            this.InvalidateArrange();
        }

        private void StrokeResize_BottomMid_End(object sender, DragCompletedEventArgs e)
        {
            this.changedStart = true;
            if (this.scale == null)
                return;
            this.outLine.RenderTransform = (Transform)null;
            this.InvalidateArrange();
        }

        private void StrokeResize_BottomMid_Start(object sender, DragDeltaEventArgs e)
        {
            if (this.changedStart)
            {
                this.SendMessageToDrawingTool();
                this.changedStart = false;
            }

            this.scale = new ScaleTransform((this.strokeBounds.Width + 0.0) / this.strokeBounds.Width,
                (this.strokeBounds.Height + (Mouse.GetPosition((IInputElement)this).Y - this.bottomMidPnt.Y)) /
                this.strokeBounds.Height, this.bottomMidPnt.X, this.bottomMidPnt.Y - this.strokeBounds.Height);
            var transformMatrix = new Matrix();
            transformMatrix.ScaleAtPrepend(this.scale.ScaleX, this.scale.ScaleY, this.bottomMidPnt.X,
                this.bottomMidPnt.Y - this.strokeBounds.Height);
            this.AdornedStrokes.Transform(transformMatrix, false);
            this.strokeBounds = this.AdornedStrokes.GetBounds();
            if (this.strokeBounds.Height > 5.0)
                this.all.Height = this.strokeBounds.Height - 5.0;
            else
                this.all.Height = this.strokeBounds.Height;
            if (this.strokeBounds.Width > 5.0)
                this.all.Width = this.strokeBounds.Width - 5.0;
            else
                this.all.Width = this.strokeBounds.Width;
            this.outLine.RenderTransform = (Transform)this.scale;
            this.InvalidateArrange();
        }

        private void StrokeResize_BottomRight_End(object sender, DragCompletedEventArgs e)
        {
            this.changedStart = true;
            if (this.scale == null)
                return;
            this.outLine.RenderTransform = (Transform)null;
            this.InvalidateArrange();
        }

        private void StrokeResize_BottomRight_Start(object sender, DragDeltaEventArgs e)
        {
            var position = Mouse.GetPosition(this);
            this.scale = new ScaleTransform(
                (this.strokeBounds.Width + (position.X - this.bottomRightPnt.X)) / this.strokeBounds.Width,
                (this.strokeBounds.Height + (position.Y - this.bottomRightPnt.Y)) / this.strokeBounds.Height,
                this.bottomRightPnt.X - this.strokeBounds.Width, this.bottomRightPnt.Y - this.strokeBounds.Height);
            var transformMatrix = new Matrix();
            transformMatrix.ScaleAtPrepend(this.scale.ScaleX, this.scale.ScaleY,
                this.bottomRightPnt.X - this.strokeBounds.Width, this.bottomRightPnt.Y - this.strokeBounds.Height);
            this.AdornedStrokes.Transform(transformMatrix, false);
            this.strokeBounds = this.AdornedStrokes.GetBounds();
            if (this.strokeBounds.Height > 5.0)
                this.all.Height = this.strokeBounds.Height - 5.0;
            else
                this.all.Height = this.strokeBounds.Height;
            if (this.strokeBounds.Width > 5.0)
                this.all.Width = this.strokeBounds.Width - 5.0;
            else
                this.all.Width = this.strokeBounds.Width;
            this.outLine.RenderTransform = (Transform)this.scale;
            this.InvalidateArrange();
        }

        #endregion
        private StrokeCollection AdornedStrokes
        {
            get { return ((InkPresenter)this.AdornedElement).Strokes; }
        }

        protected override int VisualChildrenCount
        {
            get { return this.visualChildren.Count; }
        }

        protected override Visual GetVisualChild(int index)
        {
            return this.visualChildren[index];
        }
    }
}
