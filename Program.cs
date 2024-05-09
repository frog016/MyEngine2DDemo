using MyEngine2D.Core;
using MyEngine2D.Core.Input;
using MyEngine2D.Core.Physic;

namespace MyEngine2DDemo
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            var game = new GameBuilder()
                .WithConfiguredLevels(new SpaceLevelConfigurator())
                .WithInputActions(GetInputActions())
                .WithPhysicLayers(GetLayerCollisionSettings())
                .Build();

            game.Run();
        }

        private static InputActionBase[] GetInputActions()
        {
            return new InputActionBase[]
            {
                new MoveForwardInputAction(),
                new TurnLeftInputAction(),
                new TurnRightInputAction(),
                new ShootInputAction(),
            };
        }

        private static LayerCollisionSetting[] GetLayerCollisionSettings()
        {
            return new LayerCollisionSetting[]
            {
                new LayerCollisionSetting(ConcreteLayer.Default, ConcreteLayer.Default),

                new LayerCollisionSetting(Constants.BulletLayer, Constants.AsteroidLayer),
                new LayerCollisionSetting(Constants.AsteroidLayer, Constants.BulletLayer),

                new LayerCollisionSetting(Constants.ShipLayer, Constants.AsteroidLayer),
                new LayerCollisionSetting(Constants.AsteroidLayer, Constants.ShipLayer),
            };
        }
    }
}