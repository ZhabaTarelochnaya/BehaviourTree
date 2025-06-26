using Godot;
using Godot.Collections;
using System;
namespace BehaviourTree.Leafs;
[Tool]
[Icon("res://addons/behaviourtree/Icons/leafs/animation.svg")]
public partial class PlayAnimation : Leaf
{
    [Export] AnimationPlayer _animationPlayer;
    [Export] string _animationName;
    [Export] bool _waitUntillEnd = true;
    /// <summary>
    /// If true, plays part of animation specified by three variables below.
    /// </summary>
    [Export] bool _playPart = false;
    [Export] int _framesTotal;
    [Export] int _firstFrameIndex = 0;
    [Export] int _lastFrameIndex = 1;
    bool _isAnimationPlaying;
    bool _resultReady;
    [Export] public float Speed { get; set; } = 1;
    public event Action AnimationFinished;
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (!Engine.IsEditorHint() && _isAnimationPlaying)
        {
            if (!((BTree)Root).IsActive) { return; }
            if (!_waitUntillEnd) _resultReady = true;
            if (_playPart)
            {
                if (_animationPlayer.CurrentAnimationPosition >= GetFrameLength() * _lastFrameIndex)
                {
                    _animationPlayer.Pause();
                    if (_waitUntillEnd) _resultReady = true;
                    _isAnimationPlaying = false;
                }
                return;
            }
            if (!_waitUntillEnd) _isAnimationPlaying = false;
            else if (!_animationPlayer.IsPlaying())
            {
                _resultReady = true;
                _isAnimationPlaying = false;
            }
        }
    }
    public override void Run(Dictionary blackboard)
    {
        if (!_animationPlayer.HasAnimation(_animationName))
        {
            GD.PrintErr($"{_animationPlayer.Name} doesn't have {_animationName} animation");
            return;
        }
        if (_isAnimationPlaying && _waitUntillEnd)
        {
            SetRunning();
            return;
        }
        if(_resultReady)
        {
            SetSuccess();
            _resultReady = false;
            return;
        }
        if (_animationPlayer.CurrentAnimation == _animationName && !AnimationLooping())
        {
            _animationPlayer.Stop();
        }
        _animationPlayer.CurrentAnimation = _animationName;
        _isAnimationPlaying = true;
        if (_playPart)
        {
            _animationPlayer.Seek(GetFrameLength() * _firstFrameIndex);
            _animationPlayer.Play(_animationName, customSpeed: Speed);
        }
        else _animationPlayer.Play(_animationName, customSpeed: Speed);
        if(!_waitUntillEnd) SetSuccess();
    }
    float GetFrameLength() => _animationPlayer.GetAnimation(_animationName).Length / _framesTotal;
    bool AnimationLooping() => _animationPlayer.GetAnimation(_animationName).LoopMode == Animation.LoopModeEnum.Linear;
}
