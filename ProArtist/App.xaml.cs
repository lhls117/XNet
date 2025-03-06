using System.Configuration;
using System.Data;
using System.Windows;
using XNet.Presentation.Wpf.Controllers;
using XNet.Presentation.Wpf;
using AutoMapper;

using System.Reflection;

namespace ProArtist
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

            var config = new MapperConfiguration(cfg =>
            {
                var allTypes = AppDomain.CurrentDomain.GetAssemblies()
                  .Where(a => a.GetName().Name != nameof(AutoMapper))
                  .SelectMany(a => a.DefinedTypes)
                  .ToArray();

                var profiles = allTypes
                    .Where(t => typeof(Profile).GetTypeInfo().IsAssignableFrom(t) && !t.IsAbstract);

                 foreach (var profile in profiles.Select(t=>t.AsType()))
                {
                    cfg.AddProfile(profile);
                }

                //cfg.AddProfile<MapperProfile>();
            });

           
            XNet.Presentation.Wpf.Container.Default.Services = Container.Default.Services;
            using (var container = Container.Default.Composition.CreateContainer())
            {
                _moduleControllers = container.GetExports<IModuleController>();
                foreach (var moduleController in _moduleControllers) { moduleController.Initialize(); }
                _applicationController = (IApplicationController)_moduleControllers.First(o => o is IApplicationController);
                _applicationController.Run();
            }

        }
    }

}
