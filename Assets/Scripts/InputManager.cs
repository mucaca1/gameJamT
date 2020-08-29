using System;
using UnityEngine;

public class InputManager : MonoBehaviour 
{

    public enum InputAlternative : int
    {
        One = 1,
        Two = 2
    }
    
    public static event Action<bool> onFire;

    [Tooltip("Current joystick input")]
    private Vector2 joystick = new Vector2();

    // Button bool states representing their current state
    private bool actionButton;
    private bool fireButton;
    private bool menuButton;

    [HideInInspector] public InputAlternative inputAlternative = InputAlternative.One;

    private void OnDisable() 
    {
        joystick *= 0;
        actionButton = false;
        fireButton = false;
        menuButton = false;    
    }

    // Captures current inputs from Unity's InputManager
    void FixedUpdate() 
    {
        joystick.x = Input.GetAxisRaw("Horizontal" + (int)inputAlternative);
        joystick.y = Input.GetAxisRaw("Vertical" + (int)inputAlternative);
        
        actionButton = Input.GetKeyDown("joystick " + (int)inputAlternative + " button 0") || (InputAlternative.One == inputAlternative ? Input.GetKeyDown("e"): Input.GetKeyDown(","));
        fireButton = Input.GetKeyDown("joystick " + (int)inputAlternative + " button 1") || (InputAlternative.One == inputAlternative ? Input.GetKeyDown("f"): Input.GetKeyDown("."));
        menuButton = Input.GetKeyDown("joystick " + (int)inputAlternative + " button 6")  || (InputAlternative.One == inputAlternative ? Input.GetKeyDown("o"): Input.GetKeyDown("p"));
        
        if (fireButton)
            onFire.Invoke(true);
    }

    private void Update()
    {
        
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