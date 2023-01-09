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
            foreach (var prop in property.HostObject)
            {
                var bind = ctrlCheckBox.Bind(ToggleButton.IsCheckedProperty, new Binding(property.Descriptor.Name, BindingMode.TwoWay){Source = prop});
                bindings.Add(bind);
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
            var t = property.GetValue();
            foreach (var prop in property.HostObject)
            {
                var bind = ctrlTextBox.Bind(TextBox.TextProperty, new Binding(property.Descriptor.Name, BindingMode.TwoWay) { Source = prop });
                bindings.Add(bind);
            }

            ctrlTextBox.Width = double.NaN;
            ctrlTextBox.IsReadOnly = property.IsReadOnly;

            /*if (null != property.GetCustomAttribute<LinkAttribute>())
            {
                ctrlTextBox.Classes.Add("Link");
                ctrlTextBox.IsReadOnly = true;
                ctrlTextBox.Cursor = new Cursor(StandardCursorType.Hand);
                ctrlTextBox.PointerReleased += (sender, args) =>
                {
                    CommonUtil.OpenUrlInBrowser(ctrlTextBox.Text);
                };
            }*/

            return ctrlTextBox;
        }

        protected virtual Control CreateEnumControl(
            ConfigurablePropertyMetadata property,
            IEnumerable<ConfigurablePropertyMetadata> allProperties)
        {
            var ctrlComboBox = new ComboBox();
            ctrlComboBox.Items = property.GetEnumMembers();
            foreach (var prop in property.GetValue())
            {
                var bind = ctrlComboBox.Bind(SelectingItemsControl.SelectedItemProperty, new Binding(property.Descriptor.Name, BindingMode.TwoWay) { Source = prop });
                bindings.Add(bind);
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
