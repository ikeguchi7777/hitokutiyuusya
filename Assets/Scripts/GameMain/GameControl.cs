using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    CountDown,
    Playing,
}
public class GameControl : StatefulObjectBase<GameControl, GameState>
{
    [SerializeField] Animation countdown;
    [SerializeField] float timeLimit = 300.0f;

    PauseControl pauseControl;

    protected override void Awake()
    {
        base.Awake();
        pauseControl = GetComponent<PauseControl>();
    }

    protected override GameState GetFirstState()
    {
        return GameState.CountDown;
    }

    protected override void StateListInit()
    {
        stateList.Add(new StateCountDown(this));
        stateList.Add(new StatePlaying(this));
    }

    class StateCountDown : State<GameControl>
    {
        float time;
        public StateCountDown(GameControl owner) : base(owner, GameState.CountDown) { }

        public override void Enter()
        {
            time = 0.0f;
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
            if (time >= owner.timeLimit)
            {
                Debug.Log("時間切れ");
            }
        }
    }
}
