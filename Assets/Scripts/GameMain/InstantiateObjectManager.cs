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
            InstantiatePlayer(i + 1, PlayerID.Instance.PlayerTypes[i]);
        }
        EnemyList = new List<LockOnable>();
        InstantiateEnemy();
    }

    void InstantiatePlayer(int id, PlayerType type)
    {
        GameObject spawnobj = null;
        switch (type)
        {
            case PlayerType.None:
                break;
            case PlayerType.Swordswoman:
                break;
            case PlayerType.Witch:
                spawnobj = Instantiate(spawnObjects[(int)type - 1], spawnPos[id - 1].position, spawnPos[id - 1].rotation);
                spawnobj.GetComponent<WitchUserControl>().setParams(id);
                break;
            case PlayerType.Healer:
                break;
        }
        if (spawnobj != null)
        {
            var camera = Instantiate(Camera).GetComponent<Camera>();
            camera.tag = id + "P";
            camera.rect = SetCam(counter, playernum);
            counter++;
            playerList.Add(spawnobj.GetComponent<PlayerMover>());
            playerList[id - 1].setCamera(camera.GetComponent<PlayerCameraControl>(), -90.0f * (id - 1));
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
        var width = length < 2 ? 1.000f : 0.500f;
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
}
