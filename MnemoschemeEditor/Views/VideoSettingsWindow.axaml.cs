using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MnemoschemeEditor.ViewModels;
using ReactiveUI;

namespace MnemoschemeEditor.Views;

public partial class VideoSettingsWindow : ReactiveWindow<VideoSettingsViewModel>
{
    public VideoSettingsWindow()
    {
        InitializeComponent();
        this.WhenActivated(d => d(ViewModel!.SubmitSettingsCommand.Subscribe(this.Close)));
    }
    

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}