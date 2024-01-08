using System.Configuration;
using System.Data;
using System.Windows;
using XNet.Presentation.Wpf;
using XNet.Presentation.Wpf.Controllers;
using XNet.Presentation.Wpf.ViewModels;
using XNet.Presentation.Wpf.Views;
using System.Linq;

namespace TestMEFUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IApplicationController _applicationController;
        private IEnumerable<IModuleController> _moduleControllers;
      
        public App()
        {
            
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            XNet.Presentation.Wpf.Container.Default.Services = Container.Default.Services;
            using(var container=Container.Default.Composition.CreateContainer())
            {
               _moduleControllers= container.GetExports<IModuleController>();
                foreach(var moduleController in _moduleControllers) { moduleController.Initialize(); }
                _applicationController=(IApplicationController) _moduleControllers.First(o => o is IApplicationController);
                _applicationController.Run();
            }

        }
    }

}
