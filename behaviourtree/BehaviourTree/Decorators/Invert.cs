using Godot;
using Godot.Collections;
namespace BehaviourTree.Decorators;
[Tool]
[Icon("res://addons/behaviourtree/Icons/decorators/Invert.svg")]
public partial class Invert : Decorator
{
    Task _child;
    public override void _Ready()
    {
        base._Ready();
        if (!Engine.IsEditorHint()) _child = GetChild<Task>(0);
    }
    public override void Run(Dictionary blackboard)
    {
        _child.Run(blackboard);
        SetRunning();
    }
    public override void ChildFail()
    {
        base.ChildFail();
        SetSuccess();
    }
    public override void ChildSuccess()
    {
        base.ChildSuccess();
        SetFail();
    }
}
