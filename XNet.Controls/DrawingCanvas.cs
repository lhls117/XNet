using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System.ComponentModel;

namespace XNet.Controls
{
    public enum DrawingCanvasEditMode
    {
        None,
        Drawing,
        Select,
        Move

    }

    public enum SelectControll
    {
        [Description("图片")]
        Image,
        [Description("文字")]
        Text,
        [Description("数据")]
        Data,
        [Description("状态条")]
        StatusBar,
        [Description("弧形条")]
        ArcBar,
    }

    public class DrawingCanvas : InkCanvas
    {
        #region 有bug，暂时不用

        //public static readonly DependencyProperty SelectedStrokeProperty = DependencyProperty.Register("SelectedStroke", typeof(Stroke), typeof(DrawingCanvas), new PropertyMetadata(StrokeChanged));
        //public static void StrokeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    var canvas = d as DrawingCanvas;
        //    if (canvas.SelectedStroke == null)
        //        return;
        //    else
        //    {
        //        bool flag = false;
        //        foreach(var item in canvas.Strokes)
        //        {
        //            if(item== canvas.SelectedStroke)
        //            {
        //                flag = true;
        //                break;
        //            }
        //        }
        //        if(flag)
        //        {
        //            canvas.StrokeResize(canvas.SelectedStroke);
        //        }
        //        else
        //        {
        //            canvas.SelectedStroke = null;
        //        }

        //    }
        //}
        //public Stroke SelectedStroke
        //{
        //    get => (Stroke)GetValue(SelectedStrokeProperty);
        //    set => SetValue(SelectedStrokeProperty, value);
        //}

        #endregion


        #region DependencyProperty
        public static readonly DependencyProperty IsDrawingProperty = DependencyProperty.Register("IsDrawing", typeof(bool), typeof(DrawingCanvas), new PropertyMetadata(false));
        /// <summary>
        /// 是否在绘制中
        /// </summary>
        public bool IsDrawing
        {
            get => (bool)GetValue(IsDrawingProperty);
            private set => SetValue(IsDrawingProperty, value);
        }

        #endregion
        #region Fields
        Stroke _selectedStroke;
        InkPresenter _inkPresenter;
        LabelStroke _lable = null;
        private AdornerLayer _adorner;
        private Adorner _ador;

        public event Action<int, int> TextBlockEvent;
       

        private SelectControll selectControll;
        public  SelectControll SelectControll
        {
            get => selectControll;
            set => selectControll = value;
        }

        private TextBlock _selectTextBlock;

        public TextBlock SelectTextBlock
        {
            get => _selectTextBlock;
            set => _selectTextBlock = value;
        }

        DrawingType _drawingType = DrawingType.Classification;
        DrawingCanvasEditMode _editMode = DrawingCanvasEditMode.None;
        //public StrokeCollection Marks
        //{
        //    get => _marks;
        //    set
        //    {
        //        _marks = value;
        //        // RaisePropertyChanged();
        //    }
        //}

        #endregion
        #region Properties
        /// <summary>
        /// 获取或设置当前画笔
        /// </summary>
        public Pen Pen
        {
            get;
            set;
        }
        /// <summary>
        /// 选中的那个Stroke
        /// </summary>
        public Stroke SelectedStroke        {
            get => _selectedStroke;
            
        }
        public DrawingCanvasEditMode EditMode
        {
            get => _editMode;
            set
            {
                _editMode = value;
                ClearSelectStroke();
            }
        }
        public DrawingType DrawingType
        {
            get => _drawingType;
            set
            {
                _drawingType = value;
            }

        }

        #endregion
        public DrawingCanvas()
        {
            Loaded += DrawingCanvas_Loaded;
            EditingMode = InkCanvasEditingMode.None;
            UseCustomCursor = true;
            this.Background = new SolidColorBrush(Colors.Transparent);
            Strokes = new StrokeCollection();
            // _drawingType = DrawingType.Polygon;
        }

        private TranslateTransform translateTransform = new TranslateTransform();
        private ScaleTransform scaleTransform = new ScaleTransform() { ScaleX= 1, ScaleY = 1 };
        private TransformGroup transformGroup = new TransformGroup();

        private void DrawingCanvas_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _inkPresenter = new InkPresenter();
            AdornerLayer layer = AdornerLayer.GetAdornerLayer(this);
            layer.Add(new InkPresenterAdorner(this, _inkPresenter));
        }

