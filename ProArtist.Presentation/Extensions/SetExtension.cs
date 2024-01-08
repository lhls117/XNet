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
    public class SetExtension : EntryExtension<SetViewModel>
    {
        [ImportingConstructor]
        public SetExtension(ExportFactory<SetViewModel> viewModelFactory) : base(viewModelFactory)
        {
            Name = "设置";
        }
    }
}
