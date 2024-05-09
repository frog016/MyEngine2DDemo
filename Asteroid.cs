using MyEngine2D.Core;
using MyEngine2D.Core.Entity;
using MyEngine2D.Core.Graphic;
using MyEngine2D.Core.Level;
using MyEngine2D.Core.Physic;
using MyEngine2D.Core.Resource;
using MyEngine2D.Core.Structure;

namespace MyEngine2DDemo;

public class Asteroid : DamageableObject
{
    private RigidBody _rigidBody;

    public Asteroid(GameObject gameObject) : base(gameObject)
    {
    }

    public void Initialize(int maxHealth)
    {
        Health = maxHealth;
        MaxHealth = maxHealth;
    }

    public override void Start()
    {
        _rigidBody = GameObject.GetComponent<RigidBody>();
        _rigidBody.BodyCollided += OnAsteroidCollided;
    }

    public override void ApplyDamage()
    {
        Health--;
        RaiseHealthDamagedEvent();

        if (Health == 0)
        {
            GameObject.Destroy();
        }
    }

    private void OnAsteroidCollided(Contact contact)
    {
        var otherBody = contact.First == _rigidBody ? contact.Second : contact.First;
        var otherGameObject = otherBody.GameObject;

        if (otherGameObject.TryGetComponent<Ship>(out var ship))
        {
            ship?.ApplyDamage();
            GameObject.Destroy();
        }
    }

    public static Asteroid InstantiateBig(Vector2 position, float rotation)
    {
        return Instantiate(position, rotation, Vector2.One * 1.1f, "Big", 1);
    }

    public static Asteroid InstantiateMedium(Vector2 position, float rotation)
    {
        return Instantiate(position, rotation, Vector2.One * 0.7f, "Medium", 1);
    }

    public static Asteroid InstantiateSmall(Vector2 position, float rotation)
    {
        return Instantiate(position, rotation, Vector2.One * 0.4f, "Small", 1);
    }

    private static Asteroid Instantiate(Vector2 position, float rotation, Vector2 size, string namePostfix, int maxHealth)
    {
        const int minSpriteIndex = 1;
        const int maxSpriteIndex = 3;

        var level = ServiceLocator.Instance.Get<GameLevelManager>().CurrentLevel;
        var asteroidObject = level.Instantiate($"Asteroid_{namePostfix}", position, rotation, new Action<GameObject>[]
        {
            asteroidInstance =>
            {
                var rigidBody = asteroidInstance.AddComponent<RigidBody>();
                var shape = new RectanglePhysicShape(rigidBody, size);
                rigidBody.Initialize(shape, Constants.DefaultMaterial, 1, ComputeMassMode.Manually, gravityScale: 0,
                    layer: Constants.AsteroidLayer);
            },
            asteroidInstance =>
            {
                var spriteRenderer = asteroidInstance.AddComponent<SpriteRenderer>();
                var randomSpriteIndex = Random.Shared.Next(minSpriteIndex, maxSpriteIndex + 1);
                var spriteLoadData = new SpriteLoadData($"Asteroid/Asteroid_{namePostfix}_V{randomSpriteIndex}.png");
                spriteRenderer.Initialize(spriteLoadData, layer: 1);
            },
            asteroidInstance =>
            {
                var constantForce = asteroidInstance.AddComponent<RandomConstantForce>();
                constantForce.Initialize(10, 25);
            },
            asteroidInstance =>
            {
                asteroidInstance.AddComponent<ObjectScreenCycleWrapper>();
            },
            asteroidInstance =>
            {
                var asteroid = asteroidInstance.AddComponent<Asteroid>();
                asteroid.Initialize(maxHealth);
            },
        });

        return asteroidObject.GetComponent<Asteroid>();
    }
}