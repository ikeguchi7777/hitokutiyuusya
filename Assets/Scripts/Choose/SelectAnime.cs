using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SelectAnime : MonoBehaviour
{

    public GameObject ReadyText;
    private int state,joincount,readycount;
    public static bool[] ready = new bool[4];
    public static bool[] join = new bool[4];
    public static int[] type = new int[4];

    public Animator Select, Kenshi, Mahou, Souryo;
    [SerializeField] private int _id;

    public Text textA, textB,textC;

    // Use this for initialization
    void Start()
    {
        PlayerID.Instance.Init();

        textA.text = "ボタンを押してください";
        textB.text = "";
        textC.text = "";
        joincount = 0;        
        readycount = 0;

        ReadyText.SetActive(false);
        state = 0;

        for (int i = 0; i < 4; i++)
        {
            ready[i] = false;
            join[i] = false;
            type[i] = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {

        
        var t = PlayerInput.PlayerInputs[_id].GetAxisPulse(EAxis.X);



        if (t == 1 && join[_id] == true && ready[_id] == false)
        {
          //  Debug.Log(t);
            state++;

            if (state >= 4)
            {
                state = 1;
            }
            Select.SetInteger("State", state);
        }
        if (t == -1 && join[_id] == true && ready[_id] == false)
        {
            Debug.Log(t);
            state--;
            if (state <= 0)
            {
                state = 3;
            }
            Select.SetInteger("State", state);
        }

        if (join[_id] == true && ready[_id] == false)
        {
            switch (state)
            {

                case 1:
                    textB.text = "剣士";
                    break;
                case 2:
                    textB.text = "魔法使い";
                    break;
                case 3:
                    textB.text = "僧侶";
                    break;
            }
        }


        if (PlayerInput.PlayerInputs[_id].GetButtonDown(EButton.WeakAttackAndSubmit))
        {
            if (join[_id] == false)
            {
                join[_id] = true;
                Select.SetBool("join", true);
                joincount ++;

                textA.text = "";
                textC.text = (_id + 1) + "P";
                state = 1;


            }
            else if (join[_id] == true)
            {
                ready[_id] = true;
                Kenshi.SetBool("Ready", ready[_id]);
                Mahou.SetBool("Ready", ready[_id]);
                Souryo.SetBool("Ready", ready[_id]);
                type[_id] = state;
                for (int i = 0; i < 4; i++)
                {
                    
                   // Debug.Log((PlayerType)type[i]);
                }

            }

            if (ready[_id] == true && ReadyText.activeSelf == true)
            {

                for (int i = 0; i < 4; i++)
                {
                    PlayerID.Instance.PlayerTypes[i] = (PlayerType)type[i];
                    Debug.Log(PlayerID.Instance.PlayerTypes[i]);

                }

               SceneManager.LoadScene("GameMain");


            }


            
            Change();
        }
        if (PlayerInput.PlayerInputs[_id].GetButtonDown(EButton.Evade))
        {
            if (ready[_id] == true)
            {
                ready[_id] = false;
                Kenshi.SetBool("Ready", ready[_id]);
                Mahou.SetBool("Ready", ready[_id]);
                Souryo.SetBool("Ready", ready[_id]);
                
            }
            else if(ready[_id] == false && join[_id] == true)
            {
                join[_id] = false;
                Select.SetBool("join",false);
                joincount--;

                textA.text = "ボタンを押してください";
                textB.text = "";
                textC.text = "";
                state = 0;
            }

            Change();
        }
                   



        ReadyText.transform.localScale = new Vector2(1 + Mathf.Pow(Mathf.Cos(Mathf.PI * Time.time), 2) * 0.3f, 1 + Mathf.Pow(Mathf.Cos(Mathf.PI * Time.time), 2) * 0.3f);

    }

    public void Change()
    {
        ReadyText.SetActive(false);
        readycount = 0;
        joincount = 0;
        for (int i = 0; i < 4; i++)
        {

            if (ready[i] == true)
            {              
                readycount++;
            }

            if (join[i] == true)
            {
                joincount++;
            }
                     
        }

        
     
        if (readycount == joincount && joincount != 0)
        {
            ReadyText.SetActive(true);

            
        }

        

    }

}
