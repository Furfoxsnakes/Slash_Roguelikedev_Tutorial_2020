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
