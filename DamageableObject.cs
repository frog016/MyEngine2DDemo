using MyEngine2D.Core.Entity;

namespace MyEngine2DDemo;

public abstract class DamageableObject : Component
{
    public int Health { get; protected set; }
    public int MaxHealth { get; protected set; }
    public event Action<DamageableObject> HealthDamaged;

    protected DamageableObject(GameObject gameObject) : base(gameObject)
    {
    }

    public abstract void ApplyDamage();

    protected void RaiseHealthDamagedEvent()
    {
        HealthDamaged?.Invoke(this);
    }
}