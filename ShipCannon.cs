using MyEngine2D.Core.Entity;
using MyEngine2D.Core.Input;
using MyEngine2D.Core.Structure;

namespace MyEngine2DDemo;

public class ShipCannon : Component
{
    private readonly InputSystem _inputSystem;

    private Vector2 _shootOffset;
    private Cooldown _cooldown;

    public ShipCannon(GameObject gameObject, InputSystem inputSystem) : base(gameObject)
    {
        _inputSystem = inputSystem;
    }

    public void Initialize(Vector2 shootOffset, float cooldownTime)
    {
        _shootOffset = shootOffset;
        _cooldown = new Cooldown(cooldownTime);
    }

    public override void Start()
    {
        _inputSystem.SubscribeInputListener<ShootInputAction>(ShootForward);
    }

    public override void Update(float deltaTime)
    {
        _cooldown.Tick(deltaTime);
    }

    public override void OnDestroy()
    {
        _inputSystem.UnsubscribeInputListener<ShootInputAction>(ShootForward);
    }

    private void ShootForward()
    {
        if (_cooldown.TryRestart() == false)
        {
            return;
        }

        var direction = Transform.GetForwardVector();
        var bullet = Bullet.Instantiate(GetShootPosition(), Transform.Rotation);

        bullet.Shoot(direction);
    }

    private Vector2 GetShootPosition()
    {
        return Transform.Position + _shootOffset.Rotate(Transform.Rotation);
    }
}