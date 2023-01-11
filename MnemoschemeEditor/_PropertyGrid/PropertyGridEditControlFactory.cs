using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using DynamicData;
using ReactiveUI;

namespace MnemoschemeEditor._PropertyGrid
{
    public class PropertyGridEditControlFactory
    {
        private List<IDisposable> bindings = new List<IDisposable>();
        public virtual Control? CreateControl(
            ConfigurablePropertyMetadata property, 
            IEnumerable<ConfigurablePropertyMetadata> allProperties)
        {
            Control? ctrlValueEdit = null;
            switch (property.ValueType)
            {
                case PropertyValueType.Bool:
                    ctrlValueEdit = this.CreateCheckBoxControl(property, allProperties);
                    break;

                case PropertyValueType.String:
                    ctrlValueEdit = this.CreateTextBoxControl(property, allProperties);
                    break;

                case PropertyValueType.Enum:
                    ctrlValueEdit = this.CreateEnumControl(property, allProperties);
                    break;

                case PropertyValueType.DetailSettings:
                    break;

                default:
                    throw new ArgumentOutOfRangeException($"Unsupported value {property.ValueType}");
            }

            return ctrlValueEdit;
        }

        protected virtual Control CreateCheckBoxControl(
            ConfigurablePropertyMetadata property, 
            IEnumerable<ConfigurablePropertyMetadata> allProperties)
        {
            var ctrlCheckBox = new CheckBox();
            var vals = property.HostObject.Select(x => (bool)property.Descriptor.GetValue(x)).ToList();
            bool allEqual = vals.All(x=>x.Equals(vals[0]));
            if (allEqual)
            {
                ctrlCheckBox.IsChecked = vals[0];
                foreach (var prop in property.HostObject)
                {
                    var bind = ctrlCheckBox.Bind(ToggleButton.IsCheckedProperty,
                        new Binding(property.Descriptor.Name, BindingMode.OneWayToSource) { Source = prop });
                    bindings.Add(bind);
                }
            }
            else
            {
                ctrlCheckBox.IsChecked = null;
                EventHandler<RoutedEventArgs> handler = null;
                handler = (sender, args) =>
                {
                    foreach (var prop in property.HostObject)
                    {
                        var bind = ctrlCheckBox.Bind(CheckBox.IsCheckedProperty,
                            new Binding(property.Descriptor.Name, BindingMode.OneWayToSource) { Source = prop });
                        bindings.Add(bind);
                    }
                    ctrlCheckBox.Checked -= handler;
                };
                ctrlCheckBox.Checked += handler; 
            }

            ctrlCheckBox.HorizontalAlignment = HorizontalAlignment.Left;
            ctrlCheckBox.IsEnabled = !property.IsReadOnly;
            return ctrlCheckBox;
        }

        protected virtual Control CreateTextBoxControl(
            ConfigurablePropertyMetadata property,
            IEnumerable<ConfigurablePropertyMetadata> allProperties)
        {
            var ctrlTextBox = new TextBox();
            var vals = property.HostObject.Select(x => property.Descriptor.GetValue(x)).ToList();
            bool allEqual = vals.All(x=>x==vals[0]);
            if (allEqual)
            {
                ctrlTextBox.Text = vals[0]?.ToString() ?? "";
                foreach (var prop in property.HostObject)
                {
                    var bind = ctrlTextBox.Bind(TextBox.TextProperty,
                        new Binding(property.Descriptor.Name, BindingMode.OneWayToSource) { Source = prop });
                    bindings.Add(bind);
                }
            }
            else
            {
                EventHandler<TextChangedEventArgs> handler = null;
                handler = (sender, args) =>
                {
                    foreach (var prop in property.HostObject)
                    {
                        var bind = ctrlTextBox.Bind(TextBox.TextProperty,
                            new Binding(property.Descriptor.Name, BindingMode.OneWayToSource) { Source = prop });
                        bindings.Add(bind);
                    }
                    ctrlTextBox.TextChanged -= handler;
                };
                ctrlTextBox.TextChanged += handler;
            }

            ctrlTextBox.Width = double.NaN;
            ctrlTextBox.IsReadOnly = property.IsReadOnly;
            return ctrlTextBox;
        }

        protected virtual Control CreateEnumControl(
            ConfigurablePropertyMetadata property,
            IEnumerable<ConfigurablePropertyMetadata> allProperties)
        {
            var ctrlComboBox = new ComboBox();
            ctrlComboBox.Items = property.GetEnumMembers();
            var vals = property.HostObject.Select(x => (Enum)property.Descriptor.GetValue(x)).ToList();
            bool allEqual = vals.All(x=>x.Equals(vals[0]));
            if (allEqual)
            {
                ctrlComboBox.SelectedItem = vals[0];
                foreach (var prop in property.HostObject)
                {
                    var bind = ctrlComboBox.Bind(ComboBox.SelectedItemProperty,
                        new Binding(property.Descriptor.Name, BindingMode.OneWayToSource) { Source = prop });

                    bindings.Add(bind);
                }
            }
            else
            {
                
                EventHandler<SelectionChangedEventArgs> handler = null;
                handler = (sender, args) =>
                {
                    foreach (var prop in property.HostObject)
                    {
                        var bind = ctrlComboBox.Bind(ComboBox.SelectedItemProperty,
                            new Binding(property.Descriptor.Name, BindingMode.OneWayToSource) { Source = prop });
                        bindings.Add(bind);
                    }
                    ctrlComboBox.SelectionChanged -= handler;
                };
                ctrlComboBox.SelectionChanged += handler;
            }

            ctrlComboBox.Width = double.NaN;
            ctrlComboBox.IsEnabled = !property.IsReadOnly;
            return ctrlComboBox;
        }

        public void Reset()
        {
            foreach (var binding in bindings)
            {
                binding.Dispose();
            }
        }
    }
}
