using Godot;
using Godot.Collections;
using System.Collections.Generic;
using System.Diagnostics;
namespace BehaviourTree;
[Tool]
public abstract partial class Task : Node
{
    public TaskState Status { get; private set; } = TaskState.Fresh;
	public Task Parent { get; private set; } = null;
	public Task Root { get; protected set; } = null;
    public bool IsDebugOn { get; set; }
    public void SetRunning()
	{
		Status = TaskState.Running;
		Parent?.ChildRunning();
	}
	public void SetSuccess()
    {
        Status = TaskState.Succeeded;
        Parent?.ChildSuccess();
        DebugStatus();
    }
	public void SetFail()
    {
        Status = TaskState.Failed;
        Parent?.ChildFail();
        DebugStatus();
    }
	public virtual void Cancel()
	{
		if (Status == TaskState.Running)
		{
			Status = TaskState.Canceled;
			foreach (Task child in GetChildren())
			{
				child.Cancel();
			}
        }
	}
	public void Reset()
	{
		Cancel();
		Status = TaskState.Fresh;
    }
    public void DebugStatus()
    {
        if (IsDebugOn) GD.Print($"{Name}: {Status}");
    }
    public virtual void Start()
	{
		Status = TaskState.Fresh;
		List<Task> children = new();
        foreach (var item in GetChildren())
		{
            if(item is Task) children.Add((Task)item);
        }
		foreach (Task child in children)
		{
            child.Parent = this;
            child.Root = Root;
            child.IsDebugOn = IsDebugOn;
            child.Start();
		}
	}
	public virtual void ChildRunning() { }
    public virtual void ChildSuccess() { }
	public virtual void ChildFail() { }
	public abstract void Run(Dictionary blackboard);
    
    public enum TaskState
    {
        Fresh,
        Running,
        Failed,
        Succeeded,
        Canceled
    }
}

