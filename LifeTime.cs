using MyEngine2D.Core.Entity;

namespace MyEngine2DDemo;

public class LifeTime : Component
{
    private float _maxLifeTime = 10f;
    private float _currentLifeTime;
    private bool _activated;

    public LifeTime(GameObject gameObject) : base(gameObject)
    {
    }

    public void Initialize(float maxLifeTime)
    {
        _maxLifeTime = maxLifeTime;
    }

    public void Activate()
    {
        _activated = true;
    }

    public override void Update(float deltaTime)
    {
        if (_activated)
        {
            UpdateLifeTime(deltaTime);
        }
    }

    private void UpdateLifeTime(float deltaTime)
    {
        _currentLifeTime += deltaTime;
        if (_currentLifeTime < _maxLifeTime)
        {
            return;
        }

        _activated = false;
        GameObject.Destroy();
    }
}