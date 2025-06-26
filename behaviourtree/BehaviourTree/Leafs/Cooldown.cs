using Godot;
using Godot.Collections;
namespace BehaviourTree.Leafs;
[Tool]
[Icon("res://addons/behaviourtree/Icons/leafs/cooldown.svg")]
public partial class Cooldown : Leaf
{
    [Export] float _seconds = 1;
    [Export] bool _isReadyAtStart = true;
    double _timer;
    public override void _Ready()
    {
        base._Ready();
        if (!Engine.IsEditorHint() && !_isReadyAtStart) _timer = _seconds;
    }
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (!Engine.IsEditorHint() && _timer > 0) _timer -= delta;
    }
    public override void Run(Dictionary blackboard)
    {
        if (_timer > 0) SetFail();
        else
        {
            _timer = _seconds;
            SetSuccess();
        }
    }
}
