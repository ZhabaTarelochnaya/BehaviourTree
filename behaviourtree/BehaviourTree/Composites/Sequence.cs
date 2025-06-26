using Godot;
using Godot.Collections;
using System.Collections.Generic;
namespace BehaviourTree.Composites;
[Tool]
[Icon("res://addons/behaviourtree/Icons/composites/Sequence.svg")]
public partial class Sequence : Composite
{
    int _currentChild;
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
        _children[_currentChild].Run(blackboard);
        SetRunning();
    }
    public override void ChildSuccess()
    {
        base.ChildSuccess();
        _currentChild += 1;
        if (_currentChild >= GetChildCount()) 
        {
            _currentChild = 0;
            SetSuccess();
        }
    }
    public override void ChildFail()
    {
        base.ChildFail();
        _currentChild = 0;
        SetFail();
    }
    public override void Cancel()
    {
        base.Cancel();
        _currentChild = 0;
    }
    public override void Start()
    {
        base.Start();
        _currentChild = 0;
    }
}
