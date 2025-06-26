using Godot;
using System;
using System.Collections.Generic;
namespace FSM;
[Icon("res://addons/behaviourtree/Icons/FSM/State.svg")]
public abstract partial class State : Node
{
    List<IActivatable> _activatables = new();
    public StateMachine Fsm { get; set; }
    [Export] public bool ResetChild { get; set; } = true;
    public event Action Entering;
    public event Action Exiting;
    public event Action Processing;
    public event Action PhysicsProcessing;
    public override void _Ready()
    {
        base._Ready();
        if (!Engine.IsEditorHint())
        {
            foreach (Node child in GetChildren())
            {
                IActivatable activatable = child as IActivatable;
                if (child != null)
                {
                    activatable.IsActive = false;
                    _activatables.Add(activatable);
                }
            }
        }
    }
    public virtual void Enter() 
    {
        #if DEBUG
        if (Fsm.IsDebugEnabled) GD.Print($"Entering {Name}");
        #endif
        Entering?.Invoke();
        foreach (IActivatable activatable in _activatables)
        {
            activatable.Activate();
        }
    }
    public virtual void Exit() 
    {
        #if DEBUG
        if (Fsm.IsDebugEnabled) GD.Print($"Exiting {Name}");
        #endif
        Exiting?.Invoke();
        foreach (IActivatable activatable in _activatables)
        {
            activatable.Deactivate();
        }
    }
    public virtual void Process(float delta) 
    {
        Processing?.Invoke();
    }
    public virtual void PhysicsProcess(float delta) 
    {
        PhysicsProcessing?.Invoke();
    }
    public abstract void CheckTransitions();
}