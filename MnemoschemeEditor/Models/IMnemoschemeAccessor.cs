using Avalonia.Controls;

namespace MnemoschemeEditor.Models;

public interface IMnemoscheme
{
    public string Name { get; }
    
    public Canvas GetMnemoscheme();
    public string GetMnemoschemeMarkup();

    public void SaveMnemoscheme(Canvas canvas);
}