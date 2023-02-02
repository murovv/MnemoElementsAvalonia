using System;
using System.Diagnostics;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform;
using AvAp2.Models;

namespace AvAp2.Views
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
            CArrowIsSelected.Click+= CArrowIsSelectedOnClick;
            CArrowReserveAngle.Click+= CArrowReserveAngleOnClick;
            CArrowReserveIsActive.Click+= CArrowReserveIsActiveOnClick;
            
            CAutomaticSwitch1.ControlISSelected = true;
            CAutomaticSwitch1.TagDataMainState = new TagDataItem(null)
            {
                TagValueString = "1"
            };
            CAutomaticSwitchState.Click+= CAutomaticSwitchStateOnClick;
            CAutomaticSwitchShowNormal.Click+= CAutomaticSwitchShowNormalOnClick;
            CAutomaticSwitchIsSelected.Click+= CAutomaticSwitchIsSelectedOnClick;
            CCellCart21.ControlISSelected = true;
            CCellCart2State.Click+= CCellCart2StateOnClick;
 
            CCellCart2LeftConnector.Click+= CCellCart2LeftConnectorOnClick;
            CCellCart2RightConnector.Click+= CCellCart2RightConnectorOnClick;
            CCellCart21.TagDataMainState = new TagDataItem(null){
                TagValueString = "1"
            };

            CCurrentDataAnalog1.TextUom = "0";
            CCurrentDataAnalog1.TagDataMainState = new TagDataItem(null)
            {
                TagValueString = "0",
                Quality = TagValueQuality.Good
            };
            CCurrentDataAnalog1.TextName = "name";
            CCurrentDataAnalogChangeName.Click+= CCurrentDataAnalogChangeNameOnClick;
            CCurrentDataAnalogChangeVoltage.Click+= CCurrentDataAnalogChangeVoltageOnClick;
            CCurrentDataAnalogChangeTagValueString.Click+= CCurrentDataAnalogChangeTagValueStringOnClick;
            CCurrentDataAnalogChangeQuality.Click+= CCurrentDataAnalogChangeQualityOnClick;
            CDiagnosticDeviceSetImageSource.Click+= CDiagnosticDeviceSetImageSourceOnClick;
            CDiagnosticDeviceIsSelected.Click+= CDiagnosticDeviceIsSelectedOnClick;
            CDiagnosticDevice1.TextName = "картинка";
        }

        private void CAutomaticSwitchIsSelectedOnClick(object? sender, RoutedEventArgs e)
        {
            CAutomaticSwitch1.ControlISSelected = !CAutomaticSwitch1.ControlISSelected;
        }

        private void CArrowIsSelectedOnClick(object? sender, RoutedEventArgs e)
        {
            CArrow1.ControlISSelected = !CArrow1.ControlISSelected;
        }
        

        private void CDiagnosticDeviceIsSelectedOnClick(object? sender, RoutedEventArgs e)
        {
            CDiagnosticDevice1.ControlISSelected = !CDiagnosticDevice1.ControlISSelected;
        }

        private void CDiagnosticDeviceSetImageSourceOnClick(object? sender, RoutedEventArgs e)
        {
            CDiagnosticDevice1.ImageFileName = CDiagnosticDeviceImageSource.Text;
        }

        private void CCurrentDataAnalogChangeQualityOnClick(object? sender, RoutedEventArgs e)
        {
            var t = Enum.GetValues(typeof(TagValueQuality)).OfType<TagValueQuality>().ToList();
            var i = t.FindIndex(x=>x == CCurrentDataAnalog1.TagDataMainState.Quality);
            CCurrentDataAnalog1.TagDataMainState.Quality = t[(i + 1) % t.Count];
        }

        private void CCurrentDataAnalogChangeTagValueStringOnClick(object? sender, RoutedEventArgs e)
        {
            CCurrentDataAnalog1.TagDataMainState.TagValueString = (int.Parse((string)CCurrentDataAnalog1.TagDataMainState.TagValueString) + 1).ToString();
        }

        private void CCurrentDataAnalogChangeVoltageOnClick(object? sender, RoutedEventArgs e)
        {
            CCurrentDataAnalog1.TextUom = (int.Parse((string)CCurrentDataAnalog1.TextUom) + 1).ToString();
        }

        private void CCurrentDataAnalogChangeNameOnClick(object? sender, RoutedEventArgs e)
        {
            if (CCurrentDataAnalog1.TextName == "name")
            {
                CCurrentDataAnalog1.TextName = "other name";
            }
            else
            {
                CCurrentDataAnalog1.TextName = "name";
            }
        }

        private void CCellCart2RightConnectorOnClick(object? sender, RoutedEventArgs e)
        {
            CCellCart21.IsConnectorExistRight = !CCellCart21.IsConnectorExistRight;
        }

        private void CCellCart2LeftConnectorOnClick(object? sender, RoutedEventArgs e)
        {
            CCellCart21.IsConnectorExistLeft = !CCellCart21.IsConnectorExistLeft;
        }
        

        private void CCellCart2StateOnClick(object? sender, RoutedEventArgs e)
        {
            switch (CCellCart21.TagDataMainState.TagValueString)
            {
                case "1":
                    CCellCart21.TagDataMainState.TagValueString = "2";
                    break;
                case "2":
                    CCellCart21.TagDataMainState.TagValueString = "3";
                    break;
                case "3":
                    CCellCart21.TagDataMainState.TagValueString = "1";
                    break;
            }
        }

        private void CAutomaticSwitchShowNormalOnClick(object? sender, RoutedEventArgs e)
        {
            CAutomaticSwitch1.ShowNormalState = !CAutomaticSwitch1.ShowNormalState;
        }

        private void CAutomaticSwitchStateOnClick(object? sender, RoutedEventArgs e)
        {
            switch (CAutomaticSwitch1.TagDataMainState.TagValueString)
            {
                case "1":
                    CAutomaticSwitch1.TagDataMainState.TagValueString = "2";
                    break;
                case "2":
                    CAutomaticSwitch1.TagDataMainState.TagValueString = "3";
                    break;
                case "3":
                    CAutomaticSwitch1.TagDataMainState.TagValueString = "1";
                    break;
            }
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