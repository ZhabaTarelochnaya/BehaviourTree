using BehaviourTree;
using Godot;
using Godot.Collections;

public partial class SharedBlackboard : Node
{
    [Export] public Dictionary Blackboard { get; set; }
    [Export] public string BoardName { get; private set; }
    public Array<BTree> ConnectedTrees { get; private set; }
    public override void _Ready()
    {
        base._Ready();
        foreach (var item in ConnectedTrees) 
        {
            item.Blackboard.Add(BoardName, Blackboard);
        }
    }
    public void ConnectTree(BTree tree)
    {
        tree.Blackboard.Add(BoardName, Blackboard);
        ConnectedTrees.Add(tree);
    }
    public void DisconnectTree(BTree tree)
    {
        tree.Blackboard.Remove(BoardName);
        ConnectedTrees.Remove(tree);
    }
}
