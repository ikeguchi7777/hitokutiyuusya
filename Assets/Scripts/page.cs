using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class page : MonoBehaviour {



    [SerializeField] Material[] Lpages = new Material[4];
    [SerializeField] Material[] Rpages = new Material[4];

    //private Material[] mats;
    [SerializeField] Renderer[] pages;

    // private Renderer L, R;
    public GameObject paper;
    public GameObject book;

    private int[] t = new int[4];
    public Animator Ani;

    private bool isRunning;
    private int pagenum;
    private float fadetime;

    // Use this for initialization
    void Start () {

        pages[0].material = Lpages[0];
        pages[1].material = Rpages[0];
        // L = GetComponent<Renderer>();
        // R = GetComponent<Renderer>();

        //mats = GetComponent<Renderer>().materials;

        paper.SetActive(false);
        isRunning = false;
        pagenum = 0;
        fadetime = 1;
        
    }
	
	// Update is called once per frame
	void Update () {

        for (int i = 0; i < 4; i++)
        {
            t[i] = PlayerInput.PlayerInputs[i].GetAxisPulseName(EAxis.X);
                        
        }

        if (t[0] + t[1] + t[2] +t[3] < 0 && !isRunning)
        {
            if (pagenum > 0)
            {
                pagenum--;
                StartCoroutine("Backflick");

            }
        }

        if (t[0] + t[1] + t[2] + t[3] > 0 && !isRunning)
        {
            if (pagenum == 4)
            {
                SceneManager.LoadScene("Choose");
            }
            if (pagenum < 4)
            {

                pagenum++;
                StartCoroutine("Nextflick");
            }

          
        }
       
    }



    private IEnumerator Backflick()
    {

        // ReplaceMaterial(7,Lpages[0]);

        isRunning = true;



            Ani.SetTrigger("back1");
            paper.SetActive(true);
            yield return new WaitForSeconds(1.0f);

            pages[0].material = Lpages[pagenum];
            pages[1].material = Rpages[pagenum];

            yield return new WaitForSeconds(1.0f);

            Ani.SetTrigger("back2");

        yield return new WaitForSeconds(1.0f);
        paper.SetActive(false);
            isRunning = false;
        
    }

    private IEnumerator Nextflick()
    {

        isRunning = true;


        Ani.SetTrigger("next1");
        paper.SetActive(true);
        yield return new WaitForSeconds(1.0f);

        pages[0].material = Lpages[pagenum];
        pages[1].material = Rpages[pagenum];

        yield return new WaitForSeconds(1.0f);

        Ani.SetTrigger("next2");

        yield return new WaitForSeconds(1.0f);
        paper.SetActive(false);
        isRunning = false;
    
       
    }

    private void ReplaceMaterial(int index, Material mat)

    {

        Renderer renderer = book.GetComponent<Renderer>();

        Material[] mats = renderer.materials;

        if (index < 0 || mats.Length <= index) return;

        mats[index] = mat;

        renderer.materials = mats;

    }
}

