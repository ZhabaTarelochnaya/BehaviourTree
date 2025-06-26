using Godot;
using Godot.Collections;
using System.Collections.Generic;
namespace BehaviourTree.Composites;
[Tool]
[Icon("res://addons/behaviourtree/Icons/composites/Random.svg")]
public partial class Random : Composite
{
    int _index;
    int[] _sequence;
    System.Random rng;
    List<Task> _children;
    public override void _Ready()
    {
        base._Ready();
        if (!Engine.IsEditorHint())
        {
            _children = new List<Task>();
            foreach (Task child in GetChildren())
            {
                if (child is Task) _children.Add(child);
            }
            _sequence = new int[GetChildCount()];
            for (int i = 0; i < GetChildCount(); i++)
            {
                _sequence[i] = i;
            }
            rng = new System.Random();
            SetSequence();
        }
    }
    public override void Run(Dictionary blackboard)
    {
        GetChild<Task>(_sequence[_index]).Run(blackboard);
        SetRunning();
    }
    public override void ChildFail()
    {
        base.ChildFail();
        SetSequence();
        SetFail();
    }
    public override void ChildSuccess()
    {
        base.ChildSuccess();
        SetSequence();
        SetSuccess();
    }
    void SetSequence()
    {
        _index = 0;
        rng.Shuffle(_sequence);
    }
    public override void Cancel()
    {
        base.Cancel();
        SetSequence();
    }
    public override void Start()
    {
        base.Start();
        SetSequence();
    }
}
