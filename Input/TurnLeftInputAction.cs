using MyEngine2D.Core.Input;
using SharpDX.DirectInput;

namespace MyEngine2DDemo;

public class TurnLeftInputAction : KeyboardInputAction
{
    protected override Key TriggeredKey => Key.A;
}