using MyEngine2D.Core.Entity;

namespace MyEngine2DDemo;

public class Ship : DamageableObject
{
    public Ship(GameObject gameObject) : base(gameObject)
    {
    }

    public void Initialize(int maxHealth)
    {
        Health = maxHealth;
        MaxHealth = maxHealth;
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
}