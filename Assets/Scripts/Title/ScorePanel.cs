using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePanel : MonoBehaviour {
    [SerializeField] private Text nameText;
    [SerializeField] private Text scoreText;

    public void Set(string name,int score)
    {
        nameText.text = name;
        scoreText.text = score.ToString();
    }
}
