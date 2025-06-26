using Godot;
using Godot.Collections;
namespace BehaviourTree.Leafs;
[Tool]
[Icon("res://addons/behaviourtree/Icons/leafs/Wait.svg")]
public partial class Duration : Leaf
{
    double _timer;
    bool _isRunning;
    bool _timeOut;
    [Export] float _duraton;
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (!Engine.IsEditorHint() && _isRunning)
        {
            _timer += delta;
            if (_timer > _duraton)
            {
                _timer = 0;
                _timeOut = true;
            }
        }
    }
    public override void Run(Dictionary blackboard)
    {
        if (!_isRunning) _isRunning = true;
        if (!_timeOut)
        {
            SetFail();
        }
        else
        {
            SetSuccess();
            _isRunning = false;
            _timeOut = false;
        }
    }
}
