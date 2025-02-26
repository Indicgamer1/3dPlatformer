using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Platformer/InputReader")]
public class InputReader : ScriptableObject, PlayerInputAction.IPlayerActions
{
    public UnityAction<Vector2> Move;
    public UnityAction<Vector2, bool> Look;
    public UnityAction EnableMouseControlCamera;
    public UnityAction DisableMouseControlCamera;
    public UnityAction<bool> Fly;

    PlayerInputAction inputActions;

    public Vector3 Direction => inputActions.Player.Move.ReadValue<Vector2>();
    public Vector3 MouseMovement => inputActions.Player.Look.ReadValue<Vector2>();

    private void OnEnable()
    {
        if(inputActions == null)
        {
            inputActions = new PlayerInputAction();
            inputActions.Player.SetCallbacks(this);
        }
        
    }

    public void EnabelInputAction()
    {
        inputActions.Enable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Move?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Look?.Invoke(context.ReadValue<Vector2>(), IsDeviceMouse(context));
    }

    bool IsDeviceMouse(InputAction.CallbackContext context) => context.control.device.name == "Mouse";

    public void OnMouseControlCamera(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                EnableMouseControlCamera?.Invoke();
                break;
            case InputActionPhase.Canceled:
                DisableMouseControlCamera?.Invoke();
                break;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
            case InputActionPhase.Performed:
                Fly?.Invoke(true);
                break;

            case InputActionPhase.Canceled:
                Fly?.Invoke(false);
                break;
        }
    }
    
    public void OnRun(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        //no code
    }

    
}
