using Godot;
using System.Collections.Generic;
namespace BehaviourTree.Leafs;
[Tool]
[Icon("res://addons/behaviourtree/Icons/leafs/Leaf.svg")]
public abstract partial class Leaf : Task
{
    public override string[] _GetConfigurationWarnings()
    {
        List<string> warnings = new List<string>();
        if(GetChildren().Count > 0)
        {
            warnings.Add("Leaf can't have children");
        }
        return warnings.ToArray();
    }
}
