using Godot;
using Godot.Collections;
namespace BehaviourTree.Decorators;
[Tool]
[Icon("res://addons/behaviourtree/Icons/decorators/PRINTRESULT.svg")]
public partial class PrintResult : Decorator
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
        SetRunning();
    }
    public override void ChildFail()
    {
        PrintInfo("Fail");
        SetFail();
    }
    public override void ChildSuccess()
    {
        PrintInfo("Success");
        SetSuccess();
    }
    void PrintInfo(string result) => GD.Print(_child.Name + ": " + result);
}
