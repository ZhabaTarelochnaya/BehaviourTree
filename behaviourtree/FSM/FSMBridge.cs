using BehaviourTree.Composites;
using FSM;
using Godot;
using Godot.Collections;
using System.Collections.Generic;
namespace BehaviourTree.Decorators;
[Tool]
[Icon("res://addons/behaviourtree/Icons/FSM/Bridge.svg")]
public partial class FSMBridge : Decorator
{
    IActivatable _fsm;
    public override void _Ready()
    {
        base._Ready();
        if(!Engine.IsEditorHint())
        {
            _fsm = GetChild<IActivatable>(0);
            _fsm.IsActive = false;
        }
    }
    public override void Run(Dictionary blackboard)
    {
        if (_fsm.ResetOnActivation) _fsm.Reset();
        if (!_fsm.IsActive) _fsm.Activate();
    }
    public override void ChildSuccess()
    {
        base.ChildSuccess();
        if (_fsm.IsActive) _fsm.Deactivate();
        SetSuccess();
    }
    public override string[] _GetConfigurationWarnings()
    {
        List<string> warnings = new List<string>();
        Node parent = GetParent();
        Array<Node> children = GetChildren();
        if (!(parent is Composite) && !(parent is Decorator))
        {
            warnings.Add("FSMBridge must be a child of Composite or Decorator");
        }
        if (children.Count == 0)
        {
            warnings.Add("FSMBridge must have a child");
        }
        else if (children.Count > 1)
        {
            warnings.Add("FSMBridge must have only one child");
        }
        return warnings.ToArray();
    }
}
