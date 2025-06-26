using Godot;
using Godot.Collections;
using System;
namespace BehaviourTree.Leafs;
[Tool]
[Icon("res://addons/behaviourtree/Icons/leafs/blackboard.svg")]
public partial class BlackboardDebug : Leaf
{
    [Export] public Godot.Collections.Array Keys { get; set; }
    public override void Run(Dictionary blackboard)
    {
        if (blackboard != null)
        {
            foreach (Variant key in Keys)
            {
                if (blackboard.ContainsKey(key)) GD.Print(blackboard[key]);
            }
        }
        SetSuccess();
    }
}