        private bool isMouseLeftButtonDown;
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            //if (_editMode == DrawingCanvasEditMode.Drawing)
            //{
            //    BeginDrawing();
            //}
            //else if (_editMode == DrawingCanvasEditMode.Select)
            //{
            //    SelectStroke(e.GetPosition(this));
            //}
            //else if (_editMode == DrawingCanvasEditMode.Move)
            //{

            //}
            isMouseLeftButtonDown = true;
           
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
           isMouseLeftButtonDown=false;
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            double scale = e.Delta * 0.002;

            SelectTextBlock.RenderTransform = scaleTransform;
            scaleTransform.ScaleX += scale;
            scaleTransform.ScaleY += scale;
        }
        void BeginDrawing()
        {
            if (_lable == null)
            {
                IsDrawing = true;
                var start = Mouse.GetPosition(this);
                var pts = new StylusPointCollection();
                pts.Add(new StylusPoint(start.X, start.Y));
                pts.Add(new StylusPoint(start.X, start.Y));

                _lable = new LabelStroke(pts, _drawingType, Pen);
                //     guide.SetPenBrush(2); 
                Strokes.Add(_lable);

            }
            else
            {
                if (_lable.DrawingType == DrawingType.Polygon)
                {
                    var start = Mouse.GetPosition(this);
                    _lable.AddPolyLinePoint(new StylusPoint(start.X, start.Y));
                }
                else
                {
                    DrawingComplete();
                }
            }
        }
        void SelectStroke(Point point)
        {

            foreach (var item in Children)
            {
                if(item is TextBlock)
                {
                    var a = (TextBlock)item;
                    SelectTextBlock = a;
                    //SetLeft(a, 200);
                    //SetTop(a, 200);
                }
            }
            //foreach (var item in Strokes)
            //{
            //    if (item.GetBounds().Contains(point))
            //    {
            //        var str = item as LabelStroke;
            //        //     str.IsVisible = !str.IsVisible;
            //        StrokeResize(item);
            //        return;
            //    }
            //}
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            //if (_lable != null)
            //{
            //    var position = Mouse.GetPosition(this);



            //    //     _lable.ChangeEndPoint(new StylusPoint(position.X, position.Y));
            //    if (_lable.DrawingType == DrawingType.Polygon)
            //    {
            //        if (Keyboard.IsKeyDown(Key.LeftShift))
            //        {
            //            _lable.AddPolyLinePoint(new StylusPoint(position.X, position.Y));
            //            return;
            //        }
            //        else if (Keyboard.IsKeyDown(Key.LeftCtrl))
            //        {
            //            var points = _lable.Points;
            //            var point = points[points.Count() - 2];
            //            StraightLineTransform(point, ref position);
            //        }
            //        _lable.ChangeEndPoint(new StylusPoint(position.X, position.Y));
            //        return;
            //    }
            //    else if ((_lable.DrawingType == DrawingType.Ellipse || _lable.DrawingType == DrawingType.Rectangle) && Keyboard.IsKeyDown(Key.LeftCtrl))
            //    {
            //        var point = _lable.FirstPoint;
            //        SquareTransform(point, ref position);
            //    }
            //    _lable.ChangeEndPoint(new StylusPoint(position.X, position.Y));
            //}
            //if(Keyboard.IsKeyDown(Key.lef))
           if(isMouseLeftButtonDown)
            {
                var p = Mouse.GetPosition(this);
                SetLeft(SelectTextBlock, p.X);
                SetTop(SelectTextBlock, p.Y);
                TextBlockEvent?.Invoke(Convert.ToInt16(p.X), Convert.ToInt16(p.Y));
            }
            

             
        }


        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            if (e.Key == Key.Delete)
            {
                if (InkPresenter.Strokes.Count > 0)
                {
                    var temp = _inkPresenter.Strokes.First();
                    Remove(temp);
                    //this.Strokes.Remove(temp);
                    //ClearSelectStroke();
                    //_inkPresenter.Strokes.Clear();
                }
            }
        }
        /// <summary>
        /// 删除一个图形
        /// </summary>
        /// <param name="stroke"></param>
        public void Remove(Stroke stroke)
        {
            this.Strokes.Remove(stroke);
            ClearSelectStroke();
            if (_inkPresenter != null)
                _inkPresenter.Strokes.Clear();
        }
        /// <summary>
        /// 清除所有图形
        /// </summary>
        public void ClearAll()
        {
            this.Strokes.Clear();
            ClearSelectStroke();
            if (_inkPresenter != null)
                _inkPresenter.Strokes.Clear();
        }
        #region Transform
        /// <summary>直线变换
        /// 
        /// </summary>
        /// <param name="point">标定点位</param>
        /// <param name="position">要变换的点位</param>
        void StraightLineTransform(Point point, ref Point position)
        {
            if ((Math.Abs(point.X - position.X) - Math.Abs(point.Y - position.Y)) > 0)
            {
                position.Y = point.Y;
            }
            else
            {
                position.X = point.X;
            }
        }
        /// <summary>正
        /// 
        /// </summary>
        /// <param name="point">标定点位</param>
        /// <param name="position">变换点位</param>
        void SquareTransform(Point point, ref Point position)
        {
            if (point.X > position.X && point.Y > position.Y)
            {
                ///左下角
                if ((point.X - position.X) > (point.Y - position.Y))
                {
                    position.Y = point.Y - (point.X - position.X);
                }
                else
                {
                    position.X = point.X - (point.Y - position.Y);
                }

            }
            else if (point.X < position.X && point.Y > position.Y)
            {
                ///右上角
                if ((Math.Abs(point.X - position.X) - Math.Abs(point.Y - position.Y)) > 0)
                {
                    position.Y = point.Y - (position.X - point.X);
                }
                else
                {
                    position.X = point.X + (point.Y - position.Y);
                }
            }
            else if (point.X > position.X && point.Y < position.Y)
            {
                ///左上角
                if ((Math.Abs(point.X - position.X) - Math.Abs(point.Y - position.Y)) > 0)
                {
                    position.Y = point.Y + (point.X - position.X);
                }
                else
                {
                    position.X = point.X - (position.Y - point.Y);
                }
            }
            else if (point.X < position.X && point.Y < position.Y)
            {
                ///右下角
                if ((Math.Abs(point.X - position.X) - Math.Abs(point.Y - position.Y)) > 0)
                {
                    position.Y = point.Y + (position.X - point.X);
                }
                else
                {
                    position.X = point.X + (position.Y - point.Y);
                }
            }
        }
        #endregion
        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
            if (_lable != null && _lable.DrawingType == DrawingType.Polygon)
            {
                _lable.DrawComplte();
            }
            DrawingComplete();
        }


        void StrokeResize(Stroke stroke)
        {
            _selectedStroke = stroke;
            _inkPresenter.Strokes.Clear();
            _inkPresenter.Strokes.Add(stroke);
            ClearSelectStroke();
            _adorner = AdornerLayer.GetAdornerLayer(_inkPresenter);
            _ador = new ShapeAdorner(_inkPresenter, 2);
            _ador.IsClipEnabled = true;
            _ador.ClipToBounds = true;
            _adorner.Add(_ador);
            _adorner.ClipToBounds = true;
            //stroke.StylusPointsChanged -=
            //    new EventHandler(TrainDrawingViewModel_StylusPointsChanged);
            //stroke.StylusPointsChanged +=
            //    new EventHandler(TrainDrawingViewModel_StylusPointsChanged);
        }
        private void ClearSelectStroke()
        {

            if (_adorner != null)
            {
                var adorners = _adorner.GetAdorners(_inkPresenter);
                if (adorners != null && (uint)adorners.Length > 0U)
                    _adorner.Remove(adorners[0]);
            }
            _selectedStroke = null;
        }
        //private void TrainDrawingViewModel_StylusPointsChanged(object sender, EventArgs e)
        //{
        //    var label1 = sender as LabelStroke;
        //    var bounds = label1.GetBounds();
        //    foreach (var group in Strokes)
        //    {
        //        foreach (LabelStroke label2 in Strokes)
        //            if (label2.PrimaryKey == label1.PrimaryKey)
        //                label2.UpdateStroke(label1.StylusPoints);
        //    }
        //}
        void DrawingComplete()
        {
            _lable = null;
            IsDrawing = false;
        }

        public void DrawText(double x, double y, string text, Brush brush, double fontSize = 24.0)
        {
            TextBlock element = new TextBlock
            {
                Text = text,
                FontSize = fontSize,
                Foreground = brush
            };
            InkCanvas.SetLeft(element, x);
            InkCanvas.SetTop(element, y);
            base.Children.Add(element);
            this.SelectTextBlock = element;
        }
    }
}
