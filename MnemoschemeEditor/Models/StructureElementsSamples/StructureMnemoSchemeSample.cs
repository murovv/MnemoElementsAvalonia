using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using IProjectModel.Structure;

namespace MnemoschemeEditor.Models.StructureElementsSamples;

public class StructureMnemoSchemeSample:IStructureMnemoScheme
{
    private MnemoschemeAccessor _accessor;
    public string MnemoSсhemeHeader { get; }
    public string MnemoSсhemeName { get; set; }
    public string MnemoSсhemeXAML { get; set; }
    public IStructureSubstationNode StructureSubstationNode { get; set; }

    public StructureMnemoSchemeSample(MnemoschemeAccessor accessor)
    {
        _accessor = accessor;
    }
    public Canvas GetMnemo()
    {
        var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
        var canvas =  _accessor.Deserialize(MnemoSсhemeXAML);
        canvas.Background = new ImageBrush(new Bitmap(assets.Open(new Uri($@"avares://MnemoschemeEditor/Assets/plate.png"))))
        {
            DestinationRect = RelativeRect.Parse("0 0 15 15"),
            Stretch = Stretch.Fill,
            TileMode = TileMode.FlipXY
        };
        return canvas;
    }
}