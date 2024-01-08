using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProArtist.Domain
{
    public interface IController
    {
        int Index { get;set; }
        string Des { get;set; }

        ControllerType Type { get; set; }
        int X { get; set; }
        int Y { get; set; }
    }
}
