using Godot;
using System;
using System.Collections.Generic;
using GoRogue;
using GoRogue.GameFramework;

public class Character : Node2D, IGameObject
{
    public AnimatedSprite AnimatedSprite;
    public Tween Tween;
    private float _tweenLength = 0.1f;

    public int FOVRadius = 3;

    protected DungeonMap Map;
    // protected DungeonMap Map => GameController.Instance.DungeonMap;
    private IGameObject _gameObjectImplementation;

    public override void _Ready()
    {
        AnimatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        AnimatedSprite.Play();
        Map = GetTree().Root.GetNode<DungeonMap>("Game/Nav/TempMap");
        Tween = GetNode<Tween>("Tween");
    }

    public void Kill()
    {
        Map.RemoveCharacter(this);
        QueueFree();
    }

    public void TweenToPosition(Vector2 from, Vector2 to)
    {
        Tween.InterpolateProperty(this, "global_position", from, to, _tweenLength);
        Tween.Start();
    }

    public void BumpObject(IGameObject gameObject)
    {
        var objectWorldPos = Map.MapToWorld(new Vector2(gameObject.Position.X, gameObject.Position.Y));
        var startingPos = GlobalPosition;
        Tween.InterpolateProperty(this, "global_position", startingPos, objectWorldPos, _tweenLength);
        Tween.InterpolateProperty(this, "global_position", objectWorldPos, startingPos, _tweenLength);
        Tween.Start();
    }

    public override void _Process(float delta)
    {
        // this doesn't seem very well optimized
        if (Map.FOV[Position] > 0)
            AnimatedSprite.Show();
        else
            AnimatedSprite.Hide();
    }

    public Character()
    {
        _gameObjectImplementation = new GameObject(Coord.NONE, 1, this, false, false, false);
    }

    #region IGameObject

    public uint ID => _gameObjectImplementation.ID;

    public int Layer => _gameObjectImplementation.Layer;

    public void AddComponent(object component) => _gameObjectImplementation.AddComponent(component);

    public T GetComponent<T>() => _gameObjectImplementation.GetComponent<T>();

    public IEnumerable<T> GetComponents<T>() => _gameObjectImplementation.GetComponents<T>();

    public bool HasComponent(Type componentType) => _gameObjectImplementation.HasComponent(componentType);

    public bool HasComponent<T>() => _gameObjectImplementation.HasComponent<T>();

    public bool HasComponents(params Type[] componentTypes) => _gameObjectImplementation.HasComponents(componentTypes);

    public void RemoveComponent(object component) => _gameObjectImplementation.RemoveComponent(component);

    public void RemoveComponents(params object[] components) => _gameObjectImplementation.RemoveComponents(components);

    public bool MoveIn(Direction direction) => _gameObjectImplementation.MoveIn(direction);

    public void OnMapChanged(Map newMap) => _gameObjectImplementation.OnMapChanged(newMap);

    public Map CurrentMap => _gameObjectImplementation.CurrentMap;

    public bool IsStatic => _gameObjectImplementation.IsStatic;

    public bool IsTransparent
    {
        get => _gameObjectImplementation.IsTransparent;
        set => _gameObjectImplementation.IsTransparent = value;
    }

    public bool IsWalkable
    {
        get => _gameObjectImplementation.IsWalkable;
        set => _gameObjectImplementation.IsWalkable = value;
    }

    public new Coord Position
    {
        get => _gameObjectImplementation.Position;
        set
        {
            _gameObjectImplementation.Position = value;
            GlobalPosition = Map.MapToWorld(new Vector2(value.X, value.Y));
        }
    }

    public event EventHandler<ItemMovedEventArgs<IGameObject>> Moved
    {
        add => _gameObjectImplementation.Moved += value;
        remove => _gameObjectImplementation.Moved -= value;
    }

    #endregion
    
}
