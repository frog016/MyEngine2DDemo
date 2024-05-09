using MyEngine2D.Core.Input;
using SharpDX.DirectInput;

namespace MyEngine2DDemo;

public class MoveForwardInputAction : KeyboardInputAction
{
    protected override Key TriggeredKey => Key.W;
}