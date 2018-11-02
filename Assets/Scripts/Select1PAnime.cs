using UnityEngine;
using UnityEngine.UI;


public class Select1PAnime : MonoBehaviour {


    private int state;
    public static  bool ready;
    public Animator Select1P,Kenshi1P,Mahou1P,Souryo1P;
    
    public Text textA,textB;
    public GameObject Camera1;
	// Use this for initialization
	void Start () {
        state = 0;
        ready = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.RightArrow) && state != 0 && ready == false)
        {
            
            state++;
            if (state >= 4)
            {
                state = 1;
            }
            Select1P.SetInteger("State", state);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && state != 0 && ready == false) //PlayerInput.PlayerInputs[0].GetAxis(EAxis.X) == -1
        {
            
            state--;
            if (state <= 0)
            {
                state = 3;
            }
            Select1P.SetInteger("State", state);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (state == 0)
            {
                state = 1;
               Select1P.SetInteger("State",state);
            }
            else if (state != 0)
            {
                ready = true;
                Kenshi1P.SetBool("Ready", ready);
                Mahou1P.SetBool("Ready", ready);
                Souryo1P.SetBool("Ready", ready);
            }
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (ready == false)
            {
                state = 0;
                Select1P.SetInteger("State", state);
            }
            else
            {
                ready = false;
                Kenshi1P.SetBool("Ready", ready);
                Mahou1P.SetBool("Ready", ready);
                Souryo1P.SetBool("Ready", ready);
            }
        }

        switch (state)
        {
            case 0:
                textA.text = "ボタンを押してください";
                textB.text = "";
                break;

            case 1:
                textA.text = "1P";
                textB.text = "剣士";                
                break;

            case 2:
                textA.text = "1P";
                textB.text = "魔法使い";
                break;
            case 3:
                textA.text = "1P";
                textB.text = "僧侶";
                break;
        }
        
    }
}
