using Avalonia.Controls;
using Avalonia.Metadata;

namespace MnemoschemeEditor.Models;

public class SerializableCanvas:Canvas
{
    public Controls Children { get; private set; } =  new Controls();
}