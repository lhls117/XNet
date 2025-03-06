using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNet.Presentation.Wpf.Views
{
    /// <summary>
    /// 外壳视图接口
    /// </summary>
    public interface IShellView:IView
    {
        /// <summary>
        /// 对话框宿主
        /// </summary>
        object DialogHost { get; }
        /// <summary>
        /// 显示
        /// </summary>
        void Show();
    }
}
