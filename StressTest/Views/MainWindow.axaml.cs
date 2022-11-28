using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using AvAp2;
using AvAp2.Models;
using IProjectModel;

namespace StressTest.Views
{
    public partial class MainWindow : Window
    {
        private int width = 15;
        private int height = 15;
        private List<CAutomaticSwitch> children = new List<CAutomaticSwitch>();
        public MainWindow()
        {
            InitializeComponent();
            for (int i = 0; i < width; i++)
            {
                StackPanel column = new StackPanel();
                for (int j = 0; j < height; j++)
                {
                    var arrow = new CAutomaticSwitch()
                    {
                        Height = 30,
                        Width = 30,
                        ControlISSelected = true,
                        Angle = 5,
                        TagDataBanners = new TagDataItem(null)
                        {
                            TagValueString = "3"
                        }
                    };
                    column.Children.Add(arrow);
                    children.Add(arrow);
                }
                Panel.Children.Add(column);
            }
            
            Button.Click+= ButtonOnClick;
        }

        private void ButtonOnClick(object? sender, RoutedEventArgs e)
        {
            foreach (var arrow in children)
            {
                arrow.Angle += 5;
            }
        }
    }
}