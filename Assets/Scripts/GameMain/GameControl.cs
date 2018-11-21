using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;

public enum GameState
{
    CountDown,
    Playing,
    GameOver,
    GameClear
}
public class GameControl : StatefulObjectBase<GameControl, GameState>
{
    [SerializeField] Animation countdown;
    [SerializeField] float timeLimit = 300.0f;
    [SerializeField] GameObject GameOverPanel;
    PauseControl pauseControl;
    static Subject<float> timeSubject = new Subject<float>();

    public static Subject<float> RemainTime
    {
        get
        {
            return timeSubject;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        pauseControl = GetComponent<PauseControl>();
        RemainTime.OnNext(1.0f);
    }

    protected override GameState GetFirstState()
    {
        return GameState.CountDown;
    }

    protected override void StateListInit()
    {
        stateList.Add(new StateCountDown(this));
        stateList.Add(new StatePlaying(this));
        stateList.Add(new StateGameOver(this));
        stateList.Add(new StateGameClear(this));
    }

    class StateCountDown : State<GameControl>
    {
        float time;
        public StateCountDown(GameControl owner) : base(owner, GameState.CountDown) { }

        public override void Enter()
        {
            time = 0.0f;
            owner.countdown.gameObject.SetActive(true);
            owner.countdown.Play();
        }

        public override void Execute()
        {
            time += Time.deltaTime;
            if (time > owner.countdown.clip.length)
            {
                owner.ChangeState(GameState.Playing);
            }
        }

        public override void Exit()
        {
            InstantiateObjectManager.Instance.MoveCharactor();
        }
    }

    class StatePlaying : State<GameControl>
    {
        float time;
        public StatePlaying(GameControl owner) : base(owner, GameState.Playing)
        {
            time = 0.0f;
        }

        public override void Execute()
        {
            if (ScoreBoard.Instance.isWin)
                return;
            for (int i = 0; i < 4; i++)
            {
                if (PlayerInput.PlayerInputs[i].GetButtonDown(EButton.Select))
                {
                    if (PlayerID.Instance.PlayerTypes[i] != PlayerType.None)
                    {
                        owner.pauseControl.Pause(i);
                    }
                }
            }
            time += Time.deltaTime;
            if (InstantiateObjectManager.Instance.PlayerList.Count == 0)
                owner.ChangeState(GameState.GameOver);
            RemainTime.OnNext(1.0f - time / owner.timeLimit);
            if (time >= owner.timeLimit)
            {
                owner.ChangeState(GameState.GameOver);
            }
        }
        public override void Exit()
        {
            ScoreBoard.Instance.RemainTime = owner.timeLimit - time;
        }
    }

    class StateGameOver : State<GameControl>
    {
        public StateGameOver(GameControl owner) : base(owner, GameState.GameOver)
        {
        }

        public override void Enter()
        {
            owner.GameOverPanel.SetActive(true);
            owner.gameObject.GetComponent<IntroBGM>().PlayBgm("gameover");
        }

        public override void Execute()
        {
            if (Input.GetButtonDown("Submit"))
                SceneManager.LoadScene("Title");
        }
    }

    class StateGameClear : State<GameControl>
    {
        public StateGameClear(GameControl owner) : base(owner, GameState.GameClear)
        {
        }

        public override void Enter()
        {
            Ranking.Instance.Score = ScoreBoard.Instance.GetScore();
            owner.StartCoroutine(LoadResult());
        }
        IEnumerator LoadResult()
        {
            yield return new WaitForSeconds(1.0f);
            SceneManager.LoadScene("Result");
        }
    }
}
