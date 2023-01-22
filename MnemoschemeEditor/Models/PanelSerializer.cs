using System;
using Avalonia.Controls;
using MnemoschemeEditor.jsons;
using Newtonsoft.Json;

namespace MnemoschemeEditor.Models;

public class PanelSerializer:JsonConverter<Panel>
{
    public override void WriteJson(JsonWriter writer, Panel? value, JsonSerializer serializer)
    {
        var output = new Newtonsoft.Json.JsonSerializer();
        output.ContractResolver = new PanelPropertiesResolver(new[] { "Parent", "Owner", "FocusAdorner", "DataContext", "Classes", "Background", "Resources", "Template"});
        output.TypeNameHandling = TypeNameHandling.All;
        output.Serialize(writer, value);
    }

    public override Panel? ReadJson(JsonReader reader, Type objectType, Panel? existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        var output = new JsonSerializer();
        output.Converters.Add(new ControlsConverter());
        output.ContractResolver = new PanelPropertiesResolver(new[] { "Parent", "Owner", "FocusAdorner", "DataContext", "Classes", "Background", "Resources", "Template"});
        output.TypeNameHandling = TypeNameHandling.All;
        
        return output.Deserialize<Panel>(reader);
    }
}