using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    private PlayerInputActions inputAction;

    private void Awake() {

        inputAction = new PlayerInputActions();
        inputAction.Player.Enable();


#if UNITY_EDITOR

        inputAction.Cheat.Enable();
        inputAction.Cheat.ResetGameData.performed += ResetGameData_performed;
        inputAction.Cheat.CheatGold.performed += CheatGold_performed;
#endif

    }

    private void OnDestroy() {

#if UNITY_EDITOR
        inputAction.Cheat.ResetGameData.performed -= ResetGameData_performed;
        inputAction.Cheat.CheatGold.performed -= CheatGold_performed;
#endif

        inputAction.Player.Disable();
    }

#if UNITY_EDITOR
    private void ResetGameData_performed(InputAction.CallbackContext obj) {

        DataManager.ForceResetGame();
    }

    private void CheatGold_performed(InputAction.CallbackContext obj) {
        DataManager.UpdateGold(100000);
    }
#endif

    public Vector2 GetInputVectorNormalized() {
        return inputAction.Player.Move.ReadValue<Vector2>().normalized;
    }
}
