using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput
{
    string[] axis;
    List<string> axislist = new List<string>();
    List<string> buttonlist = new List<string>();
    string X;
    string Y;
    string CamX;
    string CamY;
    string Evade;
    string WeakAttack;
    string StrongAttack;
    string SpecialAttack;
    string Select;
    string LockOn;

    public static PlayerInput[] PlayerInputs =
    {
        new PlayerInput(1),
        new PlayerInput(2),
        new PlayerInput(3),
        new PlayerInput(4)
    };

    void SetAxis(Axis id, string name)
    {
        axislist.Insert((int)id, name);
    }

    void SetButton(Button id, string name)
    {
        buttonlist.Insert((int)id, name);
    }

    PlayerInput(int id)
    {
        SetAxis(Axis.X, "GamePad" + id + "_X");
        SetAxis(Axis.Y, "GamePad" + id + "_Y");
        SetAxis(Axis.CamX, "GamePad" + id + "_CamX");
        SetAxis(Axis.CamY, "GamePad" + id + "_CamY");
        SetButton(Button.Evade, "GamePad" + id + "_Evade and Cancel");
        SetButton(Button.WeakAttackAndSubmit, "GamePad" + id + "_WeakAttack and Submit");
        SetButton(Button.StrongAttack, "GamePad" + id + "_StrongAttack");
        SetButton(Button.SpecialAttack, "GamePad" + id + "_SpecialAttack");
        SetButton(Button.Select, "GamePad" + id + "_Select");
        SetButton(Button.LockOn, "GamePad" + id + "_LockOn");
    }

    float GetAxis(Axis axis)
    {
        return Input.GetAxis(axislist[(int)axis]);
    }

    float GetAxisRaw(Axis axis)
    {
        return Input.GetAxisRaw(axislist[(int)axis]);
    }

    bool GetButtonDown(Button button)
    {
        return Input.GetButtonDown(buttonlist[(int)button]);
    }
    bool GetButton(Button button)
    {
        return Input.GetButton(buttonlist[(int)button]);
    }
    bool GetButtonUp(Button button)
    {
        return Input.GetButtonUp(buttonlist[(int)button]);
    }
}

enum Axis
{
    X,
    Y,
    CamX,
    CamY
}

enum Button
{
    Evade,
    WeakAttackAndSubmit,
    StrongAttack,
    SpecialAttack,
    Select,
    LockOn
}