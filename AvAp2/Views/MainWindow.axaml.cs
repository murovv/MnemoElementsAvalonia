using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AvAp2.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AutomaticSwitchButton.Click += AutomaticSwitchButton_Click;
            CellCartButton.Click += CellCartButton_Click;
            CAutomaticSwitch1.TagDataMainState = new TagDataItem(null);
            CCellCart1.TagDataMainState = new TagDataItem(null);
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
