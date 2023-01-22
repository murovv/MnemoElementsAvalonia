using Avalonia.Controls;
using Avalonia.Metadata;
using Newtonsoft.Json;

namespace MnemoschemeEditor.Models;

public class SerializableCanvas : Canvas
{
    [JsonProperty]
    public Controls SerializeChildren
    {
        get => Children;
        private set
        {
            Children.Clear();
            Children.AddRange(value);
        }
    }
}