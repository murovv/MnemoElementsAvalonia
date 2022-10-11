using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using AvAp2.Models;
using IProjectModel;

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
            CHighFrequencyButton.Click+= CHighFrequencyButtonOnClick;
            CpeSwitchButton.Click+= CpeSwitchButtonOnClick;
            CLineButton.Click+= CLineButtonOnClick;
            CArrowButton.Click+= CArrowButtonOnClick;
            CArrowReserveButton.Click+= CArrowReserveButtonOnClick;
            CIsolatingSwitchButton.Click+= CIsolatingSwitchButtonOnClick;
            CReactorButton.Click+= CReactorButtonOnClick;
            CResistorButton.Click+= CResistorButtonOnClick;
            CSurgeSuppressorButton.Click+=CSurgeSuppressorButtonOnClick;
            CRectangleButton.Click+= CRectangleButtonOnClick;
            CCurrentDataAnalogButton.Click+= CCurrentDataAnalogButtonOnClick;
            CLineCrossButton.Click+= ButtonOnClick;
            CCurrentTransformerButton.Click+= CCurrentTransformerButtonOnClick;
            CTransformerCoilButton.Click+= CTransformerCoilButtonOnClick;
            CTransformerCoil2Button.Click+= CTransformerCoil2ButtonOnClick;
            #endregion
            #region TagData init
            CAutomaticSwitch1.TagDataMainState = new TagDataItem(null);
            CCellCart1.TagDataMainState = new TagDataItem(null);
            CCellCart2.TagDataMainState = new TagDataItem(null);
            CFuse.TagDataMainState = new TagDataItem(null);
            CPEConnector.TagDataMainState = new TagDataItem(null);
            CFilterOfConnection.TagDataMainState = new TagDataItem(null);
            CHighFrequencyLineTrap.TagDataMainState = new TagDataItem(null);
            CpeSwitch.TagDataMainState = new TagDataItem(null);
            CpeSwitch.TagDataMainState.TagValueString = "0";
            CLine.TagDataMainState = new TagDataItem(new TagDataItem(null).TagValueString="1");
            CIsolatingSwitch.TagDataMainState = new TagDataItem(new TagDataItem(null).TagValueString = "0");
            CLine.CoordinateX2 = 20;
            CLine.CoordinateY2 = 20;
            CLine.TextNameISVisible = true;
            CLine.TextName = "линия 1";
            CRectangle.TagDataMainState = new TagDataItem(new TagDataItem(null).TagValueString = "0");
            CRectangle.CoordinateX2 = 10;
            CRectangle.CoordinateY2 = 30;
            CLine.TextNameISVisible = true;
            CLine.TextName = "прямоугольничек";
            CArrow.TagDataMainState = new TagDataItem(null);
            CArrowReserve.TagDataMainState = new TagDataItem(null);
            CResistor.TagDataMainState = new TagDataItem(new TagDataItem(null).TagValueString = "0");
            CReactor.TagDataMainState = new TagDataItem(new TagDataItem(null).TagValueString = "0");
            CSurgeSuppressor.TagDataMainState = new TagDataItem(new TagDataItem(null).TagValueString = "0");
            CCurrentDataAnalog.TagDataMainState =
                new TagDataItem(new TagDataItem(null).Quality = TagValueQuality.Blocking);
            CCurrentDataAnalog.TagDataMainState.TagValueString = "1";
            CCurrentDataAnalog.TextUom = "uom";
            CCurrentDataAnalog.TextName = "name";
            CLineCross.TagDataMainState = new TagDataItem(null);
            CCurrentTransformer.TagDataMainState = new TagDataItem(new TagDataItem(null).TagValueString = "0");
            CTransformerCoil.TagDataMainState = new TagDataItem(null);
            CTransformerCoil.IsPower = true;
            CTransformer2Coils.TagDataMainState = new TagDataItem(null);
            CTransformer2Coils.IsPower = true;
            

            #endregion
        }

        private void CTransformerCoil2ButtonOnClick(object? sender, RoutedEventArgs e)
        {
            CTransformer2Coils.IsPower = !CTransformer2Coils.IsPower;
        }

        private void CTransformerCoilButtonOnClick(object? sender, RoutedEventArgs e)
        {
            CTransformerCoil.TagDataMainState.TagValueString = "1";
            CTransformerCoil.IsPower = !CTransformerCoil.IsPower;
            CTransformerCoil.VoltageEnum = (VoltageClasses)((int)(CTransformerCoil.VoltageEnum + 1)%(Enum.GetValues(typeof(VoltageClasses)).Length));

        }

        private void CCurrentTransformerButtonOnClick(object? sender, RoutedEventArgs e)
        {
            CCurrentTransformer.TagDataMainState.TagValueString = "0";
            CCurrentTransformer.VoltageEnum = (VoltageClasses)((int)(CCurrentTransformer.VoltageEnum + 1)%(Enum.GetValues(typeof(VoltageClasses)).Length));
        }

        private void ButtonOnClick(object? sender, RoutedEventArgs e)
        {
            CLineCross.LineThickness++;
        }

        private void CCurrentDataAnalogButtonOnClick(object? sender, RoutedEventArgs e)
        {
            CCurrentDataAnalog.Quality = IProjectModel.TagValueQuality.Handled;
        }

        private void CRectangleButtonOnClick(object? sender, RoutedEventArgs e)
        {
            CRectangle.ControlISSelected = !CRectangle.ControlISSelected;
        }

        private void CSurgeSuppressorButtonOnClick(object? sender, RoutedEventArgs e)
        {
            CSurgeSuppressor.TagDataMainState.TagValueString = CSurgeSuppressor.TagDataMainState.TagValueString == "0" ? "1" : "0";
        }

        private void CResistorButtonOnClick(object? sender, RoutedEventArgs e)
        {
            CResistor.IsConnectorExistLeft = !CResistor.IsConnectorExistLeft;
            CResistor.IsConnectorExistRight = !CResistor.IsConnectorExistRight;
        }

        private void CReactorButtonOnClick(object? sender, RoutedEventArgs e)
        {
            CReactor.IsRegulator = !CReactor.IsRegulator;
        }

        private void CIsolatingSwitchButtonOnClick(object? sender, RoutedEventArgs e)
        {
            int tag;
            if (int.TryParse(CIsolatingSwitch.TagDataMainState.TagValueString, out tag))
            {
                CIsolatingSwitch.TagDataMainState.TagValueString = ((tag+1)%4).ToString();
            }
            else
            {
                CIsolatingSwitch.TagDataMainState.TagValueString = "1";
            }
            CIsolatingSwitch.VoltageEnum = (VoltageClasses)((int)(CIsolatingSwitch.VoltageEnum + 1)%(Enum.GetValues(typeof(VoltageClasses)).Length));

        }

        private void CArrowReserveButtonOnClick(object? sender, RoutedEventArgs e)
        {
            CArrowReserve.TagDataMainState.TagValueString = CArrowReserve.TagDataMainState.TagValueString == "0" ? "1" : "0";
        }

        private void CArrowButtonOnClick(object? sender, RoutedEventArgs e)
        {
            CArrow.TagDataMainState.TagValueString = CArrow.TagDataMainState.TagValueString == "0" ? "1" : "0";
        }

        private void CLineButtonOnClick(object? sender, RoutedEventArgs e)
        {
            CLine.ControlISSelected = !CLine.ControlISSelected;
        }

        private void CpeSwitchButtonOnClick(object? sender, RoutedEventArgs e)
        {
            CpeSwitch.Angle += 5;
            string state = CpeSwitch.TagDataMainState.TagValueString;
            int stateInt;
            if (int.TryParse(state, out stateInt))
            {
                CpeSwitch.TagDataMainState.TagValueString = ((stateInt + 1) % 4).ToString();
            }
            else
            {
                CpeSwitch.TagDataMainState.TagValueString = "0";
            }
        }

        private void CHighFrequencyButtonOnClick(object? sender, RoutedEventArgs e)
        {
            CHighFrequencyLineTrap.Angle += 1;
        }

        private void CFilterOfConnectionButtonOnClick(object? sender, RoutedEventArgs e)
        {
            string tagData = CFilterOfConnection.TagDataMainState.TagValueString;
            if (String.IsNullOrEmpty(tagData) || tagData == "0")
            {
                CFilterOfConnection.TagDataMainState.TagValueString = "1";

            }
            else
            {
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
