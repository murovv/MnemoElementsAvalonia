using System.Collections.Generic;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using AvAp2;
using AvAp2.Models;

namespace StressTest.Views;

public partial class AlarmWindow : Window
{
    
        private int width = 20;
        private int height = 20;

        TagDataItem tdi = new TagDataItem(null);
        List<CAlarmIndicator> children = new List<CAlarmIndicator>();
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
                    var arrow = new CAlarmIndicator()
                    {
                        IsReceipt = true,
                        Width = 30,
                        Height = 30,
                        Margin = new Thickness(20,20),
                        TextName = ""
                    };
                    column.Children.Add(arrow);
                    children.Add(arrow);
                }
                Panel.Children.Add(column);
            }
            TextBlock.Text = "Нарисовали за " + sw.ElapsedMilliseconds + " мс" ;
        }
        public AlarmWindow()
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
            foreach (CAlarmIndicator control in children)
            {
                /*control.ShowNormalState = !control.ShowNormalState;
                control.IsConnectorExistLeft = !control.IsConnectorExistLeft;
                control.IsConnectorExistRight = !control.IsConnectorExistRight;*/
                control.Angle += 5;
            }
            TextBlock.Text = "Изменили за " + sw.ElapsedMilliseconds + " мс";
        }
}