using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

public class GamePadCursor : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private RectTransform cursorTransform;
    [SerializeField] private float cursorSpeed = 1000;
    [SerializeField] private RectTransform canvasRecTransform;
    [SerializeField] private Canvas canvas;

    [SerializeField] float padding = 35f;


    private Mouse virtualMouse;
    private bool previousMouseState;
    private Camera mainCamera;
    private Mouse currentMouse;

    private string previousControlScheme = "";
    private string gamePadScheme = "Gamepad";
    private string mouseScheme = "Keyboard&Mouse";

    private void OnEnable()
    {

        mainCamera = Camera.main;
        currentMouse = Mouse.current;
        //Initialize virtual mouse
        if (virtualMouse == null)
        {
            virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
        }
        else if (!virtualMouse.added)
        {
            InputSystem.AddDevice(virtualMouse);
        }

        InputUser.PerformPairingWithDevice(virtualMouse, playerInput.user);

        if (cursorTransform != null)
        {
            Vector2 position = cursorTransform.anchoredPosition;
            InputState.Change(virtualMouse.position, position);
        }

        InputSystem.onAfterUpdate += UpdateMotion;
        // playerInput.onControlsChanged += OnControlsChanged;
    }

    private void OnDisable()
    {
        if (virtualMouse != null && virtualMouse.added)
        {
            InputSystem.RemoveDevice(virtualMouse);
        }
        InputSystem.onAfterUpdate -= UpdateMotion;
        //  playerInput.onControlsChanged -= OnControlsChanged;
    }

    private void UpdateMotion()
    {
        if (virtualMouse == null || Gamepad.current == null)
        {
            return;
        }

        Vector2 deltaValue = Gamepad.current.rightStick.ReadValue();
        deltaValue *= cursorSpeed * Time.deltaTime;

        Vector2 currentPosition = virtualMouse.position.ReadValue();
        Vector2 newPosition = currentPosition + deltaValue;

        newPosition.x = Mathf.Clamp(newPosition.x, padding, Screen.width - padding);
        newPosition.y = Mathf.Clamp(newPosition.y, padding, Screen.height - padding);
        //Cursor.lockState = CursorLockMode.Confined;
        InputState.Change(virtualMouse.position, newPosition);
        InputState.Change(virtualMouse.delta, deltaValue);

        //Cheching button inputs
        bool aButtonIsPressed = Gamepad.current.aButton.IsPressed();

        if (previousMouseState != aButtonIsPressed)
        {
            virtualMouse.CopyState<MouseState>(out var mouseState);
            mouseState.WithButton(MouseButton.Left, aButtonIsPressed);
            InputState.Change(virtualMouse, mouseState);
            previousMouseState = aButtonIsPressed;
        }

        AnchoCursor(newPosition);
    }

    private void AnchoCursor(Vector2 _newPos)
    {
        Vector2 anchoredPosition = Vector2.zero;

        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            mainCamera = null;
        }

        // Pass the RectTransform of the canvas to the function.
        if (Gamepad.current.rightStick.ReadValue() != Vector2.zero)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRecTransform, _newPos, mainCamera, out anchoredPosition);
            cursorTransform.anchoredPosition = anchoredPosition;
        }
    }

    public void OnControlsChanged(PlayerInput _playerInput)
    {
        if (_playerInput.currentControlScheme == mouseScheme && previousControlScheme != mouseScheme)
        {
            cursorTransform.gameObject.SetActive(false);
            Cursor.visible = true;
            currentMouse.WarpCursorPosition(virtualMouse.position.ReadValue());
            previousControlScheme = mouseScheme;

        }
        else if (_playerInput.currentControlScheme == gamePadScheme && previousControlScheme != gamePadScheme)
        {
            cursorTransform.gameObject.SetActive(true);
            Cursor.visible = false;
            InputState.Change(virtualMouse.position, currentMouse.position.ReadValue());
            AnchoCursor(currentMouse.position.ReadValue());
            previousControlScheme = gamePadScheme;
        }
    }

    //private void Update()
    //{
    //    if(previousControlScheme != playerInput.currentControlScheme)
    //    {
    //        OnControlsChanged(playerInput);
    //    }
    //    previousControlScheme = playerInput.currentControlScheme;
    //}
}


/*
 * enable and use new input system
 https://www.youtube.com/watch?v=Yjee_e4fICc
 https://www.youtube.com/watch?v=Y3WNwl1ObC8&t=296s
 */