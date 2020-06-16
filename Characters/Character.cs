using Godot;
using System;
using System.Collections.Generic;

public class Character : Node2D
{
    public Dictionary<Vector2, RayCast2D> RayCastDirections = new Dictionary<Vector2, RayCast2D>();
    public AnimatedSprite AnimatedSprite;
    
    protected TileMap Map;

    protected Vector2 MapPosition
    {
        get => Map.WorldToMap(Position);
        set => Position = Map.MapToWorld(value);
    }
    
    public override void _Ready()
    {
        AnimatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        AnimatedSprite.Play();
        Map = GetParent<TileMap>();
        GetRaycastDirections();
    }

    public bool MoveBy(Vector2 movement)
    {
        if (RayCastDirections[movement].IsColliding()) return false;

        MapPosition += movement;
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
