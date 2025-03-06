using System;
using System.Collections.Generic;
using System.Text;

namespace XNet.Presentation
{
    /// <summary>
    /// View视图接口
    /// </summary>
    public interface IView
    {
         /// <summary>
         /// View对应的上下文
         /// </summary>
        object DataContext { get; set; }
    }
}
