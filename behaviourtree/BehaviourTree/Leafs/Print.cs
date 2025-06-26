using Godot;
using Godot.Collections;
namespace BehaviourTree.Leafs;
[Tool]
[Icon("res://addons/behaviourtree/Icons/leafs/Print.svg")]
public partial class Print : Leaf
{
    [Export] string _msg;
    public override void Run(Dictionary blackboard)
    {
        GD.Print(_msg);
        SetSuccess();
    }
}
