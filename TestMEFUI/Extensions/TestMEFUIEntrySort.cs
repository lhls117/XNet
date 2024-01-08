using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNet.Presentation.Wpf;

namespace TestMEFUI.Extensions
{
    [Export(typeof(IEntrySorter ))]
    public class TestMEFUIEntrySort : IEntrySorter
    {
        public int Index(IEntryExtension entry)
        {
           if(entry is HelloExtension)
            {
                return 1; 
            }
           if(entry is XNetExtension)
            {
                return 2;
            }
            return 10;
        }
    }
}
