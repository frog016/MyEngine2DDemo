using MyEngine2D.Core.Graphic;
using MyEngine2D.Core.Level;
using MyEngine2D.Core.Math;
using MyEngine2D.Core.Physic;
using MyEngine2D.Core.Resource;
using MyEngine2D.Core.Structure;

namespace MyEngine2DDemo;

public class SpaceLevelConfigurator : GameLevelConfigurator
{
    private const string LevelName = "SpaceLevel";
    private const string PlayerName = "Player_Ship";

    private readonly Vector2 _playerShapeSize = new Vector2(0.96f, 0.64f);

    protected override GameLevel CreateConfiguredLevel()
    {
        var level = new GameLevel(LevelName);

        CreatePlayerShip(level);
        CreateBackground(level);
        CreateCamera(level);

        return level;
    }

    private void CreatePlayerShip(GameLevel level)
    {
        var shipObject = level.Instantiate(PlayerName);

        var rigidBody = shipObject.AddComponent<RigidBody>();
        var shape = new RectanglePhysicShape(rigidBody, _playerShapeSize);
        rigidBody.Initialize(shape, Constants.DefaultMaterial, 1, ComputeMassMode.Manually, gravityScale: 0, layer: Constants.ShipLayer);

        var spriteRenderer = shipObject.AddComponent<SpriteRenderer>();
        var spriteLoadData = new SpriteLoadData("Ship/Ship_Idle.png");
        spriteRenderer.Initialize(spriteLoadData, layer: 1);

        var shipEngine = shipObject.AddComponent<ShipEngine>();
        shipEngine.Initialize(4, 8, Math2D.PI / 2);

        shipObject.AddComponent<ObjectScreenCycleWrapper>();

        var shipCannon = shipObject.AddComponent<ShipCannon>();
        shipCannon.Initialize(new Vector2(0.5f, 0), 0.25f);

        var ship = shipObject.AddComponent<Ship>();
        ship.Initialize(1);
    }

    private static void CreateBackground(GameLevel level)
    {
        var background = level.Instantiate("Background");

        var spriteRenderer = background.AddComponent<SpriteRenderer>();
        var spriteLoadData = new SpriteLoadData("Space_Background.png");
        spriteRenderer.Initialize(spriteLoadData, layer: -1);

        var spawner = background.AddComponent<AsteroidSpawner>();
        spawner.Initialize(new Vector2(1, 1), 2);
    }

    private static void CreateCamera(GameLevel level)
    {
        const string defaultName = "Camera";
        const float orthoSize = 10;

        var cameraObject = level.Instantiate(defaultName, Vector2.Zero);
        var camera = cameraObject.AddComponent<Camera>();
        camera.Initialize(orthoSize);
    }
}