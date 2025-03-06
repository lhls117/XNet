using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNet.Presentation.Wpf.Services;
using XNet.Presentation.Wpf.ViewModels;
using XNet.Presentation.Wpf.Views;

namespace XNet.Presentation.Wpf.Controllers
{
    /// <summary>
    ///     Responsible for the application lifecycle.
    /// </summary>
    [Export(typeof(IApplicationController))]
    [Export(typeof(IModuleController))]
    [Shared]
    public class ApplicationController : IApplicationController
    {
        private readonly Lazy<MainViewModel> _mainViewModel;
        private readonly Lazy<ShellViewModel> _shellViewModel;
        private readonly IShellService shellService;
        

        [ImportingConstructor]
        public ApplicationController(Lazy<ShellViewModel> shellViewModel,Lazy<MainViewModel> mainViewModel,IShellService shellService)
        {

            _shellViewModel = shellViewModel;
            _mainViewModel = mainViewModel;
            this.shellService = shellService;

        }

        private ShellViewModel ShellViewModel => _shellViewModel.Value;
        private MainViewModel MainViewModel => _mainViewModel.Value;

        #region implete of IApplicationController
        public void Initialize()
        {
           
        }

        public void Run()
        {
            ((ShellService)shellService).ShellView = ShellViewModel.View;
            ((ShellService)shellService).DialogHost = (ShellViewModel.View as IShellView)?.DialogHost;

            ShellViewModel.ContentView = MainViewModel.View;
            ShellViewModel.Show();
        }

        public void Shutdown()
        {
           
        }
        #endregion
    }
}
