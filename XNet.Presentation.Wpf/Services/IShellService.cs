using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNet.Presentation.Wpf.Services
{
    /// <summary>
    /// 外壳服务
    /// </summary>
    public interface IShellService
    {
        /// <summary>
        /// 对话框宿主
        /// </summary>
        object DialogHost { get; }


        /// <summary>
        /// 程序主窗体
        /// </summary>
        object ShellView { get; }
    }
}
