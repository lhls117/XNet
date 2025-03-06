using ProArtist.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProArtist.Presentation.Theme.Helps
{
   public static class CreatControllerHelp
    {
        public static int CreatControllerIndex(List<IController> controllers)
        {
            if(controllers.Count == 0)
            {
                return 0;
            }
            else
            {
                return controllers.OrderByDescending(c => c.Index).First().Index + 1;
            }
            
        }
    }
}
