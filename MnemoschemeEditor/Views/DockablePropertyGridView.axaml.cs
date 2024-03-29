﻿using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using DynamicData.Binding;
using MnemoschemeEditor._PropertyGrid;
using MnemoschemeEditor.ViewModels;

namespace MnemoschemeEditor.Views;

public partial class DockablePropertyGridView : UserControl
{
    public DockablePropertyGridView()
    {
        InitializeComponent();
    }

    protected override void OnAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        var source = (((IClassicDesktopStyleApplicationLifetime)Avalonia.Application.Current.ApplicationLifetime)
            .MainWindow.DataContext as MainWindowViewModel).SelectedMnemoElements;
        var propertyGrid = this.FindControl<PropertyGrid>("propertyGrid");
        propertyGrid.SelectedObjects = source;
        base.OnAttachedToLogicalTree(e);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}