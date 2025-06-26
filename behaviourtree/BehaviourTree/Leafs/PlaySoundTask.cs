using Godot;
using Godot.Collections;
namespace BehaviourTree.Leafs;
[Tool]
[Icon("res://addons/behaviourtree/Icons/leafs/PlaySound.svg")]
public partial class PlaySoundTask : Leaf
{
    [Export] AudioStreamPlayer _audioPlayer;
    [Export] AudioStreamPlayer2D _audioPlayer2D;
    [Export] bool _is2D;
    public override void Run(Dictionary blackboard)
    {
        if (_is2D) _audioPlayer2D.Play();
        else _audioPlayer.Play();
        SetSuccess();
    }
}