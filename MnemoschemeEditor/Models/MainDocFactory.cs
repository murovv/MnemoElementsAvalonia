using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.PanAndZoom;
using Avalonia.Data;

using Dock.Avalonia.Controls;
using Dock.Model.Controls;
using Dock.Model.Core;
using Dock.Model.Mvvm.Controls;
using Dock.Model.ReactiveUI;
using IProjectModel.Structure;
using MnemoschemeEditor.Models.StructureElementsSamples;
using MnemoschemeEditor.ViewModels;
using MnemoschemeEditor.Views;
using ReactiveUI;
using Document = Dock.Model.ReactiveUI.Controls.Document;
using DocumentDock = Dock.Model.ReactiveUI.Controls.DocumentDock;
using ProportionalDock = Dock.Model.ReactiveUI.Controls.ProportionalDock;
using RootDock = Dock.Model.ReactiveUI.Controls.RootDock;

namespace MnemoschemeEditor.Models;

public class MainDocFactory : Factory
{
    private ProportionalDock _documentDock;
    private readonly object _context;
    private ToolDock _mnemoSelector;
    private ToolDock _canvas;
    private ToolDock _propertyGrid;
    public MainDocFactory(object context)
    {
        _context = context;
    }

    public override IRootDock CreateLayout()
    {
        var document1 = new DockableZoomBorderViewModel()
        {
            Id = "Document1",
            Title = "Document1",
            CanClose = false,
            CanFloat = false
        };
        var document2 = new DockablePropertyGridViewModel()
        {
            Id = "Document2",
            Title = "Document2"
        };
        var document3 = new DockableMnemoSchemeSelectorViewModel((List<StructureSubstationNodeSample>)_context)
        {
            Id = "Document3",
            Title = "Document3"
        };
        Tool tool = new Tool();
        
        _mnemoSelector = new ToolDock
        {
            ActiveDockable = document3,
            VisibleDockables = CreateList<IDockable>(document3),
        };
        _canvas = new ToolDock
        {
            ActiveDockable = document1,
            VisibleDockables = CreateList<IDockable>(document1)
        };
        _propertyGrid = new ToolDock
        {
            ActiveDockable = document2,
            VisibleDockables = CreateList<IDockable>(document2)
        };
        var mainDock = new ProportionalDock()
        {
            Id = "DocumentsPane",
            Title = "DocumentsPane",
            Proportion = double.NaN,
            ActiveDockable = null,
            VisibleDockables = CreateList<IDockable>
            (_mnemoSelector,
                new ProportionalDockSplitter(),
                _canvas,
                new ProportionalDockSplitter(),
                _propertyGrid
                )
        };
        
        

        var mainView = new RootDock()
        {
            Id = "Main",
            Title = "Main",
            ActiveDockable = new ToolDock
            {
                ActiveDockable = document1,
                VisibleDockables = CreateList<IDockable>(mainDock)
            },
            VisibleDockables = CreateList<IDockable>(
               mainDock)
        };

        var root = CreateRootDock();

        root.Id = "Root";
        root.Title = "Root";
        root.ActiveDockable = mainView;
        root.DefaultDockable = mainView;
        root.VisibleDockables = CreateList<IDockable>(mainView);

        _documentDock = mainDock;

        return root;
    }

    public override void InitLayout(IDockable layout)
    {
        ContextLocator = new Dictionary<string, Func<object>>
        {
            [nameof(IRootDock)] = () => _context,
            [nameof(IProportionalDock)] = () => _context,
            [nameof(IDocumentDock)] = () => _context,
            [nameof(IToolDock)] = () => _context,
            [nameof(IProportionalDockSplitter)] = () => _context,
            [nameof(IDockWindow)] = () => _context,
            [nameof(IDocument)] = () => _context,
            [nameof(ITool)] = () => _context,
            ["Document1"] = () => new Document(),
            ["Document2"] = () => new Document(),
            ["LeftPane"] = () => _context,
            ["LeftPaneTop"] = () => _context,
            ["LeftPaneTopSplitter"] = () => _context,
            ["LeftPaneBottom"] = () => _context,
            ["RightPane"] = () => _context,
            ["RightPaneTop"] = () => _context,
            ["RightPaneTopSplitter"] = () => _context,
            ["RightPaneBottom"] = () => _context,
            ["DocumentsPane"] = () => _context,
            ["MainLayout"] = () => _context,
            ["LeftSplitter"] = () => _context,
            ["RightSplitter"] = () => _context,
            ["MainLayout"] = () => _context,
            ["Main"] = () => _context,
        };

        HostWindowLocator = new Dictionary<string, Func<IHostWindow>>
        {
            [nameof(IDockWindow)] = () =>
            {
                var hostWindow = new HostWindow
                {
                    [!Window.TitleProperty] = new Binding("ActiveDockable.Title")
                };
                return hostWindow;
            }
        };

        DockableLocator = new Dictionary<string, Func<IDockable>>()
        {
            ["Canvas"] = () => _canvas,
            ["PropertyGrid"] = () => _propertyGrid,
            ["MnemoSelector"] = () => _mnemoSelector
        };

        base.InitLayout(layout);

        SetActiveDockable(_documentDock);
        SetFocusedDockable(_documentDock, _documentDock.VisibleDockables?.FirstOrDefault());
    }
}