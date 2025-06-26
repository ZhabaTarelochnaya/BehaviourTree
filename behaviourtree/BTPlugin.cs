#if TOOLS
using Godot;
using Godot.Collections;
namespace BehaviourTree.Plugin;
[Tool]
public partial class BTPlugin : EditorPlugin
{
    PackedScene _toolsUI = GD.Load<PackedScene>("res://addons/behaviourtree/UI/ToolsUI.tscn");
    EditorUndoRedoManager _undoRedo;
    ToolsUI _uiCanvas;
    public override void _EnterTree()
    {
        _undoRedo = GetUndoRedo();
        _uiCanvas = _toolsUI.Instantiate<ToolsUI>();
        AddControlToContainer(CustomControlContainer.CanvasEditorSideLeft, _uiCanvas);
        _uiCanvas.Visible = false;
        _uiCanvas.UndoRedo = _undoRedo;
        EditorInterface.Singleton.GetSelection().Connect(EditorSelection.SignalName.SelectionChanged, new Callable(this, "OnSelectionChanged"));
        AddCustomType("BehaviourTree", "Node", GD.Load<Script>("res://addons/behaviourtree/BehaviourTree/BTree.cs"), GD.Load<Texture2D>("res://addons/behaviourtree/Icons/Tree.svg"));
        AddCustomType("SharedBlackboard", "Node", GD.Load<Script>("res://addons/behaviourtree/BehaviourTree/SharedBlackboard.cs"), GD.Load<Texture2D>("res://addons/behaviourtree/Icons/Tree.svg"));
        AddCustomType("FSM", "Node", GD.Load<Script>("res://addons/behaviourtree/FSM/StateMachine.cs"), GD.Load<Texture2D>("res://addons/behaviourtree/Icons/FSM/StateMachine.svg"));
    }

    public override void _ExitTree()
    {
        RemoveControlFromContainer(CustomControlContainer.CanvasEditorSideLeft, _uiCanvas);
        _uiCanvas.QueueFree();
        RemoveCustomType("BehaviourTree");
        RemoveCustomType("FSM");
    }
    void OnSelectionChanged()
    {
        Array<Node> selection = EditorInterface.Singleton.GetSelection().GetSelectedNodes();
        if (selection.Count == 0)
        {
            _uiCanvas.Visible = false;
            return;
        }
        _uiCanvas.SetCurrentSelection(selection[0]);
        foreach (Node node in selection)
        {
            if (node is Task)
            {
                _uiCanvas.Visible = true;
                return;
            }
        }
        _uiCanvas.Visible = false;
    }
}
#endif
