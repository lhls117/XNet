using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows;

namespace XNet.Controls
{
    internal class InkPresenterAdorner : Adorner
    {
        protected override int VisualChildrenCount
        {
            get { return 1; }
        }
        InkPresenter _inkPresenter;
        public InkPresenterAdorner(UIElement adornedElement, InkPresenter inkPresenter)
            : base(adornedElement)
        {
            _inkPresenter = inkPresenter;
            this.AddVisualChild(_inkPresenter);
        }
        protected override Visual GetVisualChild(int index)
        {
            return _inkPresenter;
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            _inkPresenter.Arrange(new Rect(arrangeBounds));
            return arrangeBounds;
        }
    }
}
