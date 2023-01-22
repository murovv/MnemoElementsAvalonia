using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dock.Model.Mvvm.Controls;
using MnemoschemeEditor.Models;

namespace MnemoschemeEditor.ViewModels;

public class DockableMnemoSchemeSelectorViewModel:Tool
{
    public List<IMnemoscheme> Files { get; }

    public DockableMnemoSchemeSelectorViewModel()
    {
        Files = new List<IMnemoscheme>(Directory.GetFiles(@"C:\Users\murov\RiderProjects\AvaloniaApplication1\MnemoschemeEditor\jsons")
            .Select(x => new MnemoschemeFile(x)).ToList());
    }
}
