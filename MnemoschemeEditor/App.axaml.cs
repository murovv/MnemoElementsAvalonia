using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Dock.Model.Core;
using Dock.Serializer;
using IProjectModel.Structure;
using MnemoschemeEditor.Models;
using MnemoschemeEditor.Models.StructureElementsSamples;
using MnemoschemeEditor.ViewModels;
using MnemoschemeEditor.Views;

namespace MnemoschemeEditor
{
    public partial class App : Application
    {
        public Window MainWindow { get; set; }
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var node = new StructureSubstationNodeSample()
            {
                SubstationNodeName = "да что это"
            };
            var anotherNode = new StructureSubstationNodeSample()
            {
                SubstationNodeName = "дочерний узел",
                StructureSubstationNodeParent = node
            };
            var schemes = new[]
            {
                new StructureMnemoSchemeSample(new MnemoschemeAccessor(@$"{Directory.GetCurrentDirectory().ToString()}/jsons/canvas.json"))
                {
                    MnemoSсhemeName = "мнемосхема моя мнемосхемма",
                    MnemoSсhemeXAML =
                        File.ReadAllText(
                            @$"{Directory.GetCurrentDirectory().ToString()}/jsons/canvas.json"),
                    StructureSubstationNode = node
                }
            };
            node.StructureMnemoSchemes = schemes;
            node.StructureSubstationNodesChildren = new[] { anotherNode };
            List<StructureSubstationNodeSample> nodes = new List<StructureSubstationNodeSample>()
            {
                node
            };
            
            var factory = new MainDocFactory(nodes);
            IDock loaded =
                new DockSerializer(typeof(ObservableCollection<>)).Load<IDock>(
                    @$"{Directory.GetCurrentDirectory().ToString()}/jsons/layout.json");
            IDock layout = loaded ?? factory.CreateLayout();

            factory.InitLayout(layout);

            var mainWindowViewModel = new MainWindowViewModel()
            {
                Factory = factory,
                Layout = layout
            };

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            {
                var mainWindow = new MainWindow()
                {
                    DataContext = mainWindowViewModel
                };

                mainWindow.Closing += (_, _) =>
                {
                    if (layout is IDock dock)
                    {
                        if (dock.Close.CanExecute(null))
                        {
                            dock.Close.Execute(null);
                        }
                    }
                };

                desktopLifetime.MainWindow = mainWindow;

                desktopLifetime.Exit += (_, _) =>
                {
                    if (layout is IDock dock)
                    {
                        if (dock.Close.CanExecute(null))
                        {
                            dock.Close.Execute(null);
                        }
                    }
                };
            }
            base.OnFrameworkInitializationCompleted();
        }
    }
}