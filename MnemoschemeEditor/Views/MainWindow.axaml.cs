using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using AvAp2.Models.BaseClasses;
using AvAp2.Models.Controls;
using Dock.Avalonia.Controls;
using Dock.Serializer;
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

        private void ToFront_OnClick(object? sender, RoutedEventArgs e)
        {
            var zIndex =
                (DataContext as MainWindowViewModel).CurrentMnemo.Children.Max(x =>
                    ((x as Panel).Children[0] as BasicMnemoElement).ZIndex) + 1;
            (DataContext as MainWindowViewModel).SelectedMnemoElements.ToList().ForEach(control =>
            {
                ((BasicMnemoElement)control).Parent.ZIndex = zIndex ;
            } );
        }

        private void ToBack_OnClick(object? sender, RoutedEventArgs e)
        {
            var zIndex =
                (DataContext as MainWindowViewModel).CurrentMnemo.Children.Min(x =>
                    ((x as Panel).Children[0] as BasicMnemoElement).ZIndex) - 1;
            (DataContext as MainWindowViewModel).SelectedMnemoElements.ToList().ForEach(control =>
            {
                ((BasicMnemoElement)control).Parent.ZIndex = zIndex ;
            } );
        }

        private void CRectangleButton_OnClick(object? sender, RoutedEventArgs e)
        {
            this.Find<ComboBox>("MnemoElementSelector").SelectedItem = null;
            this.Find<ComboBox>("TransformerSelector").SelectedItem = null;
            (DataContext as MainWindowViewModel).SelectedMnemoElement = typeof(CRectangle);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            //Удалить выбранные
            if (e.Key == Key.Delete && (this.DataContext as MainWindowViewModel).SelectedMnemoElements.Count != 0)
            {
                SubmitDelete.Open();
                EventHandler<RoutedEventArgs> okHandler = null;
                okHandler = (sender, args) =>
                {
                    (this.DataContext as MainWindowViewModel).DeleteSelected();
                    SubmitDelete.Close();
                    OkDelete.Click -= okHandler;
                };
                OkDelete.Click += okHandler;
                
                EventHandler<RoutedEventArgs> cancelHandler = null;
                cancelHandler = (sender, args) =>
                {
                    SubmitDelete.Close();
                    CancelDelete.Click -= cancelHandler;
                };
                CancelDelete.Click += cancelHandler;
            }
            //Выделить все
            else if (e.Key == Key.A && e.KeyModifiers == KeyModifiers.Control)
            {
                (this.DataContext as MainWindowViewModel).SelectAllElements();
            }
            
            base.OnKeyDown(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            //TODO hardcode
            var dock = this.FindControl<DockControl>("DockControl");
            if (dock?.Layout is { })
            {
                new DockSerializer(typeof(ObservableCollection<>)).Save(@$"{Directory.GetCurrentDirectory().ToString()}/jsons/layout.json", dock.Layout);
            }
        }
    }
}