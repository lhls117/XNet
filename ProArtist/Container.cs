using Microsoft.Extensions.DependencyInjection;
using ProArtist.Extensions;
using ProArtist.Presentation.Theme.Views;
using ProArtist.Presentation.Views;
using System;
using System.Collections.Generic;
using System.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XNet.Presentation.Wpf.Views;

namespace ProArtist
{
    public class Container
    {
        private Container()
        {
            ConfigureComposition();
            ConfigureServices();
        }
        public static Container Default { get; } = new Container();

        public ContainerConfiguration Composition { get; set; }

        public ServiceProvider Services { get; set; }

        private void ConfigureComposition()
        {
            Composition = new ContainerConfiguration()
                .WithAssembly(typeof(App).GetTypeInfo().Assembly)
            .WithAssembly(typeof(IMainView).GetTypeInfo().Assembly)
            .WithAssembly(typeof(IThemeView).GetTypeInfo().Assembly)
            .WithAssembly(typeof(IAboutView).GetTypeInfo().Assembly);

           

        }
        private void ConfigureServices()
        {
            var service = new ServiceCollection();
            service.AddAutoMapper();
            Services = service.BuildServiceProvider();

            
        }

    }
}
