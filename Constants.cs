using MyEngine2D.Core.Physic;

namespace MyEngine2DDemo;

public static class Constants
{
    public static readonly PhysicMaterial DefaultMaterial = 
        new(2200f, 0.2f, 0.1f, 0.8f);

    public static readonly ConcreteLayer BulletLayer = new ConcreteLayer("Bullet", 1);
    public static readonly ConcreteLayer ShipLayer = new ConcreteLayer("Ship", 2);
    public static readonly ConcreteLayer AsteroidLayer = new ConcreteLayer("Asteroid", 3);
}