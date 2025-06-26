using Godot;
using System.Collections.Generic;
namespace FSM;
[Tool]
[Icon("res://addons/behaviourtree/Icons/FSM/StateMachine.svg")]
public partial class StateMachine : Node, IActivatable
{
    Dictionary<string, State> _states;
    State _currentState;
    bool _disableTransition;
    [Export] public Node InitialState { get; set; }
    /// <summary>
    /// Determines state that FSM enters upon activation.
    /// </summary>
    [Export] public ActivationMode Mode { get; set; } = ActivationMode.CheckTransitions;
    [Export] public bool IsDebugEnabled { get; set; }
    public bool IsActive { get; set; } = true;
    public bool ResetOnActivation { get; set; }
    public override void _Ready()
    {
        if (!Engine.IsEditorHint())
        {
            InitStates();
            if (Mode == ActivationMode.ResetToInitial) ResetOnActivation = true;
            if (IsActive) _currentState.Enter();
        }
    }
    public override void _Process(double delta)
    {
        if (!Engine.IsEditorHint() && IsActive) _currentState.Process((float)delta);
    }
    public override void _PhysicsProcess(double delta)
    {
        if (!Engine.IsEditorHint() && IsActive)
        {
            _currentState.PhysicsProcess((float)delta);
            _currentState.CheckTransitions();
        }
    }
    public override string[] _GetConfigurationWarnings()
    {
        List<string> warnings = new();
        Node parent = GetParent();
        if (GetChildren().Count < 1)
        {
            warnings.Add("StateMachine must have at least one state");
        }
        if (InitialState == null)
        {
            warnings.Add("Set initial state");
        }
        return warnings.ToArray();
    }
    public void TransitionTo(string key)
    {
        if (!_states.ContainsKey(key))
        {
            GD.PushError($"Wrong state name: {key}");
            return;
        }
        if (_disableTransition)
        {
            _currentState = _states[key];
            GetState(key).CheckTransitions();
            return;
        }
        if (IsActive) _currentState.Exit();
        _currentState = _states[key];
        if (IsActive) _currentState.Enter();
    }
    public void Reset()
    {
        if (_currentState != InitialState) TransitionTo(InitialState.Name);
    }
    public void Activate()
    {
        if (ResetOnActivation) Reset();
        if (Mode == ActivationMode.CheckTransitions) CheckTransitions();
        IsActive = true;
        _currentState.Enter();
    }
    public void Deactivate()
    {
        IsActive = false;
        _currentState.Exit();
    }
    public State GetState(string name) => _states[name];
    void InitStates()
    {
        _states = new Dictionary<string, State>();
        foreach (Node node in GetChildren())
        {
            if (node is State s)
            {
                _states[node.Name] = s;
                s.Fsm = this;
            }
        }
        _currentState = (State)InitialState;
    }
    void CheckTransitions()
    {
        if (IsActive) _currentState.Exit();
        _disableTransition = true;
        _currentState.CheckTransitions();
        _disableTransition = false;
        if (IsActive) _currentState.Enter();
    }
    public enum ActivationMode
    {
        LastState,
        ResetToInitial,
        CheckTransitions
    }

}
