using Godot;
using Godot.Collections;
namespace BehaviourTree.Leafs;
[Tool]
[Icon("res://addons/behaviourtree/Icons/leafs/Wait.svg")]
public partial class Wait : Leaf
{
    double _timer;
    bool _isRunning;
    [Export] float _waitTime;
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if(!Engine.IsEditorHint() && _isRunning)
        {
            _timer += delta;
            if(_timer > _waitTime)
            {
                _timer = 0;
                _isRunning = false;
                SetSuccess();
            }
        }
    }
    public override void Run(Dictionary blackboard)
    {
        _isRunning = true;
    }
}
