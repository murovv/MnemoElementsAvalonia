using Avalonia.Controls;

namespace AvAp2.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AutomaticSwitchButton.Click += ButtonChange_Click;
            CAutomaticSwitch1.TagDataMainState = new TagDataItem(null);
        }

        private void ButtonChange_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
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
