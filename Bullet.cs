using MyEngine2D.Core;
using MyEngine2D.Core.Entity;
using MyEngine2D.Core.Graphic;
using MyEngine2D.Core.Level;
using MyEngine2D.Core.Physic;
using MyEngine2D.Core.Resource;
using MyEngine2D.Core.Structure;

namespace MyEngine2DDemo;

public class Bullet : Component
{
    private RigidBody _rigidBody;
    private LifeTime _lifeTime;

    private float _launchSpeed = 50f;

    public Bullet(GameObject gameObject) : base(gameObject)
    {
    }

    public void Initialize(float launchSpeed)
    {
        _launchSpeed = launchSpeed;
    }

    public void Shoot(Vector2 direction)
    {
        _rigidBody.ApplyForce(direction * _launchSpeed);
        _lifeTime.Activate();
    }

    public override void Start()
    {
        _rigidBody = GameObject.GetComponent<RigidBody>();
        _lifeTime = GameObject.GetComponent<LifeTime>();

        _rigidBody.BodyCollided += OnBulletCollided;
    }

    public override void OnDestroy()
    {
        _rigidBody.BodyCollided -= OnBulletCollided;
    }

    private void OnBulletCollided(Contact contact)
    {
        var otherBody = contact.First == _rigidBody ? contact.Second : contact.First;
        var otherGameObject = otherBody.GameObject;

        if (otherGameObject.TryGetComponent<Asteroid>(out var asteroid))
        {
            asteroid?.ApplyDamage();
            GameObject.Destroy();
        }
    }

    public static Bullet Instantiate(Vector2 position, float rotation)
    {
        var level = ServiceLocator.Instance.Get<GameLevelManager>().CurrentLevel;
        var bulletObject = level.Instantiate("Bullet", position, rotation, new Action<GameObject>[]
        {
            bulletInstance =>
            {
                var rigidBody = bulletInstance.AddComponent<RigidBody>();
                var shape = new CirclePhysicShape(rigidBody, 0.08f);
                rigidBody.Initialize(shape, Constants.DefaultMaterial, 1, ComputeMassMode.Manually, 0, layer: Constants.BulletLayer);
            },
            bulletInstance =>
            {
                var spriteRenderer = bulletInstance.AddComponent<SpriteRenderer>();
                var spriteLoadData = new SpriteLoadData("Ship/Ship_Projectile.png");
                spriteRenderer.Initialize(spriteLoadData, layer: 0);
            },
            bulletInstance =>
            {
                var lifeTime = bulletInstance.AddComponent<LifeTime>();
                lifeTime.Initialize(10);
            },
            bulletInstance =>
            {
                var bullet = bulletInstance.AddComponent<Bullet>();
                bullet.Initialize(120);
            },
            bulletInstance =>
            {
                bulletInstance.AddComponent<ObjectScreenCycleWrapper>();
            },
        });

        return bulletObject.GetComponent<Bullet>();
    }
}