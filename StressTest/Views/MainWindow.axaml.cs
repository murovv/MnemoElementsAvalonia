using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using AvAp2.Models;
using IProjectModel;

namespace StressTest.Views
{
    public partial class MainWindow : Window
    {
        private int width = 30;
        private int height = 30;
        private List<CArrow> children = new List<CArrow>();
        public MainWindow()
        {
            InitializeComponent();
            for (int i = 0; i < width; i++)
            {
                StackPanel column = new StackPanel();
                for (int j = 0; j < height; j++)
                {
                    var arrow = new CArrow()
                    {
                        Height = 30,
                        Width = 30,
                        ControlISSelected = true
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