using MyEngine2D.Core;
using MyEngine2D.Core.Entity;
using MyEngine2D.Core.Graphic;
using MyEngine2D.Core.Level;
using MyEngine2D.Core.Structure;
using MyEngine2D.Core.Utility;

namespace MyEngine2DDemo;

public class AsteroidSpawner : Component
{
    private Vector2 _borderOffset;
    private Cooldown _spawnCooldown;
    private Camera _camera;

    private static readonly Func<Vector2, float, Asteroid>[] AsteroidFactories = new[]
    {
        Asteroid.InstantiateSmall,
        Asteroid.InstantiateMedium,
        Asteroid.InstantiateBig,
    };

    public AsteroidSpawner(GameObject gameObject) : base(gameObject)
    {
    }

    public void Initialize(Vector2 borderOffset, float spawnRate)
    {
        _borderOffset = borderOffset;
        _spawnCooldown = new Cooldown(spawnRate);
    }

    public override void Start()
    {
        var levelManager = ServiceLocator.Instance.Get<GameLevelManager>();
        _camera = levelManager.CurrentLevel.GameObjects.FindByType<Camera>();
    }

    public override void Update(float deltaTime)
    {
        _spawnCooldown.Tick(deltaTime);
        if (_spawnCooldown.TryRestart())
        {
            SpawnRandomAsteroid();
        }
    }

    private void SpawnRandomAsteroid()
    {
        var index = Random.Shared.Next(0, AsteroidFactories.Length);
        var factory = AsteroidFactories[index];

        var position = GetRandomPosition();
        var rotation = GetRandomRotation();

        factory.Invoke(position, rotation);
    }

    private Vector2 GetRandomPosition()
    {
        var cameraRect = _camera.GetViewRectangle();
        var min = cameraRect.Min + _borderOffset;
        var max = cameraRect.Max - _borderOffset;

        var x = GetRandomFloatInRange(min.X, max.X);
        var y = GetRandomFloatInRange(min.Y, max.Y);

        return new Vector2(x, y);
    }

    private static float GetRandomRotation()
    {
        return Random.Shared.Next(0, 360);
    }

    private static float GetRandomFloatInRange(float min, float max)
    {
        return min + Random.Shared.NextSingle() * (max - min);
    }
}