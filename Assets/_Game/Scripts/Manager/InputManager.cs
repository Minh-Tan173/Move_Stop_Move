using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    private PlayerInputActions inputAction;

    private void Awake() {

        inputAction = new PlayerInputActions();
        inputAction.Player.Enable();
    }

    public Vector2 GetInputVectorNormalized() {
        return inputAction.Player.Move.ReadValue<Vector2>().normalized;
    }
}
