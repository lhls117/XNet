using ProArtist.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using XNet.Presentation;

namespace ProArtist.Presentation.Theme.Models
{
    public class TextDtoModel:Model,IController
    {
        public TextDtoModel()
        {
            Type = ControllerType.Text;
            Des = "请输入内容";
        }

        public int index;
        public int Index
        {
            get => index;
            set=>SetProperty(ref index, value);
        }
        public string des;
        public string Des
        {
            get => des;
            set=>SetProperty(ref des, value);
        }

        private ControllerType type;
        public ControllerType Type
        {
            get=> type;
            set => SetProperty(ref type, value);
        }
        private int x;
        public int X
        {
            get => x;
            set=>SetProperty(ref x, value);
        }
        private int y;
        public int Y
        {
            get=> y; 
            set => SetProperty(ref y, value);
        }
        private string text;
        public string Text
        {
            get => text; 
            set
            {
                SetProperty(ref text, value);
                Des = value;
            }
        }
        private int fontSize;

        public int FontSize
        {
            get => fontSize;
            set => SetProperty(ref fontSize, value);
        }

        private FontFamily fontFamily;
        public FontFamily FontFamily
        {
            get => fontFamily; 
            set => SetProperty(ref fontFamily, value);
        }

        public bool isBold;
        public bool IsBold
        {
            get => isBold;
            set => SetProperty(ref isBold, value);
        }
    }
}
