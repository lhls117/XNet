using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNet.Presentation.Wpf
{
    /// <summary>
    ///     扩展模块排序器。
    /// </summary>
    public interface IEntrySorter
    {
        /// <summary>
        ///     名称。
        /// </summary>
        int Index(IEntryExtension entry);
    }
}
