using MyEngine2D.Core.Entity;
using MyEngine2D.Core.Input;
using MyEngine2D.Core.Physic;
using MyEngine2D.Core.Structure;

namespace MyEngine2DDemo;

public class ShipEngine : Component
{
    private readonly InputSystem _inputSystem;

    private RigidBody _rigidBody;
    private float _thrustSpeed = 1f;
    private float _decelerateSpeed = 1f;
    private float _rotationSpeed = 0.1f;
    private bool _needThrusting;
    private float _turnDirection;
    private Vector2 _lastMoveVector;

    private InputActionBase _moveForwardInput;
    private InputActionBase _turnLeftInput;
    private InputActionBase _turnRightInput;

    public ShipEngine(GameObject gameObject, InputSystem inputSystem) : base(gameObject)
    {
        _inputSystem = inputSystem;
    }

    public void Initialize(float thrustSpeed, float decelerateSpeed, float rotationSpeed)
    {
        _thrustSpeed = thrustSpeed;
        _decelerateSpeed = decelerateSpeed;
        _rotationSpeed = rotationSpeed;
    }

    public override void Start()
    {
        _rigidBody = GameObject.GetComponent<RigidBody>();

        _moveForwardInput = _inputSystem.GetInputAction<MoveForwardInputAction>();
        _turnLeftInput = _inputSystem.GetInputAction<TurnLeftInputAction>();
        _turnRightInput = _inputSystem.GetInputAction<TurnRightInputAction>();
    }

    public override void Update(float deltaTime)
    {
        _needThrusting = _moveForwardInput.WasPressedThisFrame;
        _turnDirection = _turnLeftInput.WasPressedThisFrame ? 1 : _turnRightInput.WasPressedThisFrame ? -1 : 0;
    }

    public override void FixedUpdate(float fixedDeltaTime)
    {
        if (_needThrusting)
        {
            _lastMoveVector = Transform.GetForwardVector();
            _rigidBody.ApplyForce(_lastMoveVector * _thrustSpeed);
        }

        if (_turnDirection != 0f)
        {
            Transform.Rotation += _turnDirection * _rotationSpeed * fixedDeltaTime;
        }

        DecelerateEngine();
    }

    private void DecelerateEngine()
    {
        var rigidBodyVelocity = _rigidBody.LinearVelocity;
        if (rigidBodyVelocity.Length() == 0)
        {
            return;
        }

        var decelerateForce = _lastMoveVector.Normalize() * _decelerateSpeed;
        if (decelerateForce.Length() > rigidBodyVelocity.Length())
        {
            decelerateForce = rigidBodyVelocity;
        }

        _rigidBody.ApplyForce(-decelerateForce);
    }
}