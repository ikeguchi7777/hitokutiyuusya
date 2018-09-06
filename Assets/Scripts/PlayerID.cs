using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerType
{
    None,
    Swordswoman,
    Witch,
    Healer
}
public class PlayerID : Singleton<PlayerID>
{
    public PlayerType[] PlayerTypes { get; private set; }
    public void Init()
    {
        PlayerTypes = new PlayerType[4];
        for (int i = 0; i < PlayerTypes.Length; i++)
            PlayerTypes[i] = PlayerType.None;
    }
}
