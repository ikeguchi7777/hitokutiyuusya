using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateObjectManager : SingletonObject<InstantiateObjectManager>
{
    [SerializeField] private bool isDebug;
    [SerializeField] private Transform[] spawnPos;
    [SerializeField] private GameObject[] spawnObjects;
    [SerializeField] private GameObject Camera;
    [SerializeField] private GameObject Enemy;
    [SerializeField] private BossControl boss;

    List<MonoBehaviour> pauseScripts = new List<MonoBehaviour>();
    int playernum, counter;
    public List<LockOnable> EnemyList { get; set; }
    private List<PlayerMover> playerList = new List<PlayerMover>();

    protected override void Awake()
    {
        base.Awake();
        if (isDebug)
        {
            PlayerID.Instance.Init();
            PlayerID.Instance.PlayerTypes[0] = PlayerType.Witch;
            //PlayerID.Instance.PlayerTypes[1] = PlayerType.Witch;
            //PlayerID.Instance.PlayerTypes[2] = PlayerType.Witch;
            //PlayerID.Instance.PlayerTypes[3] = PlayerType.Witch;
        }
        playernum = 0;
        counter = 0;
        foreach (var type in PlayerID.Instance.PlayerTypes)
        {
            if (type != PlayerType.None)
                playernum++;
        }
        for (int i = 0; i < 4; i++)
        {
            InstantiatePlayer(i, PlayerID.Instance.PlayerTypes[i]);
        }
        EnemyList = new List<LockOnable>();
        InstantiateEnemy();
    }

    public void MoveCharactor()
    {
        foreach (var player in playerList)
        {
            player.gameObject.SendMessage("ChangeState", PlayerState.Moveable);
        }

        boss.gameObject.SendMessage("ChangeState", BossState.Idle);
    }

    void InstantiatePlayer(int id, PlayerType type)
    {
        if (type == PlayerType.None)
            return;
        GameObject spawnobj = Instantiate(spawnObjects[(int)type - 1], spawnPos[id].position, spawnPos[id].rotation);
        if (spawnobj != null)
        {
            var camera = Instantiate(Camera, spawnobj.transform).GetComponent<Camera>();
            camera.tag = (id + 1) + "P";
            camera.rect = SetCam(counter, playernum);
            counter++;
            playerList.Add(spawnobj.GetComponent<PlayerMover>());
            playerList[id].setCamera(camera.GetComponent<PlayerCameraControl>(), -90.0f * id);
        }
    }

    public Rect SetCam(int num, int length)
    {
        float x = 0.000f;
        float y = 0.000f;
        if ((num <= 1 && length > 2) || (num == 0 && length == 2))
            y = 0.500f;
        if ((num == 1 || num == 3) && length != 2)
            x = 0.500f;
        var width = length <= 2 ? 1.000f : 0.500f;
        var height = length == 1 ? 1.000f : 0.500f;
        return new Rect(x, y, width, height);
    }

    void InstantiateEnemy()
    {
        Instantiate(Enemy);
    }

    public void RemoveEnemy(LockOnable enemy)
    {
        EnemyList.Remove(enemy);
        foreach (var player in playerList)
        {
            if (player.LockOnObject == enemy.TargetTransform)
            {
                player.LockOn();
            }
        }
    }

    public void Pause()
    {
        foreach (var player in playerList)
        {
            foreach (var script in player.GetComponents<MonoBehaviour>())
            {
                if (script.enabled)
                {
                    pauseScripts.Add(script);
                    script.enabled = false;
                }
            }
        }
    }

    public void Resume()
    {
        foreach (var script in pauseScripts)
        {
            script.enabled = true;
        }
        pauseScripts.Clear();
    }
}
