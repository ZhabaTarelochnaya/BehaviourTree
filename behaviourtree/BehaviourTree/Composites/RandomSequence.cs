using Godot;
using Godot.Collections;
using System.Collections.Generic;
namespace BehaviourTree.Composites;
[Tool]
[Icon("res://addons/behaviourtree/Icons/composites/RandomSequence.svg")]
public partial class RandomSequence : Composite
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
            foreach (Node child in GetChildren())
            {
                if (child is Task) _children.Add((Task)child);
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
        _children[_sequence[_index]].Run(blackboard);
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
        _index += 1;
        if (_index >= _sequence.Length)
        {
            SetSequence();
            SetSuccess();
        }
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
    void SetSequence()
    {
        _index = 0;
        rng.Shuffle(_sequence);
    }
}
