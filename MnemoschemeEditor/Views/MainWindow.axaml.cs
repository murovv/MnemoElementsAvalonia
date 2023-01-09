
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.Input;
using AvAp2.Models.BaseClasses;
using AvAp2.Models.Controls;
using Dock.Avalonia.Controls;
using Dock.Model.Controls;
using IProjectModel;
using MnemoschemeEditor.ViewModels;

namespace MnemoschemeEditor.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var mnemoElementSelector = this.Find<ComboBox>("MnemoElementSelector");
            var subclassTypes = Assembly
                .GetAssembly(typeof(BasicMnemoElement))
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(BasicEquipment)));
            mnemoElementSelector.Items = subclassTypes;
            mnemoElementSelector.SelectedIndex = 0;

            var voltageSelector = this.Find<ComboBox>("VoltageSelector");
            var voltages = Enum.GetValues(typeof(VoltageClasses)).Cast<VoltageClasses>();
            voltageSelector.Items = voltages;
            voltageSelector.SelectedIndex = 0;
        }

    }
}