using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace wp1czerwca
{
    public enum Animal
    {
        Bull,
        Tiger,
        None
    }

    public class Cell
    {
        public static Size CellSize;

        public static Game GameContext;

        public Viewbox image;
        public Animal Type { get; private set; }
        public int Row { get; set; }
        public int Column { get; set; }

        public Cell(Game game, int row, int column, Animal type = Animal.None)
        {
            GameContext = game;
            Type = type;
            Row = row;
            Column = column;
            image = new Viewbox();
            image.MouseLeftButtonUp += Clicked;
            Grid.SetRow(image, row);
            Grid.SetColumn(image, column);

            BitmapImage bmp = new BitmapImage();
            if (type != Animal.None)
            {
                bmp.BeginInit();
                switch (Type)
                {
                    case Animal.Bull:
                        bmp.UriSource = GameContext.Settings.Bull;
                            //new Uri(
                              //  @"C:\Users\Dawid\Documents\Visual Studio 2013\Projects\wp1czerwca\wp1czerwca\obrazki\byk.png");
                        break;
                    case Animal.Tiger:
                        bmp.UriSource = GameContext.Settings.Tiger;
                        //    new Uri(
                        //        @"C:\Users\Dawid\Documents\Visual Studio 2013\Projects\wp1czerwca\wp1czerwca\obrazki\tygrys.png");
                        break;
                }
                bmp.EndInit();
                image.Child = new Image
                {
                    Source = bmp,
                    Stretch = Stretch.Fill
                };
            }
            else
            {
                image.Child = new Button{
                    Background = Brushes.LightGreen,
                    Foreground = Brushes.LightGreen,
                    Opacity = 0
                };
                (image.Child as Button).Click += Clicked;
            }
        }

        private void Clicked(object sender, EventArgs e)
        {
            GameContext.PerformAction(this);
        }

        public void Erase()
        {
            image.Child = new Button{
                Opacity = 0,
            };
            (image.Child as Button).Click += Clicked;
            Type = Animal.None;
        }
    }  
}
