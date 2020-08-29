using UnityEngine;

public class InputManager : MonoBehaviour 
{

    public enum InputAlternative : int
    {
        One = 1,
        Two = 2
    }

    [Tooltip("Current joystick input")]
    private Vector2 joystick = new Vector2();

    // Button bool states representing their current state
    private bool actionButton;
    private bool fireButton;
    private bool menuButton;

    [Tooltip("Which input to use")]
    public InputAlternative inputAlternative;

    // Captures current inputs from Unity's InputManager
    void FixedUpdate() 
    {
        joystick.x = Input.GetAxis("Horizontal" + (int)inputAlternative);
        joystick.y = Input.GetAxis("Vertical" + (int)inputAlternative);
        
        actionButton = Input.GetKeyDown("joystick " + (int)inputAlternative + " button 0");
        fireButton = Input.GetKeyDown("joystick " + (int)inputAlternative + " button 1");
        menuButton = Input.GetKeyDown("joystick " + (int)inputAlternative + " button 6");    
    }

    public Vector2 GetJoystickInput() {
        return joystick;
    }

    public bool GetActionButton() {
        return actionButton;
    }

    public bool GetFireButton() {
        return fireButton;
    }

    public bool GetMenuButton() {
        return menuButton;
    }
}