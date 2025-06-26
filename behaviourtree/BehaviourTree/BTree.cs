using Godot;
using Godot.Collections;
using System.Collections.Generic;
namespace BehaviourTree;
[Tool]
[Icon("res://addons/behaviourtree/Icons/Tree.svg")]
public partial class BTree : Task
{
    [Export] public bool IsActive { get; set; } = true;
    /// <summary>
    ///  Amount of times AI ticks in physycs frame.
    /// </summary>
    [Export] public int TickSpeed { get; set; } = 1;
    [Export] public Godot.Collections.Dictionary Blackboard { get; set; }
    [Export] public bool DebugMode { get; set; }
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
            Root = this;
            if (DebugMode)
            {
                foreach (Task child in _children)
                {
                    IsDebugOn = true;
                }
            }
            Start();
        }
    }
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (IsActive && !Engine.IsEditorHint())
        {
            for (int i = 0; i < TickSpeed; i++)
            {
                Run(Blackboard);
            }
        }
    }
    public override string[] _GetConfigurationWarnings()
    {
        List<string> warnings = new List<string>();
        Array<Node> children = GetChildren();
        if (children.Count == 0)
        {
            warnings.Add("Tree must have a child");
        }
        else 
        {
            foreach (Node child in children) 
            {
                if(!(child is Task))
                {
                    warnings.Add("Children must be of type Task");
                }
            }
        }
        return warnings.ToArray();
    }

    public override void Run(Dictionary blackboard)
    {
        foreach (Task child in _children)
        {
            child.Run(Blackboard);
        }
    }
}
