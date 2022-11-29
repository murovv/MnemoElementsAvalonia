using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Rendering;
using Avalonia.VisualTree;
using AvAp2;
using AvAp2.Models;
using IProjectModel;
using TagValueQuality = AvAp2.TagValueQuality;

namespace StressTest.Views
{
    public partial class MainWindow : Window
    {
        private int width = 50;
        private int height = 50;
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
                        TextName = "всем тестам тест",
                        TagDataMainState = new TagDataItem(null)
                        {
                            Quality = TagValueQuality.Handled
                        },
                        TagDataBanners = new TagDataItem(null)
                        {
                            TagValueString = "63"
                        },
                        TagDataControlMode = new TagDataItem(null)
                        {
                            TagValueString = "1",
                            Quality = TagValueQuality.Good
                        },
                        TagDataBlock = new TagDataItem(null)
                        {
                            TagValueString = "1",
                            Quality = TagValueQuality.Good
                        },
                        TagDataDeblock = new TagDataItem(null)
                        {
                            TagValueString = "1",
                            Quality = TagValueQuality.Good
                        },
                        TagIDBlockState = "1",
                        TagIDDeblock = "1",
                        TagIDControlMode = "1"

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