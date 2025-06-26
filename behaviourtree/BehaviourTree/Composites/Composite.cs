using BehaviourTree.Decorators;
using Godot;
using Godot.Collections;
using System.Collections.Generic;
namespace BehaviourTree.Composites;
[Tool]
[Icon("res://addons/behaviourtree/Icons/composites/Composite.svg")]
public abstract partial class Composite : Task
{
    public override string[] _GetConfigurationWarnings()
    {
        List<string> warnings = new List<string>();
        Node parent = GetParent();
        Array<Node> children = GetChildren();
        if (children.Count < 1)
        {
            warnings.Add("Composite must have two or more children");
        }
        else
        {
            foreach (Node child in children)
            {
                if (!(child is Task))
                {
                    warnings.Add("Children must be of type Task");
                    break;
                }
            }
        }
        return warnings.ToArray();
    }
}
