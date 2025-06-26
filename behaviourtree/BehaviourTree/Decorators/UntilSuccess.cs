using Godot;
using Godot.Collections;
namespace BehaviourTree.Decorators;
[Tool]
[Icon("res://addons/behaviourtree/Icons/decorators/UntilSuccess.svg")]
public partial class UntilSuccess : Decorator
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
    public override void ChildSuccess()
    {
        base.ChildSuccess();
        SetSuccess();
    }
}
