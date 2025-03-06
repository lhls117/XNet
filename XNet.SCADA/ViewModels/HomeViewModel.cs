using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNet.Presentation.Wpf;
using XNet.SCADA.Views;

namespace XNet.SCADA.ViewModels
{
    [Export]
    public class HomeViewModel : ViewModel<IHomeView>
    {
        [ImportingConstructor]
        public HomeViewModel(IHomeView view) : base(view)
        {

        }
    }
}
