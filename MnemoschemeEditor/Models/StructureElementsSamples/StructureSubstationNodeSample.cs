using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using IProjectModel;
using IProjectModel.Events;
using IProjectModel.Structure;
using ReactiveUI;

namespace MnemoschemeEditor.Models.StructureElementsSamples;

public class StructureSubstationNodeSample:IStructureSubstationNode
{
    public object GetParent(ViewModes AMode)
    {
        throw new System.NotImplementedException();
    }

    public string SubstationNodeName { get; set; }
    public IStructureSwitchingSubstation StructureSwitchingSubstation { get; set; }
    public IStructureVoltage StructureVoltage { get; }
    public IStructureConnection StructureConnection { get; }
    public IStructureSubstationNode StructureSubstationNodeParent { get; set; }
    public IEnumerable<IStructureSubstationNode> StructureSubstationNodesChildren { get; set; }
    public IEnumerable<IDevice> Devices { get; }
    public IEnumerable<IEventTemplate> EventTemplates { get; }
    public IEnumerable<IStructureCommutationDevice> StructureCommutationDevices { get; }
    public IEnumerable<IStructureVirtualKey> StructureVirtualKeys { get; }
    public IEnumerable<IStructureMnemoScheme> StructureMnemoSchemes
    {
        get;
        set;
    }

    public List<object> Items => StructureMnemoSchemes != null
        ? StructureMnemoSchemes.Union<object>(StructureSubstationNodesChildren).ToList()
        : new List<object>();

}