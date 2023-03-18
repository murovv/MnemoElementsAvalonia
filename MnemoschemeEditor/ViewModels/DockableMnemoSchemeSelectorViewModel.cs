using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using CommunityToolkit.Mvvm.Input;
using Dock.Model.Mvvm.Controls;
using IProjectModel.Structure;
using MnemoschemeEditor.Models;
using MnemoschemeEditor.Models.StructureElementsSamples;

namespace MnemoschemeEditor.ViewModels;

public class DockableMnemoSchemeSelectorViewModel:Tool
{
    public List<StructureSubstationNodeSample> Nodes { get; set; }
    public object SelectedNode { get; set; }
    
    public DockableMnemoSchemeSelectorViewModel(List<StructureSubstationNodeSample> nodes)
    {
        Nodes = nodes;
    }

    public void OnSelectedNodeChanged(object sender, SelectionChangedEventArgs args)
    {
        
    }

    public void AddMnemo()
    {
        /*if (SelectedNode.GetType() == typeof(IStructureSubstationNode))
        {
            ((IStructureSubstationNode)SelectedNode).StructureMnemoSchemes.
        }*/
    }

    public void OnStructureMnemoSchemeDoubleClicked(object? sender, TappedEventArgs tappedEventArgs)
    {
        var mnemoscheme = ((sender as StackPanel).DataContext as StructureMnemoSchemeSample);
        (((IClassicDesktopStyleApplicationLifetime)Avalonia.Application.Current.ApplicationLifetime)
            .MainWindow.DataContext as MainWindowViewModel).CurrentMnemo = mnemoscheme.GetMnemo();

    }
}
