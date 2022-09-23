using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using AvAp2.Models;

namespace AvAp2.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            #region OnClicks
            AutomaticSwitchButton.Click += AutomaticSwitchButton_Click;
            CellCartButton.Click += CellCartButton_Click;
            CellCart2Button.Click += CellCart2Button_Click;
            CFuseButton.Click+= CFuseButtonOnClick;
            CPEConnectorButton.Click+= CPEConnectorButtonOnClick;
            CFilterOfConnectionButton.Click+=CFilterOfConnectionButtonOnClick;
            #endregion
            #region TagData init
            CAutomaticSwitch1.TagDataMainState = new TagDataItem(null);
            CCellCart1.TagDataMainState = new TagDataItem(null);
            CCellCart2.TagDataMainState = new TagDataItem(null);
            CFuse.TagDataMainState = new TagDataItem(null);
            CPEConnector.TagDataMainState = new TagDataItem(null);
            CFilterOfConnection.TagDataMainState = new TagDataItem(null);
            #endregion
        }

        private void CFilterOfConnectionButtonOnClick(object? sender, RoutedEventArgs e)
        {
            string tagData = CFilterOfConnection.TagDataMainState.TagValueString;
            if (String.IsNullOrEmpty(tagData) || tagData == "0")
            {
                //TODO: почему нужно пересоздавать объект
                CFilterOfConnection.TagDataMainState = new TagDataItem(null);
                CFilterOfConnection.TagDataMainState.TagValueString = "1";

            }
            else
            {
                CFilterOfConnection.TagDataMainState = new TagDataItem(null);
                CFilterOfConnection.TagDataMainState.TagValueString = "0";
            }
        }

        private void CPEConnectorButtonOnClick(object? sender, RoutedEventArgs e)
        {
            CPEConnector.IsLineThin = !CPEConnector.IsLineThin;
        }

        private void CFuseButtonOnClick(object? sender, RoutedEventArgs e)
        {
            CFuse.IsConnectorExistLeft = !CFuse.IsConnectorExistLeft;
            CFuse.IsConnectorExistRight = !CFuse.IsConnectorExistRight;
        }

        private void CellCart2Button_Click(object? sender, RoutedEventArgs e)
        {
            int tag;
            if (int.TryParse(CCellCart2.TagDataMainState.TagValueString, out tag))
            {
                CCellCart2.TagDataMainState.TagValueString = ((tag+1)%4).ToString();
            }
            else
            {
                CCellCart2.TagDataMainState.TagValueString = "1";
            }
            CCellCart2.ShowNormalState = !CCellCart2.ShowNormalState;
        }

        private void CellCartButton_Click(object? sender, RoutedEventArgs e)
        {
            CCellCart1.IsConnectorExistLeft = !CCellCart1.IsConnectorExistLeft;
            int tag;
            if (int.TryParse(CCellCart1.TagDataMainState.TagValueString, out tag))
            {
                CCellCart1.TagDataMainState.TagValueString = ((tag+1)%4).ToString();
            }
            else
            {
                CCellCart1.TagDataMainState.TagValueString = "1";
            }
            CCellCart1.ShowNormalState = !CCellCart1.ShowNormalState;
        }

        private void AutomaticSwitchButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            CAutomaticSwitch1.IsConnectorExistLeft = !CAutomaticSwitch1.IsConnectorExistLeft;
            CAutomaticSwitch1.IsConnectorExistRight= !CAutomaticSwitch1.IsConnectorExistRight;
            int tag;
            if (int.TryParse(CAutomaticSwitch1.TagDataMainState.TagValueString, out tag))
            {
                CAutomaticSwitch1.TagDataMainState.TagValueString = ((tag+1)%4).ToString();
            }
            else
            {
                CAutomaticSwitch1.TagDataMainState.TagValueString = "1";
            }
            CAutomaticSwitch1.ShowNormalState = !CAutomaticSwitch1.ShowNormalState;
        }
    }
}
