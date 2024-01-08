using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProArtist.Infrastructure.Extensions
{
    public static class DescriptionExtension
    {
        /// <summary>
        /// 获取枚举的描述信息
        /// </summary>
        public static string GetDescription(this Enum em)
        {
            Type type = em.GetType();
            FieldInfo fd = type.GetField(em.ToString());
            string des = fd.GetDescription();
            return des;
        }

        /// <summary>
        /// 获取属性的描述信息
        /// </summary>
        public static string GetDescription(this Type type, string proName)
        {
            PropertyInfo pro = type.GetProperty(proName);
            string des = proName;
            if (pro != null)
            {
                des = pro.GetDescription();
            }
            return des;
        }


        /// <summary>
        /// 获取属性的描述信息
        /// </summary>
        public static string GetDescription(this MemberInfo info)
        {
            var attrs = (DescriptionAttribute[])info.GetCustomAttributes(typeof(DescriptionAttribute), false);
            string des = info.Name;
            foreach (DescriptionAttribute attr in attrs)
            {
                des = attr.Description;
            }
            return des;
        }

    }
}
