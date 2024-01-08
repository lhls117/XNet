using ProArtist.Presentation.Extensions;
using ProArtist.Presentation.Theme.Extensions;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNet.Presentation.Wpf;

namespace ProArtist.Extensions
{
    [Export(typeof(IEntrySorter))]
    public class ProAttistEntrySorter : IEntrySorter
    {
        public int Index(IEntryExtension entry)
        {
             if(entry is HomeExtension)
            {
                return 1;
            }
            if(entry is ManageExtension )
            {
                return 2;
            }
            if(entry  is ThemeExtension)
            {
                return 3;
            }
            if(entry is SetExtension )
            {
                return 4;
            }
            if (entry is AboutExtension)
            {
                return 10;
            }
            return 5;
        }
    }
}
