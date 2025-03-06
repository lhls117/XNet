using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XNet.Presentation.Wpf;
using XNet.SCADA.Views;

namespace XNet.SCADA.ViewModels
{

    [Export]
    public class AboutViewModel : ViewModel<IAboutView>
    {
        [ImportingConstructor]
        public AboutViewModel(IAboutView view) : base(view)
        {
           
        }

       

        public async Task ShowDialogAsync(object dialogHost)
        {
            await View.ShowDialogAsync(dialogHost);
        }
    }
}
