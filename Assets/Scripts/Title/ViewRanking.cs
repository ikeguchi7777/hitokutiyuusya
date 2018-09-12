using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewRanking : MonoBehaviour {
    [SerializeField] ScorePanel panel;
    private void Awake()
    {
        Ranking.Instance.Load();
        foreach (var item in Ranking.Instance.ScoreRanking)
        {
            panel.Set(item.name, item.score);
            Instantiate(panel, transform);
        }
    }
}
