using Avalonia;
using Avalonia.Media;
using AvAp2.Models;

namespace AvAp2.Interfaces
{
    public interface IBasicCommutationDevice
    {
        // string TagIDKAState { get; }
        // TagDataItem TagDataKAState { get; }
      
        CommutationDeviceStates NormalState { get; }
        bool ShowNormalState { get; }

        string TagIDControlMode { get; }
        TagDataItem TagDataControlMode { get; }
       
        string ControlModeTextDistance { get; }
        string ControlModeTextLocal { get; }
        string ControlModeToolTip { get; }
        Thickness MarginControlMode { get; }
        Color ControlModeTextColor { get; }
        
        string TagIDBlockState { get; }
        TagDataItem TagDataBlock { get; }
        Thickness MarginBlock { get; }

        string TagIDRealBlockState { get; }
        TagDataItem TagDataRealBlock { get; }

        string TagIDDeblock { get; }
        TagDataItem TagDataDeblock { get; }
        Thickness MarginDeblock { get; }

        string TagIDBanners { get; }
        TagDataItem TagDataBanners { get; }
        Thickness MarginBanner { get; }

        int CommonKAID { get; }
        bool CommutationDeviceStateManualSetEnabled { get; }

        string TagIDCommandOn { get; }
        string TagIDCommandOff { get; }
        string TagIDCommandClearReady { get; }
        string TagIDCommandReady { get; }


    }
}