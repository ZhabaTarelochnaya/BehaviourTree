using Godot;
using Godot.Collections;
namespace BehaviourTree.Decorators;
[Tool]
[Icon("res://addons/behaviourtree/Icons/decorators/UntilFail.svg")]
public partial class UntilFail : Decorator
{
    Task _child;
    public override void _Ready()
    {
        base._Ready();
        if (!Engine.IsEditorHint()) _child = GetChild<Task>(0);
    }
    public override void Run(Dictionary blackboard)
    {
        GetChild<Task>(0).Run(blackboard);
        SetRunning();
    }
    public override void ChildFail()
    {
        base.ChildFail();
        SetSuccess();
    }
}
