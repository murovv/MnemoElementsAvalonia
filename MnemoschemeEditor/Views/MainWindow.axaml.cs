
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
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
            var subclassTypes = new[]
            {
                typeof(CCurrentTransformer), typeof(CArrow), typeof(CArrowReserve), typeof(CDischarger),
                typeof(CFilterOfConnection), typeof(CFuse), typeof(CResistor), typeof(CHighFrequencyLineTrap),
                typeof(CLineCross), typeof(CLinkCapacitor), typeof(CPEConnector), typeof(CReactor),
                typeof(CSurgeSuppressor)
            };
            mnemoElementSelector.Items = subclassTypes;
            

            var transformerSelector = this.Find<ComboBox>("TransformerSelector");
            var transformers = new[]
            {
                typeof(CTransformerNPE), typeof(CTransformerCoil), typeof(CTransformer2Coils),
                typeof(CTransformer3CoilsV1), typeof(CTransformer3CoilsV1Left), typeof(CTransformer3CoilsV2),
                typeof(CTransformer4Coils)
            };
            transformerSelector.Items = transformers;
            transformerSelector.SelectionChanged += (sender, args) =>
            {
                if (sender is ComboBox c && c.SelectedItem!=null)
                {
                    mnemoElementSelector.SelectedItem = null;
                }
                
            };
            mnemoElementSelector.SelectionChanged += (sender, args) =>
            {
                if (sender is ComboBox c && c.SelectedItem != null)
                {
                    transformerSelector.SelectedItem = null;
                }
            };
            
            
            var voltageSelector = this.Find<ComboBox>("VoltageSelector");
            var voltages = Enum.GetValues(typeof(VoltageClasses)).Cast<VoltageClasses>();
            voltageSelector.Items = voltages;
            voltageSelector.SelectedIndex = 0;
        }

        private void CPointLinkPortButton_OnClick(object? sender, RoutedEventArgs e)
        {
            this.Find<ComboBox>("MnemoElementSelector").SelectedItem = null;
            this.Find<ComboBox>("TransformerSelector").SelectedItem = null;
            (DataContext as MainWindowViewModel).SelectedMnemoElement = typeof(CPointLinkPort);
        }


        private void CPointLinkButton_OnClick(object? sender, RoutedEventArgs e)
        {
            this.Find<ComboBox>("MnemoElementSelector").SelectedItem = null;
            this.Find<ComboBox>("TransformerSelector").SelectedItem = null;
            (DataContext as MainWindowViewModel).SelectedMnemoElement = typeof(CPointLink);
        }

        private void CPointOnLineButton_OnClick(object? sender, RoutedEventArgs e)
        {
            this.Find<ComboBox>("MnemoElementSelector").SelectedItem = null;
            this.Find<ComboBox>("TransformerSelector").SelectedItem = null;
            (DataContext as MainWindowViewModel).SelectedMnemoElement = typeof(CPointOnLine);
        }

        private void PointerButton_OnClick(object? sender, RoutedEventArgs e)
        {
            this.Find<ComboBox>("MnemoElementSelector").SelectedItem = null;
            this.Find<ComboBox>("TransformerSelector").SelectedItem = null;
            (DataContext as MainWindowViewModel).SelectedMnemoElement = null;
        }

        private void CHyperLinkButton_OnClick(object? sender, RoutedEventArgs e)
        {
            this.Find<ComboBox>("MnemoElementSelector").SelectedItem = null;
            this.Find<ComboBox>("TransformerSelector").SelectedItem = null;
            (DataContext as MainWindowViewModel).SelectedMnemoElement = typeof(CHyperLink);
        }

        private void CWebCameraButton_OnClick(object? sender, RoutedEventArgs e)
        {
            this.Find<ComboBox>("MnemoElementSelector").SelectedItem = null;
            this.Find<ComboBox>("TransformerSelector").SelectedItem = null;
            (DataContext as MainWindowViewModel).SelectedMnemoElement = typeof(CWebCamera);
        }

        private void CTextButton_OnClick(object? sender, RoutedEventArgs e)
        {
            this.Find<ComboBox>("MnemoElementSelector").SelectedItem = null;
            this.Find<ComboBox>("TransformerSelector").SelectedItem = null;
            (DataContext as MainWindowViewModel).SelectedMnemoElement = typeof(CText);
        }

        private void CLineButton_OnClick(object? sender, RoutedEventArgs e)
        {
            this.Find<ComboBox>("MnemoElementSelector").SelectedItem = null;
            this.Find<ComboBox>("TransformerSelector").SelectedItem = null;
            (DataContext as MainWindowViewModel).SelectedMnemoElement = typeof(CLine);
        }

        private void Button_OnClick(object? sender, RoutedEventArgs e)
        {
        }
    }
}