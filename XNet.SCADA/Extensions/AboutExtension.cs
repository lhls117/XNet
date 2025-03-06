using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNet.Presentation.Wpf;
using XNet.Presentation.Wpf.Services;
using XNet.SCADA.ViewModels;
using XNet.SCADA.Views;

namespace XNet.SCADA.Extensions
{

    [Export(typeof(IMenuitemExtension))]
    public class AboutExtension : MenuitemExtension<AboutViewModel>
    {
        [ImportingConstructor]
        public AboutExtension(ExportFactory<AboutViewModel> viewModelFactory,IShellService shellService) : base(viewModelFactory)
        {
            Name = "关于";
            this.shellService = shellService;
        }


        private readonly IShellService shellService;
        public override async void Execute()
        {
            await ViewModel.ShowDialogAsync(shellService.DialogHost);
        }
    }
}
