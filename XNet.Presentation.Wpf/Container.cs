using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNet.Presentation.Wpf
{
    public class Container
    {
        private Container()
        {
        }

        public static Container Default { get; } = new Container();

        public ServiceProvider Services { get; set; }
    }
}
