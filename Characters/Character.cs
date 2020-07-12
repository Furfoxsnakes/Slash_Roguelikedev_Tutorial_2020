using Godot;
using System;
using System.Collections.Generic;
using GoRogue;
using GoRogue.DiceNotation;
using GoRogue.GameFramework;

public class Character : Node2D, IGameObject
{
    public AnimatedSprite AnimatedSprite;
    public AnimationPlayer Anim;
    public Tween Tween;
    public Label HitText;
    private float _tweenLength = 0.2f;
    
    [Export] public int FOVRadius = 3;
    [Export] public int Level;
    [Export] public int Experience;
    [Export] public int MaxHealth;
    public int Health;
    [Export] public int Attack;
    [Export] public int AttackRating;
    [Export] public int Defense;
    [Export] public int DefenseRating;
    [Export] public int Gold;

    public AudioStreamPlayer Audio => GameController.Audio;
    [Export] private AudioStream[] _movementSounds;
    [Export] private AudioStream _hitSound;

    protected DungeonMap Map => GameController.DungeonMap;
    private IGameObject _gameObjectImplementation;

    public override void _Ready()
    {
        AnimatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        AnimatedSprite.Play();
        Anim = GetNode<AnimationPlayer>("Anim");
        HitText = GetNode<Label>("HitText");
        Tween = GetNode<Tween>("Tween");
        Health = MaxHealth;
    }

    public void PlayMovementSound()
    {
        if (_movementSounds.Length == 0) return;
        var stream = _movementSounds[(int)GD.RandRange(0, _movementSounds.Length)];
        Audio.Stream = stream;
        Audio.Stop();
        Audio.Play();
    }

    public void Kill()
    {
        Map.RemoveCharacter(this);
        QueueFree();
    }

    public void AttackCharacter(Character target)
    {
        var numHits = ResolveAttacks();
        var resolvedHits = ResolveDefense(numHits);
        target.TakeDamage(resolvedHits);
    }

    public int ResolveAttacks()
    {
        var hits = 0;
        for (var i = 0; i < Attack; ++i)
        {
            var roll = Dice.Roll("1d100");
            if (roll <= AttackRating)
                hits++;
        }

        return hits;
    }

    public int ResolveDefense(int numAttacks)
    {
        if (numAttacks == 0) return 0;
        var hits = numAttacks;

        for (var i = 0; i < Defense; ++i)
        {
            var roll = Dice.Roll("1d100");
            if (roll <= DefenseRating)
            {
                hits = Mathf.Max(0, hits - 1);
            }
        }

        return hits;
    }
    
    public void TakeDamage(int damage)
    {
        HitText.Text = damage == 0 ? "MISS!" : $"{damage} DAMAGE!";
        Anim.Stop();
        Anim.Play("Hit");

        Health -= damage;
        if (Health <= 0)
            // TODO: Game still trying to access after it's been disposed
            Kill();

        if (_hitSound == null) return;
        
        var audio = GameController.Audio;
        audio.Stop();
        audio.Stream = _hitSound;
        audio.Play();
    }

    public void TweenToPosition(Vector2 from, Vector2 to, float duration = 1f)
    {
        Tween.InterpolateProperty(this, "global_position", from, to, duration);
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
