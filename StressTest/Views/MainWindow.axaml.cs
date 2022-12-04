using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Timers;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Rendering;
using Avalonia.VisualTree;
using AvAp2;
using AvAp2.Models;
using TagValueQuality = AvAp2.TagValueQuality;

namespace StressTest.Views
{ 
    public partial class MainWindow : Window
    {
        private int width = 50;
        private int height = 50;

        TagDataItem tdi = new TagDataItem(null);
        List<CAutomaticSwitch> children = new List<CAutomaticSwitch>();
        public void Run()
        {
            Panel.Children.Clear();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < width; i++)
            {
                StackPanel column = new StackPanel();
                for (int j = 0; j < height; j++)
                {
                    var arrow = new CAutomaticSwitch()
                    {
                        Height = 50,
                        Width = 100,
                        ControlISSelected = true,
                        Angle = 5,
                        TextName = "всем тестам тест",
                        TagDataMainState = tdi,
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
                        , MarginBanner = new Avalonia.Thickness(30, 30, 0, 0)
                    };
                    column.Children.Add(arrow);
                    children.Add(arrow);
                }
                Panel.Children.Add(column);
            }
            TextBlock.Text = "Нарисовали за " + sw.ElapsedMilliseconds + " мс" ;
        }
        public MainWindow()
        {
            InitializeComponent();
            Run();
            Button.Click+= ButtonOnClick;
            ButtonChange.Click += ButtonChangeClick;
        }

        private void ButtonOnClick(object? sender, RoutedEventArgs e)
        {
            Run();
        }

        private void ButtonChangeClick(object? sender, RoutedEventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            /*tdi.TagValueString = tdi.TagValueString == "1" ? "2" : "1";*/
            foreach (CAutomaticSwitch control in children)
            {
                /*control.ShowNormalState = !control.ShowNormalState;
                control.IsConnectorExistLeft = !control.IsConnectorExistLeft;
                control.IsConnectorExistRight = !control.IsConnectorExistRight;*/
                control.TagDataBanners.TagValueString = "1";
                control.TextName = "Другое имя";
            }
            TextBlock.Text = "Изменили за " + sw.ElapsedMilliseconds + " мс";
        }
    }
}