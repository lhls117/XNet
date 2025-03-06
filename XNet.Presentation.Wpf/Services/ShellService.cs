using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNet.Presentation.Wpf.Services
{
    [Export(typeof(IShellService))]
    [Shared]
    public class ShellService : IShellService
    {
        #region implemention of IShellService
        public object DialogHost { get; internal set; }

        public object ShellView { get; internal set; }
        #endregion
    }
}
