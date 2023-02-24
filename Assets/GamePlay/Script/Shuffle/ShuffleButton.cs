using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SystemShuffle;

public class ShuffleButton : MonoBehaviour
{
    public RectTransform btn0;
    public RectTransform btn1;
    public RectTransform btn2;
    public RectTransform btn3;

    private List<Vector2> btnk = new List<Vector2>();

    private void Start()
    {
        btnk.Add(new Vector2(-600, -250));
        btnk.Add(new Vector2(-200, -250));
        btnk.Add(new Vector2(200, -250));
        btnk.Add(new Vector2(600, -250));
    }

    public void start()
    {
       // float[] btnx = { -600, -200, 200, 600 };
       // Array.shuffle(btnx);

        

        List.shuffle(btnk);



        btn0 = GetComponentInChildren<RectTransform>();
        btn0.anchoredPosition = btnk[0];
        btn1 = GetComponentInChildren<RectTransform>();
        btn1.anchoredPosition = btnk[1];
        btn2 = GetComponentInChildren<RectTransform>();
        btn2.anchoredPosition = btnk[2];
        btn3 = GetComponentInChildren<RectTransform>();
        btn3.anchoredPosition = btnk[3];

        


        //transform.position = vec[rand]
    }

    public void Shuffle()
    {
        List.shuffle(btnk);
       


        
        btn0.anchoredPosition = btnk[0];
        
        btn1.anchoredPosition = btnk[1];
        
        btn2.anchoredPosition = btnk[2];
        
        btn3.anchoredPosition = btnk[3];

        Debug.Log(this);
    }

    
}
