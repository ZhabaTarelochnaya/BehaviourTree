using BehaviourTree.Composites;
using Godot;
using Godot.Collections;
using System.Collections.Generic;
namespace BehaviourTree.Decorators;
[Tool]
[Icon("res://addons/behaviourtree/Icons/decorators/Decorator.svg")]
public abstract partial class Decorator : Task
{
    public override string[] _GetConfigurationWarnings()
    {
        List<string> warnings = new List<string>();
        Node parent = GetParent();
        Array<Node> children = GetChildren();
        if (children.Count == 0)
        {
            warnings.Add("Decorator must have a child");
        }
        else if (children.Count > 1)
        {
            warnings.Add("Decorator must have only one child");
        }
        else if (!(children[0] is Task))
        {
            warnings.Add("Decorator must have child of type Task");
        }
        return warnings.ToArray();
    }
}
