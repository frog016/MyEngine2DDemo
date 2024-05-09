using MyEngine2D.Core.Math;

namespace MyEngine2DDemo;

public class Cooldown
{
    public float CooldownTime { get; set; }

    private float _currentTime;
    private bool _isReady;

    public Cooldown(float cooldownTime)
    {
        CooldownTime = cooldownTime;
        _currentTime = 0;
        _isReady = true;
    }

    public bool TryRestart()
    {
        if (_isReady == false)
        {
            return false;
        }

        _isReady = false;
        return true;
    }

    public void Tick(float deltaTime)
    {
        if (_isReady)
        {
            return;
        }

        _currentTime = Math2D.Clamp(_currentTime + deltaTime, 0, CooldownTime);
        if (_currentTime == CooldownTime)
        {
            _isReady = true;
            _currentTime = 0;
        }
    }
}