using System;
using System.Collections.Generic;
using UnityEngine;


public class ControlManager
{
    private bool acceptInput = true;
    private static ControlManager _instance;
    private List<Axis> axis;
    private List<ControlBinding> controlBindings;
    private const string joystickPrefix = "joystick button ";

    public ControlManager()
    {
        axis = new List<Axis>();
        controlBindings = new List<ControlBinding>();
        initiateControls();
    }

    private float getAxisInput(string axisName)
    {
        //Debug.Log(axisName);
        //float axisValue = Input.GetAxisRaw(axisName);
        float axisValue = 0;
        if (axisValue == -1)
            return -1;
        else if (axisValue == 1)
            return 1;
        return 0;
    }

    private void initiateControls()
    {
        axis.Clear();
        for (int i = 0; i < 20; i++)
            axis.Add(new Axis("Axis " + i, getAxisInput("Axis " + i)));
    }

    public static ControlManager getInstance()
    {
        if (_instance == null)
            _instance = new ControlManager();
        return _instance;
    }

    public void borrowBindings(ControlManager controlManager)
    {
        for (int i = 0; i < controlManager.controlBindings.Count; i++)
        {
            ControlBinding binding = controlManager.controlBindings[i];
            setControlBinding(binding.inputItem, binding.control);
        }
    }

    public void clearBindings()
    {
        controlBindings.Clear();
    }

    public InputItem getNextInput()
    {
        for (int i = 0; i < 20; i++)
        {
            if (Input.GetKeyDown(joystickPrefix + i))
                return new InputItem(InputType.Joystick, joystickPrefix + i, joystickPrefix + i);
        }

        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(kcode))
                return new InputItem(InputType.Key, kcode, kcode.ToString());
        }

        for (int i = 0; i < axis.Count; i++)
        {
            if (getAxisInput(axis[i].axisName) != axis[i].axisDefaultValue)
            {
                string axisAsString = getAxisInput(axis[i].axisName).ToString();
                return new InputItem(InputType.Axis, axis[i].axisName + "[" + axisAsString + "]", axisAsString);
            }
        }

        return new InputItem(InputType.Invalid, "", "");
    }

    public ControlBinding getControlBinding(Control control)
    {
        for (int i = 0; i < controlBindings.Count; i++)
        {
            if (controlBindings[i].control.index == control.index)
                return controlBindings[i];
        }

        InputItem inputItem = new InputItem(InputType.Key, control.defaultKey, control.defaultKey.ToString());
        controlBindings.Add(new ControlBinding(inputItem, control));
        return controlBindings[controlBindings.Count - 1];
    }

    public ControlBinding getControlBinding(InputItem inputItem)
    {
        for (int i = 0; i < controlBindings.Count; i++)
        {
            if (controlBindings[i].inputItem.inputName == inputItem.inputName)
                return controlBindings[i];
        }
        return null;
    }

    public void setControlBinding(InputItem inputItem, Control control)
    {
        ControlBinding controlJobItem = getControlBinding(control);

        if (controlJobItem != null)
        {
            controlJobItem.inputItem = inputItem;
            return;
        }
        controlBindings.Add(new ControlBinding(inputItem, control));
    }

    public void disableInput()
    {
        acceptInput = false;
    }

    public void enableInput()
    {
        acceptInput = true;
    }

    public bool controlIsPressed(Control control)
    {
        if (!acceptInput)
            return false;

        InputItem input = getControlBinding(control).inputItem;

        if (input.inputType == InputType.Key)
            return Input.GetKey(input.keyCode);
        else if (input.inputType == InputType.Joystick)
            return Input.GetKey(input.inputName);
        else if (input.inputType == InputType.Axis)
            return getAxisInput(input.getAxisName()).ToString() == input.inputValue;

        return false;
    }

}
