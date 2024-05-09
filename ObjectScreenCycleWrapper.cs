using MyEngine2D.Core;
using MyEngine2D.Core.Entity;
using MyEngine2D.Core.Graphic;
using MyEngine2D.Core.Level;
using MyEngine2D.Core.Structure;
using MyEngine2D.Core.Utility;

namespace MyEngine2DDemo;

public class ObjectScreenCycleWrapper : Component
{
    private Camera _camera;
    private SpriteRenderer _spriteRenderer;

    public ObjectScreenCycleWrapper(GameObject gameObject) : base(gameObject)
    {
    }

    public override void Start()
    {
        var levelManager = ServiceLocator.Instance.Get<GameLevelManager>();

        _camera = levelManager.CurrentLevel.GameObjects.FindByType<Camera>();
        _spriteRenderer = GameObject.GetComponent<SpriteRenderer>();
    }

    public override void FixedUpdate(float fixedDeltaTime)
    {
        var cameraViewRect = _camera.GetViewRectangle();
        var spriteRect = _spriteRenderer.GetBoundingBox();

        if (cameraViewRect.Intersect(spriteRect) == false)
        {
            WrapObjectInCameraView(cameraViewRect);
        }
    }

    private void WrapObjectInCameraView(AxisAlignedBoundingBox viewBounds)
    {
        var objectPosition = Transform.Position;
        var wrappedPosition = Vector2.Zero;

        if (objectPosition.X > viewBounds.Max.X)
        {
            wrappedPosition = new Vector2(viewBounds.Min.X, objectPosition.Y);
        }
        else if (objectPosition.X < viewBounds.Min.X)
        {
            wrappedPosition = new Vector2(viewBounds.Max.X, objectPosition.Y);
        }
        else if (objectPosition.Y > viewBounds.Max.Y)
        {
            wrappedPosition = new Vector2(objectPosition.X, viewBounds.Min.Y);
        }
        else if (objectPosition.Y < viewBounds.Min.Y)
        {
            wrappedPosition = new Vector2(objectPosition.X, viewBounds.Max.Y);
        }

        Transform.Position = wrappedPosition;
    }
}