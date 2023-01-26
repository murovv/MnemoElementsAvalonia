using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using FirLib.Core.Patterns.Mvvm;

namespace MnemoschemeEditor._PropertyGrid
{
    public class ConfigurablePropertyMetadata : ValidatableViewModelBase
    {
        public IEnumerable<Control> HostObject;
        private IPropertyContractResolver? _propertyContractResolver;
        public PropertyDescriptor Descriptor;

        public ObservableCollection<object?> ValueAccessor
        {
            get => new() { this.GetValue() };
            
        }

        public string CategoryName
        {
            get;
            set;
        }

        public string PropertyName
        {
            get;
            set;
        }

        public string PropertyDisplayName
        {
            get;
            set;
        }

        public PropertyValueType ValueType
        {
            get;
            set;
        }

        public bool IsReadOnly
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public Type HostObjectType => HostObject.GetType();
        
        internal ConfigurablePropertyMetadata(PropertyDescriptor propertyInfo, List<Control> hostObject, IPropertyContractResolver? propertyContractResolver)
        {
            Descriptor = propertyInfo;
            HostObject = hostObject;
            _propertyContractResolver = propertyContractResolver;
            ValueAccessor.CollectionChanged+= ValueAccessorOnCollectionChanged;
            var categoryAttrib = this.GetCustomAttribute<CategoryAttribute>();
            this.CategoryName = categoryAttrib?.Category ?? string.Empty;

            this.PropertyName = propertyInfo.Name;
            this.IsReadOnly = propertyInfo.IsReadOnly;

            this.PropertyDisplayName = propertyInfo.Name;
            var displayNameAttrib = this.GetCustomAttribute<DisplayNameAttribute>();
            if ((displayNameAttrib != null) &&
                (!string.IsNullOrEmpty(displayNameAttrib.DisplayName)))
            {
                this.PropertyDisplayName = displayNameAttrib.DisplayName;
            }

            this.Description = propertyInfo.Description ?? string.Empty;

            var propertyType = Descriptor.PropertyType;

            if (propertyType == typeof(bool))
            {
                this.ValueType = PropertyValueType.Bool;
            }
            //TODO подержка редактирования Margin
            else if (propertyType == typeof(string) || propertyType == typeof(char) || propertyType == typeof(Color) /*|| propertyType == typeof(Thickness)*/)
            {
                this.ValueType = PropertyValueType.String;
            }
            else if (propertyType == typeof(double) || propertyType == typeof(float))
            {
                this.ValueType = PropertyValueType.Float;
            }else if (propertyType == typeof(decimal) ||
                      propertyType == typeof(int) || propertyType == typeof(uint) ||
                      propertyType == typeof(byte) ||
                      propertyType == typeof(short) || propertyType == typeof(ushort) ||
                      propertyType == typeof(long) || propertyType == typeof(ulong))
            {
                this.ValueType = PropertyValueType.Integer;
            }
            else if (propertyType.IsSubclassOf(typeof(Enum)))
            {
                this.ValueType = PropertyValueType.Enum;
            }
            else if(this.GetCustomAttribute<DetailSettingsAttribute>() != null)
            {
                this.ValueType = PropertyValueType.DetailSettings;
            }
            else
            {
                this.ValueType = PropertyValueType.Unsupported;
            }

            this.ValidateCurrentValue();
        }

        private void ValueAccessorOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
           
        }

        public override string ToString()
        {
            return $"{this.CategoryName} - {this.PropertyDisplayName} (type {this.ValueType})";
        }

        public Array GetEnumMembers()
        {
            if (this.ValueType != PropertyValueType.Enum) { throw new InvalidOperationException($"Method {nameof(this.GetEnumMembers)} not supported on value type {this.ValueType}!"); }
            return Enum.GetValues(Descriptor.PropertyType);
        }

        public IEnumerable<AvaloniaProperty> GetValue()
        {
            foreach (var hostObject in HostObject)
            {
                while (!hostObject.IsInitialized)
                {
                    
                }
                yield return AvaloniaPropertyRegistry.Instance.FindRegistered(hostObject, Descriptor.Name);
            }
            yield break;
        }

        public T? GetCustomAttribute<T>()
            where T : Attribute
        {
            var resultFromDataAnnotator = _propertyContractResolver?.GetDataAnnotation<T>(Descriptor.ComponentType, Descriptor.Name);
            if (resultFromDataAnnotator != null)
            {
                return resultFromDataAnnotator;
            }

            foreach (var actAttribute in Descriptor.Attributes)
            {
                if (actAttribute is T foundAttribute)
                {
                    return foundAttribute;
                }
            }

            return null;
        }

        private void ValidateCurrentValue()
        {
            var errorsFound = false;
            var ctx = new ValidationContext(HostObject);
            ctx.DisplayName = this.PropertyDisplayName;
            ctx.MemberName = this.PropertyName;
            foreach (var actAttrib in Descriptor.Attributes)
            {
                if(!(actAttrib is ValidationAttribute actValidAttrib)){ continue; }

                var validationResult = actValidAttrib.GetValidationResult(this.ValueAccessor, ctx);
                if (validationResult != null)
                {
                    this.SetError(nameof(this.ValueAccessor), validationResult.ErrorMessage ?? "Unknown");
                    errorsFound = true;
                }
            }

            if (!errorsFound)
            {
                this.RemoveErrors(nameof(this.ValueAccessor));
            }
        }
    }
}