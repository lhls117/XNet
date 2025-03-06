using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNet.Presentation.Wpf;

namespace XNet.SCADA.Extensions
{
     [Export(typeof(IEntrySorter))]
    public class SortExtension : IEntrySorter
    {
        public int Index(IEntryExtension entry)
        {
            if (entry is HomeExtension)
                return 1;
            else
                return 10;
        }
    }
}
