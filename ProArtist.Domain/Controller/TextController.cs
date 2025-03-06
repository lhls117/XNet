
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProArtist.Domain
{
    public class TextController:IController
    {
        public TextController()
        {
            Type= ControllerType.Text;
            Des ="请输入内容";
        }
        public int Index { get; set; }
        public string Des { get; set; }
        public  ControllerType Type { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string Text { get; set; }
        public int FontSize { get; set; }
        public string FontFamily { get; set; }
        public bool IsBold { get; set; }
        public Guid Id { get; set; }
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
