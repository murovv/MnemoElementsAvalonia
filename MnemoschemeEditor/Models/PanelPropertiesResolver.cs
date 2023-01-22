using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Avalonia.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using JsonProperty = Newtonsoft.Json.Serialization.JsonProperty;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace MnemoschemeEditor.jsons;

public class PanelPropertiesResolver : DefaultContractResolver
{
    private readonly HashSet<string> ignoreProps;
    public PanelPropertiesResolver(IEnumerable<string> propNamesToIgnore)
    {
        this.ignoreProps = new HashSet<string>(propNamesToIgnore);
    }

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        JsonProperty property = base.CreateProperty(member, memberSerialization);
        if (this.ignoreProps.Contains(property.PropertyName))
        {
            property.ShouldSerialize = _ => false;
        }
        if (property.PropertyName == "Children")
        {
            property.Writable = true;
            property.ValueProvider = new CollectionProvider(member as PropertyInfo);
        }
        
        if (!property.Writable)
        {
            property.ShouldSerialize = _ => false;
        }
        return property;
    }
}

public class CollectionProvider:IValueProvider
{
    
    private PropertyInfo _targetProperty;
    public CollectionProvider(PropertyInfo targetProperty)
    {
        _targetProperty = targetProperty;
    }
    public void SetValue(object target, object? value)
    {
        var targetProp = _targetProperty.GetValue(target);
        if (targetProp is Controls listTarget && value is Controls listValue)
        {
            listTarget.Clear();
            listTarget.AddRange(listValue);
        }
    }
    public object? GetValue(object target)
    {
        object value = _targetProperty.GetValue(target);
        return value;
    }
}

public class ControlsConverter : JsonConverter<Controls>{
    public override void WriteJson(JsonWriter writer, Controls? value, JsonSerializer serializer)
    {
        writer.WriteValue(value);
    }
    
    public override Controls? ReadJson(JsonReader reader, Type objectType, Controls? existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        JObject obj = JObject.Load(reader);
        JArray arr = JArray.Parse(obj["$values"].ToString());
        Controls controls = new Controls();
        foreach (var jToken in arr)
        {
            using (JsonTextReader textReader = new JsonTextReader(new StringReader(jToken.ToString())))
            {
                var control = serializer.Deserialize(textReader, Type.GetType(jToken["$type"].ToString()));
                controls.Add((IControl)control);
            }
        }
        return controls;
    }
}
