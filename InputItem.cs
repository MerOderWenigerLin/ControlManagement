using UnityEngine;

public enum InputType { Invalid, Key, Joystick, Axis };

public class InputItem
{
    public InputType inputType;
    public string inputName;
    public string inputValue;
    public KeyCode keyCode;
    
    public InputItem(InputType inputType, KeyCode keyCode, string inputValue)
    {
        this.inputType = inputType;
        this.keyCode = keyCode;
        this.inputName = keyCode.ToString();
        this.inputValue = inputValue;
    }

    public InputItem(InputType inputType, string inputName, string inputValue)
    {
        this.inputType = inputType;
        this.inputName = inputName;
        this.inputValue = inputValue;
    }

    public string getAxisName()
    {
        return inputName.Substring(0, inputName.IndexOf("["));
    }
}
