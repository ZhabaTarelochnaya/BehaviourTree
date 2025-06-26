using Godot;
using Godot.Collections;
namespace BehaviourTree.Decorators;
[Tool]
[Icon("res://addons/behaviourtree/Icons/decorators/AlwaysFail.svg")]
public partial class AlwaysFail : Decorator
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
        SetFail();
    }
    public override void ChildSuccess()
    {
        base.ChildFail();
        SetFail();
    }
}
