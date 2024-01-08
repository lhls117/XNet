using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;

namespace XNet.Controls
{
    public class LabelStroke : Stroke
    {

        #region Fields


        public bool polygonCompleted = false;
        bool _isVisible = true;
        Pen _pen;
        Pen _previousPen;
        Brush _brush;
        Brush _previousBrush;
        Guid _primaryKey;
        DrawingType _drawingType = DrawingType.Classification;

        #endregion
        #region Properties

        /// <summary>获取此ROI的唯一标志
        /// 
        /// </summary>
        public Guid PrimaryKey
        {
            get { return this._primaryKey; }
        }
        /// <summary>获取或设置可见性
        /// 
        /// </summary>
        public bool IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsVisible)));
                this._isVisible = value;
                this.UpdateVisible();
            }
        }
        /// <summary>ROI绘制类型
        /// 
        /// </summary>
        public DrawingType DrawingType
        {
            get => _drawingType;
        }
        /// <summary>获取或设置填充色
        /// 默认值为Transparent
        /// </summary>
        public Brush Brush
        {
            get => _brush;
            set
            {
                _brush = value;
                _previousBrush = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Brush)));
            }
        }
        /// <summary>获取或设置绘制ROI的Pen
        /// 
        /// </summary>
        public Pen Pen
        {
            get => _pen;
            set
            {
                _pen = value;
                _previousPen = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Pen)));
            }
        }
        /// <summary>获取所有点位
        /// 
        /// </summary>
        public Point[] Points
        {
            get
            {

                var enumerator = this.StylusPoints.GetEnumerator();
                var pointList = new List<Point>();
                while (enumerator.MoveNext())
                    pointList.Add((Point)enumerator.Current);
                if (pointList.Count == 2 && pointList[0] == pointList[1])
                    pointList.Remove(pointList[1]);
                if ((this.DrawingType == DrawingType.Rectangle || this.DrawingType == DrawingType.Ellipse) && pointList.Count == 2)
                {
                    List<Point> list = new List<Point>();
                    list.Add(pointList.First());
                    Point point = new Point();
                    point.X = pointList.Last().X;
                    point.Y = pointList.First().Y;
                    list.Add(point);
                    list.Add(pointList.Last());
                    point = new Point();
                    point.X = pointList.First().X;
                    point.Y = pointList.Last().Y;
                    list.Add(point);
                    return list.ToArray();
                    ;
                }
                else
                {
                    return pointList.ToArray();
                }

            }
        }
        /// <summary>第一个点位
        /// 
        /// </summary>
        public Point FirstPoint
        {
            get
            {
                var stylusPoint = this.StylusPoints.First();
                var x = stylusPoint.X;
                stylusPoint = this.StylusPoints[0];
                var y = stylusPoint.Y;
                return new Point(x, y);
            }
        }
        /// <summary>最后一个点位
        /// 
        /// </summary>
        public Point LastPoint
        {
            get
            {
                var stylusPoint = this.StylusPoints.Last();
                var x = stylusPoint.X;
                stylusPoint = this.StylusPoints[0];
                var y = stylusPoint.Y;
                return new Point(x, y);
            }
        }
        #endregion

        #region Constructor

        public LabelStroke(StylusPointCollection pts, DrawingType mode, Pen pen = null) : base(pts)
        {

            _primaryKey = new Guid();
            var color = new SolidColorBrush(Colors.Red);
            this.StylusPoints = pts;
            this._pen = pen == null ? new Pen(new SolidColorBrush(Colors.Red), 1) : pen;
            this._brush = (Brush)Brushes.Transparent;
            _drawingType = mode;
        }

        #endregion
        #region Drawing

        protected override void DrawCore(DrawingContext drawingContext, DrawingAttributes drawingAttributes)
        {
            if (_drawingType == DrawingType.Classification)
                return;
            drawingAttributes.Height = this._pen.Thickness;
            drawingAttributes.Width = this._pen.Thickness;
            switch (_drawingType)
            {
                case DrawingType.Line:
                    {
                        this.DrawLine(drawingContext);

                        break;
                    }
                case DrawingType.PolyLine:
                    {
                        this.DrawPolyLine(drawingContext);
                        break;
                    }
                case DrawingType.Rectangle:
                    {
                        this.DrawRectangle(drawingContext);
                        break;
                    }
                case DrawingType.Ellipse:
                    {
                        this.DrawEllipse(drawingContext);
                        break;
                    }
                case DrawingType.Polygon:
                    {
                        this.DrawPolyGon(drawingContext);
                        break;
                    }
            }
        }

        private void DrawLine(DrawingContext context)
        {
            this._pen.LineJoin = PenLineJoin.Round;
            this._pen.EndLineCap = PenLineCap.Round;
            this._pen.StartLineCap = PenLineCap.Round;
            if (this.StylusPoints.Count == 1)
                context.DrawLine(this._pen, (Point)this.StylusPoints[0], (Point)this.StylusPoints[0]);
            else
                context.DrawLine(this._pen, (Point)this.StylusPoints[0], (Point)this.StylusPoints[1]);
        }

        private void DrawPolyLine(DrawingContext context)
        {
            var enumerator = this.StylusPoints.GetEnumerator();
            var source = new List<Point>();
            while (enumerator.MoveNext())
                source.Add((Point)enumerator.Current);
            var streamGeometry = new StreamGeometry();
            using (var streamGeometryContext = streamGeometry.Open())
            {
                streamGeometryContext.BeginFigure(source[0], false, false);
                streamGeometryContext.PolyLineTo((IList<Point>)source.Skip<Point>(1).ToArray<Point>(), true, true);
            }

            this._pen.LineJoin = PenLineJoin.Round;
            this._pen.EndLineCap = PenLineCap.Round;
            this._pen.StartLineCap = PenLineCap.Round;
            context.DrawGeometry(this._brush, this._pen, (Geometry)streamGeometry);
        }

        private void DrawRectangle(DrawingContext context)
        {
            if (this.StylusPoints[0] == this.StylusPoints[1])
            {
                var stylusPoint = (Point)this.StylusPoints[0];
                this.StylusPoints[0] = new StylusPoint(stylusPoint.X - 0.5, stylusPoint.Y - 0.5);
                this.StylusPoints[1] = new StylusPoint(stylusPoint.X + 0.5, stylusPoint.Y + 0.5);
            }

            var rectangle = new Rect((Point)this.StylusPoints[0], (Point)this.StylusPoints[1]);

            // this.pen.Thickness = 1.0;
            context.DrawRectangle(this._brush, this._pen, rectangle);
        }

        private void DrawEllipse(DrawingContext context)
        {
            var center = new Point();
            if (this.StylusPoints[0] == this.StylusPoints[1])
            {
                var stylusPoint = (Point)this.StylusPoints[0];
                this.StylusPoints[0] = new StylusPoint(stylusPoint.X - 0.5, stylusPoint.Y - 0.5);
                this.StylusPoints[1] = new StylusPoint(stylusPoint.X + 0.5, stylusPoint.Y + 0.5);
            }

            // this._pen.Thickness = 1.0;
            center.X = (this.StylusPoints[0].X + this.StylusPoints[1].X) / 2.0;
            center.Y = (this.StylusPoints[0].Y + this.StylusPoints[1].Y) / 2.0;
            var stylusPoint1 = this.StylusPoints[0];
            var x1 = stylusPoint1.X;
            stylusPoint1 = this.StylusPoints[1];
            var x2 = stylusPoint1.X;
            var radiusX = Math.Abs(x1 - x2) / 2.0;
            stylusPoint1 = this.StylusPoints[0];
            var y1 = stylusPoint1.Y;
            stylusPoint1 = this.StylusPoints[1];
            var y2 = stylusPoint1.Y;
            var radiusY = Math.Abs(y1 - y2) / 2.0;
            context.DrawEllipse(this._brush, this._pen, center, radiusX, radiusY);
        }

        private void DrawPolyGon(DrawingContext context)
        {
            var enumerator = this.StylusPoints.GetEnumerator();
            var source = new List<Point>();
            while (enumerator.MoveNext())
                source.Add((Point)enumerator.Current);
            var streamGeometry = new StreamGeometry();
            using (var streamGeometryContext = streamGeometry.Open())
            {
                streamGeometryContext.BeginFigure(source[0], true, this.polygonCompleted);
                streamGeometryContext.PolyLineTo((IList<Point>)source.Skip<Point>(1).ToArray<Point>(), true, true);
            }

            if (!this.polygonCompleted)
            {
                this._pen.Thickness = 1.0;
                context.DrawGeometry((Brush)null, this._pen, (Geometry)streamGeometry);
            }
            else
            {
                this._pen.Thickness = 1.0;
                context.DrawGeometry(this._brush, this._pen, (Geometry)streamGeometry);
            }
        }

        #endregion
        public bool IsPolygonModeOnOff()
        {
            return this.StylusPoints[0] == this.StylusPoints[this.StylusPoints.Count - 1];
        }

        public void DistanceInit(Point distance)
        {
            var stylusPointCollection = new StylusPointCollection();
            foreach (var stylusPoint1 in (Collection<StylusPoint>)this.StylusPoints)
            {
                var stylusPoint2 = new StylusPoint(stylusPoint1.X - distance.X, stylusPoint1.Y - distance.Y);
                stylusPointCollection.Add(stylusPoint2);
            }

            this.StylusPoints = stylusPointCollection;
        }

        public void ChangeAndAddPoint(Point newPoint)
        {
            var stylusPointCollection = new StylusPointCollection();
            foreach (var stylusPoint1 in (Collection<StylusPoint>)this.StylusPoints)
            {
                var stylusPoint2 = new StylusPoint(stylusPoint1.X + newPoint.X, stylusPoint1.Y + newPoint.Y);
                stylusPointCollection.Add(stylusPoint2);
            }

            this.StylusPoints = stylusPointCollection;
        }

        public void ChangeEndPoint(StylusPoint newPoint)
        {
            this.StylusPoints[this.StylusPoints.Count - 1] = newPoint;
        }

        public void AddPolyLinePoint(StylusPoint point)
        {
            this.StylusPoints.Add(point);
        }
        /// <summary>刷新可视化状态
        /// 
        /// </summary>
        void UpdateVisible()
        {

            if (!this.IsVisible)
            {
                this._previousBrush = _brush;
                this._previousPen = _pen;
                this._brush = (Brush)Brushes.Transparent;
                this._pen = new Pen(Brushes.Transparent, 1);
                if (this.StylusPoints == null)
                    return;
                this.UpdateStroke(this.StylusPoints);
            }
            else
            {
                this._pen = this._previousPen;
                this._brush = this._previousBrush;
                if (this.StylusPoints != null)
                    this.UpdateStroke(this.StylusPoints);
            }
        }
        /// <summary>刷新画面，当改变了画笔或Brush
        /// 
        /// </summary>
        /// <param name="pts"></param>
        public void UpdateStroke(StylusPointCollection pts)
        {
            this.StylusPoints = pts;
        }


        /// <summary>绘制完成（多边形自动闭合）
        /// 
        /// </summary>
        public void DrawComplte()
        {
            if (this.StylusPoints.Count <= 2)
                return;
            this.polygonCompleted = true;
            var stylusPoints = this.StylusPoints;
            var stylusPointCollection = new StylusPointCollection();
            for (var index = 0; index < stylusPoints.Count - 1; ++index)
                stylusPointCollection.Add(stylusPoints[index]);
            if (stylusPointCollection.Count > 0)
                this.StylusPoints = stylusPointCollection;
            else
                this.StylusPoints = stylusPoints;
        }

        public bool IsSectionModeEnd()
        {
            if (this.StylusPoints[0] == this.StylusPoints[this.StylusPoints.Count - 1])
                return true;
            if (this.StylusPoints.Count <= 2)
                return false;
            var stylusPoint1 = this.StylusPoints[0];
            var stylusPoint2 = this.StylusPoints[this.StylusPoints.Count - 1];
            return (stylusPoint2.X <= stylusPoint1.X + 5.0 && stylusPoint2.X >= stylusPoint1.X - 5.0) &
                   (stylusPoint2.Y <= stylusPoint1.Y + 5.0 && stylusPoint2.Y >= stylusPoint1.Y - 5.0);
        }
        /// <summary>获取此ROI形状的描述
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetDescription()
        {
            var element = ((IEnumerable<MemberInfo>)_drawingType.GetType().GetMember(_drawingType.ToString())).FirstOrDefault<MemberInfo>();
            return (object)element != null ? element.GetCustomAttribute<DescriptionAttribute>()?.Description : (string)null;
        }
    }
}
