using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNet.Presentation.Wpf
{
    /// <summary>
    ///     程序扩展。
    /// </summary>
    public interface IExtension
    {
    }
    /// <summary>
    ///     视图模型扩展。
    /// </summary>
    public interface IModelExtension : IExtension
    {
        /// <summary>
        ///     关联的 ViewModel.
        /// </summary>
      ViewModel ViewModel { get; }
    }
    /// <summary>
    ///     视图模型入口扩展。
    /// </summary>
    public interface IEntryExtension : IModelExtension
    {
        /// <summary>
        ///     图标。
        /// </summary>
        string ImageKey { get; }

        /// <summary>
        ///     名称。
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     扩展使用操作。
        /// </summary>
        string Operation { get; }
    }
}
