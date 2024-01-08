using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace XNet.Presentation.Wpf
{
    /// <summary>
    ///     Abstract base class for a generic ViewModel implementation.
    /// </summary>
    /// <typeparam name="TView">The type of the view. Do provide an interface as type and not the concrete type itself.</typeparam>
    public abstract class ViewModel<TView> : ViewModel where TView : class, IView
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ViewModel{TView}" /> class.
        /// </summary>
        /// <param name="view">The view.</param>
        protected ViewModel(TView view) : base(view, false)
        {
            // Check if the code is running within the WPF application model
            if (SynchronizationContext.Current is DispatcherSynchronizationContext)
            {
                // Set DataContext of the view has to be delayed so that the ViewModel can initialize the internal data (e.g. Commands)
                // before the view starts with DataBinding.
                Dispatcher.CurrentDispatcher.BeginInvoke((Action)delegate ()
                {
                    View.DataContext = this;
                });
            }
            else
            {
                // When the code runs outside of the WPF application model then we set the DataContext immediately.
                View.DataContext = this;
            }
        }

        /// <summary>
        ///     Gets the associated view as specified view type.
        /// </summary>
        /// <remarks>
        ///     Use this property in a ViewModel class to avoid casting.
        /// </remarks>
        protected new TView View => base.View as TView;


        /// <summary>
        ///     Attaches itself as DataContext to the view.
        /// </summary>
        /// <remarks>
        ///     Called when a part's imports have been satisfied and it is safe to use.
        ///     This method is called when MEF (Managed Extensibility Framework) is used.
        /// </remarks>
        [OnImportsSatisfied]
        public virtual void AttachView()
        {
            OnAttachView();
        }

        /// <summary>
        ///     Called when a part's imports have been satisfied and it is safe to use.
        ///     This method is called when MEF (Managed Extensibility Framework) is used.
        /// </summary>
        protected virtual void OnAttachView() { }
    }
}
