using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Rendering;
using Avalonia.VisualTree;
using AvAp2;
using AvAp2.Models;
using IProjectModel;

namespace StressTest.Views
{
    public partial class MainWindow : Window
    {
        private int width = 35;
        private int height = 35;
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
                        ControlISSelected = true,
                        Angle = 5,
                    };
                    column.Children.Add(arrow);
                    children.Add(arrow);
                }
                Panel.Children.Add(column);
                this.Renderer.DrawFps = true;
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