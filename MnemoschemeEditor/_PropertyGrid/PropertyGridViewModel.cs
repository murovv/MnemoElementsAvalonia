using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Avalonia;
using Avalonia.Controls;
using AvAp2.ViewModels;
using ReactiveUI;

namespace MnemoschemeEditor._PropertyGrid
{
    public class PropertyGridViewModel : ViewModelBase
    {
        private List<ConfigurablePropertyMetadata> _propertyMetadata;
        private List<Control> _selectedObjects;
        private IPropertyContractResolver? _propertyContractResolver;

        public List<Control> SelectedObjects
        {
            get => _selectedObjects;
            set
            {
                if (_selectedObjects != value)
                {
                    _selectedObjects = value;
                    this.RaisePropertyChanged(nameof(this.SelectedObjects));

                    this.UpdatePropertyCollection();
                }
            }
        }
        

        public List<ConfigurablePropertyMetadata> PropertyMetadata
        {
            get => _propertyMetadata;
            private set
            {
                if (_propertyMetadata != value)
                {
                    _propertyMetadata = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public PropertyGridViewModel()
        {
            _propertyMetadata = new List<ConfigurablePropertyMetadata>(0);
        }

        public void SetPropertyContractResolver(IPropertyContractResolver? dataAnnotator)
        {
            _propertyContractResolver = dataAnnotator;
        }
        public static Type GetCommonBaseClass (List<Type> types)
        {
            if (types.Count == 0)
                return typeof(object);

            Type ret = types[0];

            for (int i = 1; i < types.Count; ++i)
            {
                if (types[i].IsAssignableFrom(ret))
                    ret = types[i];
                else
                {
                    // This will always terminate when ret == typeof(object)
                    while (!ret.IsAssignableFrom(types[i]))
                        ret = ret.BaseType;
                }
            }

            return ret;
        }

        private void UpdatePropertyCollection()
        {
            var newPropertyMetadata = new List<ConfigurablePropertyMetadata>();

            var selectedObjects = this.SelectedObjects;
            if (!selectedObjects.Any())
            {
                this.PropertyMetadata = newPropertyMetadata;
                return;
            }

            // Get properties for PropertyGrid
            var closestCommonType = GetCommonBaseClass(selectedObjects.Select(x=>x.GetType()).ToList());
            var properties = TypeDescriptor.GetProperties(closestCommonType);
            var avaloniaProperties = AvaloniaPropertyRegistry.Instance.GetRegistered(closestCommonType).Where(x=>!x.IsAttached && !x.IsDirect);

            // Create a viewmodel for each property
            foreach (var actProperty in avaloniaProperties)
            {
                if (actProperty == null){ continue; }
                if (!properties.Find(actProperty.Name, true).IsBrowsable){ continue; }

                var propMetadata = new ConfigurablePropertyMetadata(properties.Find(actProperty.Name, true), selectedObjects, _propertyContractResolver);
                if(propMetadata.ValueType == PropertyValueType.Unsupported){ continue; }

                newPropertyMetadata.Add(propMetadata);
            }

            this.PropertyMetadata = newPropertyMetadata;
        }
    }
}