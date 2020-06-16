using Godot;
using System;
using System.Collections.Generic;

public class Character : Node2D
{
    public Dictionary<Vector2, RayCast2D> RayCastDirections = new Dictionary<Vector2, RayCast2D>();
    public AnimatedSprite AnimatedSprite;
    public Tween Tween;
    private float _tweenLength = 0.1f;
    
    protected TileMap Map;

    protected Vector2 MapPosition
    {
        get => Map.WorldToMap(Position);
        set => SetWorldPosition(value, false);
    }

    public void SetWorldPosition(Vector2 value, bool doTween)
    {
        var oldPos = Position;
        var newPos = Map.MapToWorld(value);

        if (doTween)
        {
            Tween.InterpolateProperty(this, "Position", oldPos, newPos, _tweenLength);
            Tween.Start();
        }

        Position = newPos;
    }

    public override void _Ready()
    {
        AnimatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        AnimatedSprite.Play();
        Map = GetParent<TileMap>();
        Tween = GetNode<Tween>("Tween");
        GetRaycastDirections();
    }

    public bool MoveBy(Vector2 movement)
    {
        if (RayCastDirections[movement].IsColliding())
        {
            // bump into a wall
            var curPos = Position;
            var newPos = Position + movement * (Map.CellSize / 2);
            Tween.InterpolateProperty(this, "Position", curPos, newPos, _tweenLength/2);
            Tween.InterpolateProperty(this, "Position", newPos, curPos, _tweenLength/2);
            Tween.Start();
            return false;
        }

        SetWorldPosition(MapPosition + movement, true);
        return true;
    }

    private void GetRaycastDirections()
    {
        RayCastDirections[Vector2.Up] = GetNode<RayCast2D>("Raycasts/RayCastNorth");
        RayCastDirections[Vector2.Right] = GetNode<RayCast2D>("Raycasts/RayCastEast");
        RayCastDirections[Vector2.Down] = GetNode<RayCast2D>("Raycasts/RayCastSouth");
        RayCastDirections[Vector2.Left] = GetNode<RayCast2D>("Raycasts/RayCastWest");
        RayCastDirections[new Vector2(1, -1)] = GetNode<RayCast2D>("Raycasts/RayCastNorthEast");
        RayCastDirections[new Vector2(1, 1)] = GetNode<RayCast2D>("Raycasts/RayCastSouthEast");
        RayCastDirections[new Vector2(-1, 1)] = GetNode<RayCast2D>("Raycasts/RayCastSouthWest");
        RayCastDirections[new Vector2(-1, -1)] = GetNode<RayCast2D>("Raycasts/RayCastNorthWest");
    }
}
