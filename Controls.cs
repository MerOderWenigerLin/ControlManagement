using UnityEngine;
using System.Collections.Generic;

public enum Controls { MoveLeft, MoveRight, Jump, Options };

public sealed class Control
{
    private static List<Control> allControls;
    public readonly Controls index;
    public readonly string name;
    public readonly KeyCode defaultKey;

    public static readonly Control MoveLeft = new Control(Controls.MoveLeft, "Move Left", KeyCode.A);
    public static readonly Control MoveRight = new Control(Controls.MoveRight, "Move Right", KeyCode.D);
    public static readonly Control Jump = new Control(Controls.Jump, "Jump", KeyCode.Space);
    public static readonly Control Options = new Control(Controls.Options, "Options", KeyCode.Escape);
    public static int Count;

    private Control(Controls index, string name, KeyCode defaultKey)
    {
        if (Control.allControls == null)
            Control.allControls = new List<Control>();

        this.name = name;
        this.index = index;
        this.defaultKey = defaultKey;
        Control.allControls.Add(this);
        Control.Count = Control.allControls.Count;
    }

    public override string ToString()
    {
        return name;
    }

    public static Control getControl(Controls index)
    {
        for (int i = 0; i < allControls.Count; i++)
        {
            if(allControls[i].index == index)
                return allControls[i];
        }
        return null;
    }



}