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
            PropertyDescriptorCollection properties;
            properties = TypeDescriptor.GetProperties(selectedObjects[0]);
            var avaloniaProperties = AvaloniaPropertyRegistry.Instance.GetRegistered(selectedObjects[0]).Where(x=>!x.IsAttached && !x.IsDirect);

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