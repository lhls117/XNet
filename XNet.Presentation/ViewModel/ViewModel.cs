using System;
using System.Collections.Generic;
using System.Text;

namespace XNet.Presentation
{
    /// <summary>
    /// 抽象的ViewModel，ViewModel包含了View
    /// </summary>
    public abstract class ViewModel : Model
    {
        #region ctor

        protected ViewModel(IView view) : this(view, true) { }
        protected ViewModel(IView view, bool initializeDataContext)
        {
            View = view ?? throw new ArgumentNullException(nameof(view));
            if (initializeDataContext)
            {
                view.DataContext = this;
            }
        }

       

        #endregion

        /// <summary>
        /// ViewModel包含的View视图
        /// </summary>

        public IView View { get; }
    }
}
