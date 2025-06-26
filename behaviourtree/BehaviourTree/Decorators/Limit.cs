using Godot;
using Godot.Collections;
namespace BehaviourTree.Decorators;
[Tool]
[Icon("res://addons/behaviourtree/Icons/decorators/Limit.svg")]
public partial class Limit : Decorator
{
    [Export] public int Count {  get; set; }
    int _counter;
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
        _counter++;
        if (_counter >= Count)
        {
            _counter = 0;
            SetFail();
        }
        else SetSuccess();
    }
    public override void ChildFail()
    {
        base.ChildFail();
        _counter = 0;
        SetFail();
    }
    public override void Start()
    {
        base.Start();
        _counter = 0;
    }
    public override void Cancel()
    {
        base.Cancel();
        _counter = 0;
    }
}
