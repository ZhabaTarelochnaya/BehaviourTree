using Godot;
using Godot.Collections;
using System.Collections.Generic;
namespace BehaviourTree.Composites;
[Tool]
[Icon("res://addons/behaviourtree/Icons/composites/psequence.svg")]
public partial class ParallelSequence : Composite
{
    int _successes;
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
        }
    }
    public override void Run(Dictionary blackboard)
    {
        foreach (Task child in _children)
        {
            child.Run(blackboard);
        }
    }
    public override void ChildSuccess()
    {
        base.ChildSuccess();
        _successes += 1;
        if (_successes >= GetChildCount())
        {
            _successes = 0;
            SetSuccess();
        }
    }
    public override void ChildFail()
    {
        base.ChildFail();
        _successes = 0;
        SetFail();
    }
}
