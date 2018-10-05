using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class name : MonoBehaviour {


    public GameObject flame;
    public Text nametext;
    private string tempnametext;
    private float selecttime,settime;
    private int flamex, flamey;
    private int inputcheck;
    private Vector2 homepos;
    private int[,] kanalocation = new int[4, 3];
    private string[,,] kana = new string[,,]
    { { { "あ", "い", "う", "え", "お", "ぁ", "ぃ", "ぅ", "ぇ", "ぉ" ,"0","","","","",""},
        { "か", "き", "く", "け", "こ", "が", "ぎ", "ぐ", "げ", "ご" ,"0","","","","",""},
        { "さ", "し", "す", "せ", "そ", "ざ", "じ", "ず", "ぜ", "ぞ" ,"0","","","","",""} },
      { {"た","ち","つ","て","と","だ","ぢ","づ","で","ど","っ","0","","","",""},
        {"な","に","ぬ","ね","の","0","","","","","","","","","" ,""},
        {"は","ひ","ふ","へ","ほ","ば","び","ぶ","べ","ぼ","ぱ","ぴ","ぷ","ぺ","ぽ","0" } },
      { { "ま","み","む","め","も","0","","","","","","","","","",""},
        {"や","ゐ","ゆ","ゑ","よ","ゃ","ゅ","ょ","0","","","","","","","" },
        {"ら","り","る","れ","ろ","0","","","","","","","","","",""}, },
      { { "","0","","","","","","","","","","","","","",""},
        {"わ","を","ん","ー","！","？","、","。","＆","0","","","","","","" },
        {"","0","","","","","","","","","","","","","",""} } };

    // Use this for initialization
    void Start () {
        flamex = 0;
        flamey = 0;
        selecttime = 0;
        settime = 1;
        inputcheck = 0;
        nametext.text = "";
        tempnametext = "";

        homepos = new Vector2(-65.5f, -23.5f);
        flame.transform.localPosition = homepos;

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                kanalocation[i, j] = 0;
            }
        }

        
	}

    // Update is called once per frame
    void Update() {



        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            flamey--;
            if (flamey < 0)
            {
                flamey = 3;
            }
            kanalocation[flamey, flamex] = 0;
            if (selecttime > 0)
            {
                selecttime = 0;
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            flamey++;
            if (flamey > 3)
            {
                flamey = 0;
            }
            kanalocation[flamey, flamex] = 0;
            if (selecttime > 0)
            {
                selecttime = 0;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            flamex--;
            if (flamex < 0)
            {
                flamex = 2;
            }
            kanalocation[flamey, flamex] = 0;
            if (selecttime > 0)
            {
                selecttime = 0;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            flamex++;
            if (flamex > 2)
            {
                flamex = 0;
            }
            kanalocation[flamey, flamex] = 0;
            if (selecttime > 0)
            {
                selecttime = 0;
            }
        }




        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (!(flamex == 2 && flamey == 3) && !(flamex == 0 && flamey == 3) && selecttime <= 0 && nametext.text.Length < 8)
            {
                selecttime = settime;
                inputcheck = 1;
            }
            else if (!(flamex == 2 && flamey == 3) && !(flamex == 0 && flamey == 3) && selecttime > 0)
            {
                selecttime = settime;
                kanalocation[flamey, flamex]++;
                if (kana[flamey, flamex, kanalocation[flamey, flamex]] == "0")
                {
                    kanalocation[flamey, flamex] = 0;
                }
            }
            else if (flamex == 2 && flamey == 3)
            {
                if (nametext.text.Length > 0)
                {
                    nametext.text = nametext.text.Remove(nametext.text.Length - 1, 1);
                    tempnametext = nametext.text;
                }
            }
            else if (flamex == 0 && flamey == 3)
            {
                Ranking.Instance.AddScore(nametext.text, 100);
                Ranking.Instance.Save();
            }
        }


        if (selecttime > 0)
        {
            selecttime -= Time.deltaTime;            
            nametext.text = tempnametext + kana[flamey, flamex, kanalocation[flamey, flamex]];
        }
        else if (selecttime <= 0)
        {
            if(kanalocation[flamey, flamex] != 0)
            {
                kanalocation[flamey, flamex] = 0;
            }

            if (inputcheck == 1)
            {
                tempnametext = nametext.text;
                inputcheck = 0;
            }
        }

        flame.transform.localPosition = homepos + new Vector2(Mathf.Clamp(flamex, 0, 2) * 65, Mathf.Clamp(flamey, 0, 3) * -49);
                
	}
}
