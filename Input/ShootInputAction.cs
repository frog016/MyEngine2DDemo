using MyEngine2D.Core.Input;
using SharpDX.DirectInput;

namespace MyEngine2DDemo;

public class ShootInputAction : KeyboardInputAction
{
    protected override Key TriggeredKey => Key.Space;
}