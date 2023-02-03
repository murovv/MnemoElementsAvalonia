using System.Collections.ObjectModel;
using Avalonia.Controls;
using AvAp2.Models.Controls;
using MnemoschemeEditor._PropertyGrid;
using PropertyGridTest.Models;

namespace PropertyGridTest.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var propertyGrid = this.FindControl<PropertyGrid>("propertyGrid");
            propertyGrid.SelectedObjects = new ObservableCollection<object>();
            propertyGrid.SelectedObjects.Add(new TestObject());
        }
    }
}