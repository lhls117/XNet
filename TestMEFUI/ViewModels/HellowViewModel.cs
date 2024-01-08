using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMEFUI.Views;
using XNet.Presentation;
using XNet.Presentation.Wpf;

namespace TestMEFUI.ViewModels
{
    [Export]
    public class HellowViewModel : ViewModel<IHelloView>
    {
        [ImportingConstructor]
        public HellowViewModel(IHelloView view) : base(view)
        {
        }
    }
}
