using Godot;
using Godot.Collections;
namespace BehaviourTree.Decorators;
[Tool]
[Icon("res://addons/behaviourtree/Icons/decorators/AlwaysSucceed.svg")]
public partial class AlwaysSucceed : Decorator
{
    Task _child;
    public override void _Ready()
    {
        base._Ready();
        if (!Engine.IsEditorHint()) _child = GetChild<Task>(0);
    }
    public override void Run(Dictionary blackboard)
    {
        if (GetChildCount() > 0) _child.Run(blackboard);
    }
    public override void ChildFail()
    {
        base.ChildFail();
        SetSuccess();
    }
    public override void ChildSuccess()
    {
        base.ChildFail();
        SetSuccess();
    }
}
