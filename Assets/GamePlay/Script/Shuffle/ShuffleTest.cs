using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SystemShuffle;

public class ShuffleTest : MonoBehaviour
{
    public Button[] btn = new Button[4];

    private List<Vector2> btnk = new List<Vector2>();

    // Start is called before the first frame update
    void Start()
    {
        btnk.Add(new Vector2());
        btnk.Add(new Vector2());
        btnk.Add(new Vector2());
        btnk.Add(new Vector2());

        btn[0].transform.position = btnk[1];
        btn[1].transform.position = btnk[2];
        btn[2].transform.position = btnk[3];
        btn[3].transform.position = btnk[4];


    }

    public void Shuffle()
    {
        List.shuffle(btnk);
        while (true)
        {
            List.shuffle(btnk);

            if (btnk[0] != btnk[1] && btnk[0] != btnk[2] && btnk[0] != btnk[3] && btnk[1] != btnk[2] && btnk[1] != btnk[3] && btnk[2] != btnk[3])
            {
                break;
            }

        }
       
        btn[0].transform.position = btnk[0];
        btn[1].transform.position = btnk[1];
        btn[2].transform.position = btnk[2];
        btn[3].transform.position = btnk[3];

    }

}
