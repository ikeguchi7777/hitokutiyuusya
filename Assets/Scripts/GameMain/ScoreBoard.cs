using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoard : Singleton<ScoreBoard>
{
    int winBonus = 1500;
    int remainTimeBonus = 5;
    int noDamageBonus = 2000;
    int remainHPBonus = 2000;

    int playerNum = 0;
    public bool isWin { get; set; }
    public float RemainTime { get; set; }
    public bool[] isNoDamage { get; set; }
    public int[] RemainHP { get; set; }
    public void Init(int playernum)
    {
        playerNum = playernum;
        isNoDamage = new bool[4];
        RemainHP = new int[4];
        for (int i = 0; i < 4; i++)
        {
            if (PlayerID.Instance.PlayerTypes[i] != PlayerType.None)
                isNoDamage[i] = true;
            else
                isNoDamage[i] = false;
            RemainHP[i] = 0;
        }
    }

    public int GetScore()
    {
        return (isWin ? 1500 : 0) + (int)RemainTime * remainTimeBonus + GetRemainHPBonus() + GetNoDamageBonus();
    }

    int GetRemainHPBonus()
    {
        int sum = 0;
        foreach (var hp in RemainHP)
        {
            sum += hp;
        }
        if (sum == 0)
            return 0;
        return remainHPBonus * (100 * playerNum / sum);
    }

    int GetNoDamageBonus()
    {
        int count = 0;
        foreach (var flag in isNoDamage)
        {
            if (flag)
                count++;
        }
        return noDamageBonus / playerNum * count;
    }
}
