using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranking {

    public List<ScoreData> ScoreRanking;

    private static Ranking instance;

    public static Ranking Instance
    {
        get
        {
            if (instance == null)
                instance = SaveData.GetClass("rankdata", new Ranking());
            return instance;
        }
    }

    public Ranking()
    {
        ScoreRanking = new List<ScoreData>();
    }
	
    public void Save()
    {
        SaveData.SetList("scoredata", ScoreRanking);
    }

    public void Load()
    {
        instance.ScoreRanking = SaveData.GetList("scoredata", new List<ScoreData>());
    }

    public void AddScore(string name,int score)
    {
        ScoreRanking.Add(new ScoreData(name, score));
        ScoreRanking.Sort();
    }

    [Serializable]
    public class ScoreData : IComparable<ScoreData>
    {
        public ScoreData(string name, int score)
        {
            this.name = name;
            this.score = score;
        }
        public string name;
        public int score;

        public override string ToString()
        {
            return name + "," + score;
        }

        public int CompareTo(ScoreData obj)
        {
            return (score >= obj.score ? -1 : 1);
        }
    }
}