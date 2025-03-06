using ProArtist.Domain;
using ProArtist.Presentation.Theme.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNet.Presentation.Wpf;
using ProArtist.Infrastructure.Extensions;
using System.Windows.Input;
using XNet.Presentation;
using AutoMapper;
using ProArtist.Presentation.Theme.Models;
using System.Windows.Media;
using System.Drawing;
using ProArtist.Presentation.Theme.Helps;
using XNet.Controls;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Extensions.DependencyInjection;

namespace ProArtist.Presentation.Theme.ViewModels
{
    [Export]
    public class ThemeViewModel : ViewModel<IThemeView>
    {
        [ImportingConstructor]
        public ThemeViewModel(IThemeView view,ExportFactory<TextControllerViewModel> textFactory) : base(view)
        {
            controllers = new ObservableCollection<IController>();
            Init();
            AddControllerCmd = new DelegateCommand(AddController);
            this.textFactory = textFactory;

            //Bitmap bitmap = new Bitmap("科技数据化主题.png");
            //Image = BitmapHelper.ConvertToBitmapImage(bitmap);
            //drawingControl.DrawingCanvas.TextBlockEvent += DrawingCanvas_TextBlockEvent;
            this.View.PointChanged += View_PointChanged;
            this.View.FontSizeChanged += View_FontSizeChanged;

        }

        private void View_FontSizeChanged(double obj)
        {
            switch (selectedController.Type)
            {
                case ControllerType.Image:
                    break;
                case ControllerType.Text:
                    ((TextControllerViewModel)this.selectContent).Controller.FontSize = (int)obj;
                    break;
                case ControllerType.Data:
                    break;
                case ControllerType.StatusBar:
                    break;
                case ControllerType.ArcBar:
                    break;
                default:
                    break;
            }
        }

        private void View_PointChanged(System.Windows.Point obj)
        {
            switch (selectedController.Type)
            {
                case ControllerType.Image:
                    break;
                case ControllerType.Text:
                    ((TextControllerViewModel)this.selectContent).Controller.X =(int) obj.X;
                    ((TextControllerViewModel)this.selectContent).Controller.Y = (int)obj.Y;
                    break;
                case ControllerType.Data:
                    break;
                case ControllerType.StatusBar:
                    break;
                case ControllerType.ArcBar:
                    break;
                default:
                    break;
            }
        }

        private void DrawingCanvas_TextBlockEvent(int arg1, int arg2)
        {
            if(this.selectContent is TextControllerViewModel)
            {
                ((TextControllerViewModel)this.selectContent).Controller.X=arg1;
                ((TextControllerViewModel)this.selectContent).Controller.Y = arg2;
            }
        }

        private ExportFactory< TextControllerViewModel> textFactory;
       // private DrawingControl drawingControl=>this.View.DrawingControl;

        private ObservableCollection<string> types;
        public ObservableCollection<string> Types
        {
            get => types;
            set=>SetProperty(ref types, value);
        }

        private int selectedType;
        public int SelectedType
        {
            get => selectedType;
            set=>SetProperty(ref selectedType, value);
        }

        private ObservableCollection<IController> controllers;
        public ObservableCollection<IController> Controllers
        {
            get => controllers;
            set=>SetProperty(ref controllers, value);
        }

        private IController selectedController;
        public IController SelectedController
        {
            get => selectedController;
            set
            {
                SetProperty(ref selectedController, value);
                switch(value.Type)
                {
                    case ControllerType.Text:
                        var viewmodel= textFactory.CreateExport().Value;
                        viewmodel.Controller = (TextModel)value;
                        this.SelectContent = viewmodel;
                        
                        break;
                }
            }
        }

       

        private ViewModel selectContent;
        public ViewModel SelectContent
        {
            get => selectContent;
            set=> SetProperty(ref selectContent, value);
        }


        private ImageSource image;
        public ImageSource Image
        {
            get => image;
            set => SetProperty(ref image, value);
        }

        
        public ICommand AddControllerCmd { get; set; }



        private void AddController()
        {
            ControllerType controllerType = (ControllerType)selectedType;
            switch (controllerType)
            {
               
                case ControllerType.Text:
                    TextModel text = new TextModel();
                    text.Index = CreatControllerHelp.CreatControllerIndex(controllers.ToList());
                    text.Name = controllerType.ToString() + text.Index;
                    Controllers.Add(text);
                    TextBlock element = new TextBlock
                    {
                       
                        Text = "text",
                        FontSize = 20,
                        Foreground = new SolidColorBrush(Colors.Red),
                        Name =text.Name,
                    };
                    Panel.SetZIndex(element, 1);
                    Canvas.SetLeft(element, 200);
                    Canvas.SetTop(element, 200);
                    this.View.Canvas.Children.Add(element);
                    break;
            }

            //   this.View.DrawingControl.DrawingCanvas.DrawText(10, 10, "text", new SolidColorBrush(Colors.Red));
            // this.View.DrawingControl.DrawingCanvas.EditMode = XNet.Controls.DrawingCanvasEditMode.Select;

            //drawingControl.DrawingCanvas.Cursor= Cursors.Arrow;

            //TextBlock element = new TextBlock
            //{
            //    Text = "text",
            //    FontSize = 10,
            //    Foreground = new SolidColorBrush(Colors.Red),
            //};
            //Panel.SetZIndex(element, 1);
            //Canvas.SetLeft(element, 200);
            //Canvas.SetTop(element, 200);
            //this.View.Canvas.Children.Add(element);
        }

        #region functions
        private void Init()
        {
            types = new ObservableCollection<string>();
            foreach (var  type in Enum.GetValues(typeof(ControllerType)))
            {
                ControllerType controllerType = (ControllerType)type;
                string des = controllerType.GetDescription();
               types.Add(des);
            }
        }


        #endregion
    }
}
