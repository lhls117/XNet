using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProArtist.Domain
{
    /// <summary>
    /// 控件类型
    /// </summary>
    public enum ControllerType
    {
        [Description("图片") ]
        Image,
        [Description("文字")]
        Text,
        [Description("数据")]
        Data,
        [Description("状态条")]
        StatusBar,
        [Description("弧形条")]
        ArcBar,
       
    }
}
