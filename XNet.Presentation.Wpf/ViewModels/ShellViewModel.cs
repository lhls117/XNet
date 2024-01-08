using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XNet.Presentation.Wpf.Views;

namespace XNet.Presentation.Wpf.ViewModels
{
    [Export]
    public class ShellViewModel : ViewModel<IShellView>
    {
        [ImportingConstructor]
        public ShellViewModel(IShellView view) : base(view)
        {
        }

        private object _contentView;
        public object ContentView
        {
            get => _contentView;
            set => SetProperty(ref _contentView, value);
        }

        public void Show()
        {
            View.Show();
        }
    }
}
