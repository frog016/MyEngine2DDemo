using MyEngine2D.Core.Entity;
using MyEngine2D.Core.Physic;
using MyEngine2D.Core.Structure;

namespace MyEngine2DDemo;

public class RandomConstantForce : Component
{
    private float _forceMin;
    private float _forceMax;

    public RandomConstantForce(GameObject gameObject) : base(gameObject)
    {
    }

    public void Initialize(float forceMin, float forceMax)
    {
        _forceMin = forceMin;
        _forceMax = forceMax;
    }

    public override void Start()
    {
        var rigidBody = GameObject.GetComponent<RigidBody>();
        ApplyForceInRandomDirection(rigidBody);
    }

    private void ApplyForceInRandomDirection(RigidBody rigidBody)
    {
        var randomDirection = new Vector2(GetRandomFloatInRange(-1, 1), GetRandomFloatInRange(-1, 1)).Normalize();
        var randomForce = GetRandomFloatInRange(_forceMin, _forceMax);

        rigidBody.ApplyForce(randomDirection * randomForce);
    }

    private static float GetRandomFloatInRange(float min, float max)
    {
        return min + Random.Shared.NextSingle() * (max - min);
    }
}