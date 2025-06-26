using Godot;
using Godot.Collections;
namespace BehaviourTree.Decorators;
[Tool]
[Icon("res://addons/behaviourtree/Icons/decorators/Repeat.svg")]
public partial class Repeat : Decorator
{
    [Export] public int Count { get; private set; }
    int _counter;
    bool _isRepeating;
    Task _child;
    Dictionary _blackboard;
    public override void _Ready()
    {
        base._Ready();
        if (!Engine.IsEditorHint()) _child = GetChild<Task>(0);
    }
    public override void Run(Dictionary blackboard)
    {
        _blackboard = blackboard;
        _child.Run(blackboard);
        SetRunning();
    }
    public override void ChildSuccess()
    {
        base.ChildSuccess();
        CountResult();
    }
    public override void ChildFail()
    {
        base.ChildFail();
        CountResult();
    }
    public override void Start()
    {
        base.Start();
        _counter = 0;
        _isRepeating = false;
    }
    public override void Cancel()
    {
        base.Cancel();
        _counter = 0;
        _isRepeating = false;
    }
    void CountResult()
    {
        if (Count > 0)
        {
            _counter++;
            if (_counter >= Count)
            {
                _counter = 0;
                _isRepeating = false;
                SetSuccess();
            }
        }
        if (_isRepeating) _child.Run(_blackboard);
    }
}
