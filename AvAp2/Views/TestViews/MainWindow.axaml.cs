﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace AvAp2.Views.TestViews
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CAlarmIndicator1.IsReceipt = true;
            CAlarmIndicator1.IsActive = true;
            CAlarmIndicator1.TagDataMainState = new TagDataItem(null);
            ChangeIsReceipt.Click+= ChangeIsReceiptOnClick;
            IsActive.Click+= IsActiveOnClick;
            Angle.Click+= AngleOnClick;
            EventGroupId.Click+= EventGroupIdOnClick;
            CArrowIsActive.Click+= CArrowIsActiveOnClick;
            CArrowAngle.Click+= CArrowAngleOnClick;
            CArrowReserveAngle.Click+= CArrowReserveAngleOnClick;
            CArrowReserveIsActive.Click+= CArrowReserveIsActiveOnClick;
        }

        private void CArrowReserveIsActiveOnClick(object? sender, RoutedEventArgs e)
        {
            if (CArrowReserve1.TagDataMainState == null)
            {
                CArrowReserve1.TagDataMainState = new TagDataItem(null)
                {
                    TagValueString = "0"
                };
            }
            else
            {
                CArrowReserve1.TagDataMainState = null;
            }
        }

        private void CArrowReserveAngleOnClick(object? sender, RoutedEventArgs e)
        {
            CArrowReserve1.Angle += 5;;
        }
        

        private void CArrowAngleOnClick(object? sender, RoutedEventArgs e)
        {
            CArrow1.Angle += 5;
        }

        private void CArrowIsActiveOnClick(object? sender, RoutedEventArgs e)
        {
            if (CArrow1.TagDataMainState == null)
            {
                CArrow1.TagDataMainState = new TagDataItem(null)
                {
                    TagValueString = "0"
                };
            }
            else
            {
                CArrow1.TagDataMainState = null;
            }
        }

        private void EventGroupIdOnClick(object? sender, RoutedEventArgs e)
        {
            CAlarmIndicator1.EventGroupID = (CAlarmIndicator1.EventGroupID+1) % 5;
        }

        private void AngleOnClick(object? sender, RoutedEventArgs e)
        {
            CAlarmIndicator1.Angle += 5;
        }

        private void IsActiveOnClick(object? sender, RoutedEventArgs e)
        {
            CAlarmIndicator1.IsActive = !CAlarmIndicator1.IsActive;
        }

        private void ChangeIsReceiptOnClick(object? sender, RoutedEventArgs e)
        {
            if (CAlarmIndicator1.IsReceipt.HasValue)
            {
                if (CAlarmIndicator1.IsReceipt.Value)
                {
                    CAlarmIndicator1.IsReceipt = false;
                }
                else
                {
                    CAlarmIndicator1.IsReceipt = null;
                }
            }
            else
            {
                CAlarmIndicator1.IsReceipt = true;
            }
        }
    }
}