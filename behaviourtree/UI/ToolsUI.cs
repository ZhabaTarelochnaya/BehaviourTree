#if TOOLS
using Godot;
using BehaviourTree.Composites;
using BehaviourTree.Decorators;
using BehaviourTree.Leafs;
using System.Collections.Generic;
using FSM;
namespace BehaviourTree.Plugin;
[Tool]
public partial class ToolsUI : Control
{
	Node _currentSelection;
    Dictionary<string, Callable> _callables;
    public EditorUndoRedoManager UndoRedo { get; set; }
	
	public override void _EnterTree()
	{
        _callables = new Dictionary<string, Callable>()
        {
            {"Sequence" , new Callable(this, "OnSequencePressed")},
            {"Selector" , new Callable(this, "OnSelectorPressed")},
            {"RandomSequence" , new Callable(this, "OnRandomSequencePressed")},
            {"RandomSelector" , new Callable(this, "OnRandomSelectorPressed")},
            {"Random" , new Callable(this, "OnRandomPressed")},
            {"UntilSuccess" , new Callable(this, "OnUntilSuccessPressed")},
            {"UntilFail" , new Callable(this, "OnUntilFailPressed")},
            {"Repeat" , new Callable(this, "OnRepeatPressed")},
            {"Limit" , new Callable(this, "OnLimitPressed")},
            {"Invert" , new Callable(this, "OnInvertPressed")},
            {"AlwaysSucceed" , new Callable(this, "OnAlwaysSucceedPressed")},
            {"AlwaysFail" , new Callable(this, "OnAlwaysFailPressed")},
            {"Print" , new Callable(this, "OnPrintPressed")},
            {"Wait" , new Callable(this, "OnWaitPressed")},
            {"FSM" , new Callable(this, "OnFSMPressed")},
            {"FSMBridge" , new Callable(this, "OnFSMBridgePressed")},
            {"ParallelSequence" , new Callable(this, "OnParallelSequencePressed")},
            {"ParallelSelector" , new Callable(this, "OnParallelSelectorPressed")},
            {"BlackboardDebug" , new Callable(this, "OnBlackboardDebugPressed")},
            {"PlayAnimation" , new Callable(this, "OnPlayAnimationPressed")},
            {"Cooldown" , new Callable(this, "OnCooldownPressed")},
            {"PrintResult" , new Callable(this, "OnPrintResultPressed")},
            {"PlaySound" , new Callable(this, "OnPlaySoundPressed")},
            {"Duration" , new Callable(this, "OnDurationPressed")},
        };
        ConnectAll();
    }
    public override void _ExitTree() => DisconnectAll();
    public void SetCurrentSelection(Node selection) => _currentSelection = selection;
    void ConnectAll()
    {
        foreach (KeyValuePair<string, Callable> pair in _callables)
        {
            GetNode<Button>($"%{pair.Key}").Connect(Button.SignalName.Pressed, pair.Value);
        }
    }
    void DisconnectAll()
    {
        if (_callables != null)
        {
            foreach (KeyValuePair<string, Callable> pair in _callables)
            {
                if (GetNode<Button>($"%{pair.Key}").IsConnected(Button.SignalName.Pressed, pair.Value))
                {
                    GetNode<Button>($"%{pair.Key}").Disconnect(Button.SignalName.Pressed, pair.Value);
                }
            }
        }
    }
	void OnButtonPressed<T>(T newNode, string uniqueName) where T : Node
	{
        if (Input.IsKeyPressed(Key.Shift)) AddAsParent(newNode);
        else AddAsChild(newNode);
        string newName = uniqueName;
        int i = 1;
        while (newNode.GetParent().HasNode(newName))
        {
            newName = uniqueName + i++.ToString();
        }
        newNode.Name = newName;
    }
    void AddAsChild<T>(T newNode) where T : Node
    {
        UndoRedo.CreateAction("Add as child");
        UndoRedo.AddDoMethod(_currentSelection, "add_child", newNode);
        UndoRedo.AddUndoMethod(_currentSelection, "remove_child", newNode);
        UndoRedo.AddDoMethod(newNode, "set_owner", _currentSelection.GetTree().EditedSceneRoot);
        UndoRedo.CommitAction();
    }
    void AddAsParent<T>(T newNode) where T : Node
    {
        if(_currentSelection == _currentSelection.GetTree().EditedSceneRoot)
        {
            GD.PrintErr("Error selecting root");
            return;
        }
        UndoRedo.CreateAction("Add as parent");
        UndoRedo.AddDoMethod(_currentSelection.GetParent(), "add_child", newNode);
        UndoRedo.AddUndoMethod(_currentSelection.GetParent(), "remove_child", newNode);
        UndoRedo.AddDoMethod(_currentSelection, "reparent", newNode);
        UndoRedo.AddUndoMethod(_currentSelection, "reparent", _currentSelection.GetParent());
        UndoRedo.AddDoMethod(newNode, "set_owner", _currentSelection.GetTree().EditedSceneRoot);
        UndoRedo.AddDoMethod(_currentSelection, "set_owner", _currentSelection.GetTree().EditedSceneRoot);
        DoSetOwnersOfChildren(_currentSelection, _currentSelection);
        UndoRedo.AddUndoMethod(_currentSelection, "set_owner", _currentSelection.GetTree().EditedSceneRoot);
        UndoSetOwnersOfChildren(_currentSelection, _currentSelection);
        UndoRedo.AddDoMethod(_currentSelection.GetParent(), "move_child", newNode, _currentSelection.GetIndex());
        UndoRedo.AddUndoMethod(_currentSelection.GetParent(), "move_child", _currentSelection, _currentSelection.GetIndex());
        UndoRedo.AddDoMethod(EditorInterface.Singleton.GetSelection(), "clear");
        UndoRedo.AddUndoMethod(EditorInterface.Singleton.GetSelection(), "clear");
        UndoRedo.AddDoMethod(EditorInterface.Singleton.GetSelection(), "add_node", newNode);
        UndoRedo.AddUndoMethod(EditorInterface.Singleton.GetSelection(), "add_node", _currentSelection);
        UndoRedo.AddDoMethod(EditorInterface.Singleton, "edit_node", newNode);
        UndoRedo.AddUndoMethod(EditorInterface.Singleton, "edit_node", _currentSelection);
        UndoRedo.CommitAction();
    }
    void DoSetOwnersOfChildren(Node node, Node owner)
    {
        if (node.GetChildCount() > 0)
        {
            foreach (Node child in node.GetChildren())
            {
                UndoRedo.AddDoMethod(child, "set_owner", owner.GetTree().EditedSceneRoot);
                DoSetOwnersOfChildren(child, owner);
            }
        }
    }
    void UndoSetOwnersOfChildren(Node node, Node owner)
    {
        if (node.GetChildCount() > 0)
        {
            foreach (Node child in node.GetChildren())
            {
                UndoRedo.AddUndoMethod(child, "set_owner", owner.GetTree().EditedSceneRoot);
                UndoSetOwnersOfChildren(child, owner);
            }
        }
    }
    #region Composites
    void OnSequencePressed() => OnButtonPressed(new Sequence(), "Sequence");
    void OnSelectorPressed() => OnButtonPressed(new Selector(), "Selector");
    void OnRandomSequencePressed() => OnButtonPressed(new RandomSequence(), "RandomSequence");
    void OnRandomSelectorPressed() => OnButtonPressed(new RandomSelector(), "RandomSelector");
    void OnRandomPressed() => OnButtonPressed(new Random(), "Random");
    void OnParallelSequencePressed() => OnButtonPressed(new ParallelSequence(), "ParallelSequence");
    void OnParallelSelectorPressed() => OnButtonPressed(new ParallelSelector(), "ParallelSelector");
    #endregion
    #region Decorators
    void OnUntilSuccessPressed() => OnButtonPressed(new UntilSuccess(), "UntilSuccess");
    void OnUntilFailPressed() => OnButtonPressed(new UntilFail(), "UntilFail");
    void OnRepeatPressed() => OnButtonPressed(new Repeat(), "Repeat");
    void OnLimitPressed() => OnButtonPressed(new Limit(), "Limit");
    void OnInvertPressed() => OnButtonPressed(new Invert(), "Invert");
    void OnAlwaysSucceedPressed() => OnButtonPressed(new AlwaysSucceed(), "AlwaysSucceed");
    void OnAlwaysFailPressed() => OnButtonPressed(new AlwaysFail(), "AlwaysFail");
    void OnPrintResultPressed() => OnButtonPressed(new PrintResult(), "PrintResult");
    #endregion
    #region Leafs
    void OnPrintPressed() => OnButtonPressed(new Print(), "Print");
    void OnWaitPressed() => OnButtonPressed(new Wait(), "Wait");
    void OnBlackboardDebugPressed() => OnButtonPressed(new BlackboardDebug(), "BlackboardDebug");
    void OnPlayAnimationPressed() => OnButtonPressed(new PlayAnimation(), "PlayAnimation");
    void OnCooldownPressed() => OnButtonPressed(new Cooldown(), "Cooldown");
    void OnPlaySoundPressed() => OnButtonPressed(new PlaySoundTask(), "PlaySound");
    void OnDurationPressed() => OnButtonPressed(new Duration(), "Duration");
    #endregion
    void OnFSMPressed() => OnButtonPressed(new StateMachine(), "FSM");
    void OnFSMBridgePressed() => OnButtonPressed(new FSMBridge(), "FSMBridge");
}
#endif