using System;
using System.Collections.Generic;
using System.Text;

namespace XNet.Presentation
{
    /// <summary>
    ///     Abstract base class for a ViewModel implementation with a simple approach to set the DataContext.
    /// </summary>
    public abstract class ViewModel : Model
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ViewModel" /> class and attaches itself as DataContext to the
        ///     view.
        /// </summary>
        /// <param name="view">The view.</param>
        protected ViewModel(IView view) : this(view, true) { }

        /// <summary>
        /// Initializes a new instance and attaches itself as DataContext to the view.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="initializeDataContext">If set to true then the DataContext of the view is set within this constructor call. Default is true.</param>
        protected ViewModel(IView view, bool initializeDataContext)
        {
            View = view ?? throw new ArgumentNullException(nameof(view));
            if (initializeDataContext)
            {
                view.DataContext = this;
            }
        }

        /// <summary>
        ///     Gets the associated view.
        /// </summary>
        public IView View { get; }
    }
}
