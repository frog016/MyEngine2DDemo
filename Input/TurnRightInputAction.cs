using MyEngine2D.Core.Input;
using SharpDX.DirectInput;

namespace MyEngine2DDemo;

public class TurnRightInputAction : KeyboardInputAction
{
    protected override Key TriggeredKey => Key.D;
}