using System.Reactive;
using ReactiveUI;

namespace MnemoschemeEditor.ViewModels;

public class VideoSettingsViewModel :ViewModelBase
{
    private string _videoLogin;
    private string _videoPassword;
    private string _videoChannelPTZ;

    public string VideoLogin
    {
        get => _videoLogin;
        set => this.RaiseAndSetIfChanged(ref _videoLogin, value);
    }
    
    public string VideoPassword
    {
        get => _videoPassword;
        set => this.RaiseAndSetIfChanged(ref _videoPassword, value);
    }
    
    public string VideoChannelPTZ
    {
        get => _videoChannelPTZ;
        set => this.RaiseAndSetIfChanged(ref _videoChannelPTZ, value);
    }
    
    public ReactiveCommand<Unit, VideoSettingsViewModel> SubmitSettingsCommand { get; }

    public VideoSettingsViewModel()
    {
        SubmitSettingsCommand = ReactiveCommand.Create(() =>
        {
            return this;
        });
    }
}