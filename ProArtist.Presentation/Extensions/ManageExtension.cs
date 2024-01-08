using ProArtist.Presentation.ViewModels;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNet.Presentation.Wpf;

namespace ProArtist.Presentation.Extensions
{
    [Export(typeof(IEntryExtension))]
    public class ManageExtension : EntryExtension<ManageViewModel>
    {
        [ImportingConstructor]
        public ManageExtension(ExportFactory<ManageViewModel> viewModelFactory) : base(viewModelFactory)
        {
            Name = "设备管理";
        }
    }
}
