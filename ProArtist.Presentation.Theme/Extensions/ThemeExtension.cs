using ProArtist.Presentation.Theme.ViewModels;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNet.Presentation.Wpf;

namespace ProArtist.Presentation.Theme.Extensions
{
    [Export(typeof(IEntryExtension))]
    public class ThemeExtension : EntryExtension<ThemeViewModel>
    {
        [ImportingConstructor]
        public ThemeExtension(ExportFactory<ThemeViewModel> viewModelFactory) : base(viewModelFactory)
        {
            Name = "主题设置";
        }
    }
}
