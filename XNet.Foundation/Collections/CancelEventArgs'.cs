using System;
using System.Collections.Generic;
using System.Text;

namespace XNet.Foundation.Collections
{

    /// <inheritdoc />
    /// <summary>
    ///     为 <see cref="E:System.Patterns.Collections.NodeAdding" /> 和
    ///     <see cref="E:System.Patterns.Collections.NodeRemoving" /> 事件提供数据。
    /// </summary>
    public class CancelEventArgs<T> : EventArgs
    {
        /// <summary>
        ///     当前操作的子节点。
        /// </summary>
        public T Target { get; set; }

        /// <summary>获取或设置一个值，该值指示是否应取消事件。</summary>
        /// <returns>true 如果应取消事件;否则为 false。</returns>
        public bool Cancel { get; set; }
    }
}
