   using System;
   using System.Collections.Generic;
   using System.Collections.ObjectModel;
   using System.Reactive.Linq;
   using Avalonia;
   using Avalonia.Controls;
   using Avalonia.Media;
   using Avalonia.Media.Imaging;
   using AvAp2.Converters;
using AvAp2.Models.BaseClasses;
using Dock.Model.Core;
using IProjectModel;
using ReactiveUI;

namespace MnemoschemeEditor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private IFactory _factory;
        private IDock _layout;
        private string _currentView;

        public IFactory Factory
        {
            get => _factory;
            set => this.RaiseAndSetIfChanged(ref _factory, value);
        }

        public IDock Layout
        {
            get => _layout;
            set => this.RaiseAndSetIfChanged(ref _layout, value);
        }

        public string CurrentView
        {
            get => _currentView;
            set => this.RaiseAndSetIfChanged(ref _currentView, value);
        }

        private Type selectedItem;
        public Type SelectedMnemoElement { get=>selectedItem;
            set => selectedItem = value;
        }

        private VoltageClasses selectedVoltage;

        public VoltageClasses SelectedVoltage
        {
            get=>selectedVoltage;
            set=> this.RaiseAndSetIfChanged(ref selectedVoltage, value);
        }

        private Canvas _currentMnemo = new Canvas()
        {
            Width = 3000,
            Height = 3000,
            Background = new ImageBrush(new Bitmap(@"C:\Users\murov\RiderProjects\AvaloniaApplication1\MnemoschemeEditor\Assets\plate.png"))
            {
                Stretch = Stretch.Fill,
                DestinationRect = new RelativeRect(0,0,15,15, RelativeUnit.Absolute),
                TileMode = TileMode.FlipXY
            }
        };
        public ObservableCollection<Control> SelectedMnemoElements { get; set; } = new ObservableCollection<Control>();

        public Canvas CurrentMnemo
        {
            get => _currentMnemo;
            set => this.RaiseAndSetIfChanged(ref _currentMnemo, value);
        }
    }
}