using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNet.Presentation.Wpf.ViewModels;

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

        [ImportingConstructor]
        public ApplicationController(Lazy<ShellViewModel> shellViewModel,Lazy<MainViewModel> mainViewModel )
        {

            _shellViewModel = shellViewModel;
            _mainViewModel = mainViewModel;

        }

        private ShellViewModel ShellViewModel => _shellViewModel.Value;
        private MainViewModel MainViewModel => _mainViewModel.Value;

        #region implete of IApplicationController
        public void Initialize()
        {
           
        }

        public void Run()
        {
            ShellViewModel.ContentView = MainViewModel.View;
            ShellViewModel.Show();
        }

        public void Shutdown()
        {
           
        }
        #endregion
    }
}
