﻿using System.Collections;
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

    void SetAxis(EAxis id, string name)
    {
        axislist.Insert((int)id, name);
    }

    void SetButton(EButton id, string name)
    {
        buttonlist.Insert((int)id, name);
    }

    PlayerInput(int id)
    {
        SetAxis(EAxis.X, "GamePad" + id + "_X");
        SetAxis(EAxis.Y, "GamePad" + id + "_Y");
        SetAxis(EAxis.CamX, "GamePad" + id + "_CamX");
        SetAxis(EAxis.CamY, "GamePad" + id + "_CamY");
        SetButton(EButton.Evade, "GamePad" + id + "_Evade and Cancel");
        SetButton(EButton.WeakAttackAndSubmit, "GamePad" + id + "_WeakAttack and Submit");
        SetButton(EButton.StrongAttack, "GamePad" + id + "_StrongAttack");
        SetButton(EButton.SpecialAttack, "GamePad" + id + "_SpecialAttack");
        SetButton(EButton.Select, "GamePad" + id + "_Select");
        SetButton(EButton.LockOn, "GamePad" + id + "_LockOn");
    }

    public float GetAxis(EAxis axis)
    {
        return Input.GetAxis(axislist[(int)axis]);
    }

    public float GetAxisRaw(EAxis axis)
    {
        return Input.GetAxisRaw(axislist[(int)axis]);
    }

    public bool GetButtonDown(EButton button)
    {
        return Input.GetButtonDown(buttonlist[(int)button]);
    }
    public bool GetButton(EButton button)
    {
        return Input.GetButton(buttonlist[(int)button]);
    }
    public bool GetButtonUp(EButton button)
    {
        return Input.GetButtonUp(buttonlist[(int)button]);
    }

    public string GetButtonName(EButton button)
    {
        return buttonlist[(int)button];
    }

    public string GetAxisName(EAxis axis)
    {
        return axislist[(int)axis];
    }
}

public enum EAxis
{
    X,
    Y,
    CamX,
    CamY
}

public enum EButton
{
    Evade,
    WeakAttackAndSubmit,
    StrongAttack,
    SpecialAttack,
    Select,
    LockOn
}