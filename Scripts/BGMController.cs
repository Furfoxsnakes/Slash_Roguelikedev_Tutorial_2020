using Godot;
using System;

public class BGMController : AudioStreamPlayer
{
    private Timer _delayTimer;
    private Tween _fadeTween;

    [Export] public float MaxVolume;
    [Export] private bool _playOnStart;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _delayTimer = GetNode<Timer>("DelayTimer");
        _fadeTween = GetNode<Tween>("FadeTween");
        VolumeDb = MaxVolume;
        if (_playOnStart) FadeIn();
    }

    public override void _Process(float delta)
    {
        if (!Playing) return;
        
        if (Stream.GetLength() - GetPlaybackPosition() <= 1.1f)
            FadeOut();
    }

    private void _on_DelayTimer_timeout()
    {
        FadeIn();
    }

    public void FadeIn()
    {
        VolumeDb = -80f;
        Play();
        _fadeTween.InterpolateProperty(this, "volume_db", -80, MaxVolume, 1f);
        _fadeTween.Start();
    }

    public void FadeOut()
    {
        _fadeTween.InterpolateProperty(this, "volume_db", MaxVolume, -80, 1f);
        _fadeTween.Start();
    }

    private void _on_BGM_finished()
    {
        var s = (float)GD.RandRange(10f, 20f);
        _delayTimer.WaitTime = s;
        _delayTimer.Start();
    }
}
