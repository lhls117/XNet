using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMEFUI.Views;
using XNet.Presentation.Wpf;

namespace TestMEFUI.ViewModels
{
    [Export]
    public class XNetViewModel : ViewModel<IXNetView>
    {
        [ImportingConstructor]
        public XNetViewModel(IXNetView view) : base(view)
        {
        }
    }
}
