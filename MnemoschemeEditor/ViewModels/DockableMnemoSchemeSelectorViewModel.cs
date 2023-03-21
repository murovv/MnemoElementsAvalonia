using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using CommunityToolkit.Mvvm.Input;
using Dock.Model.Mvvm.Controls;
using IProjectModel.Structure;
using MnemoschemeEditor.Models;
using MnemoschemeEditor.Models.StructureElementsSamples;
using MnemoschemeEditor.Views;

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
        
        string filename = @$"{Directory.GetCurrentDirectory().ToString()}/jsons/{Guid.NewGuid()}.json";
        using (FileStream fs = File.Create(filename))
        {
            byte[] info = new UTF8Encoding(true).GetBytes("[{},[]]");
            // Add some information to the file.
            fs.Write(info, 0, info.Length);
        }
        var newMnemo = new StructureMnemoSchemeSample(new MnemoschemeAccessor(filename))
        {
            MnemoSсhemeName = "Новая мнемосхема"
        };
        /*if (SelectedNode.GetType().IsAssignableTo(typeof(IStructureSubstationNode)))
        {
            ((StructureSubstationNodeSample)SelectedNode).StructureMnemoSchemes.Add(newMnemo);
        }*/

         /*else if(SelectedNode.GetType().IsAssignableTo(typeof(IStructureMnemoScheme)))
        {
            ((StructureSubstationNodeSample)((IStructureMnemoScheme)SelectedNode).StructureSubstationNode).StructureMnemoSchemes = ((StructureSubstationNodeSample)((IStructureMnemoScheme)SelectedNode).StructureSubstationNode).StructureMnemoSchemes.Append(newMnemo);
        }*/
    }

    public void OnStructureMnemoSchemeDoubleClicked(object? sender, TappedEventArgs tappedEventArgs)
    {
        var mnemoscheme = ((sender as StackPanel).DataContext as StructureMnemoSchemeSample);
        (((IClassicDesktopStyleApplicationLifetime)Avalonia.Application.Current.ApplicationLifetime)
            .MainWindow.DataContext as MainWindowViewModel).CurrentMnemo = mnemoscheme.GetMnemo();

    }
}
