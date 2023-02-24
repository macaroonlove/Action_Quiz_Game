using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerCountdown : MonoBehaviour
{
    [SerializeField] float timeStart = 10.0f;
    [SerializeField] Text countdownText;

    

    void Start()
    {
        countdownText.text = timeStart.ToString();     
    }

    void Update()
    {
        if (timeStart > 0)
            timeStart -= Time.deltaTime;
        else if (timeStart <= 0)
            Time.timeScale = 0.0f;

        countdownText.text = Mathf.Round(timeStart).ToString();
    }

    public void Answer1Btn()
    {
        if(timeStart >= 0)
        {
            timeStart = 10f;
            countdownText.text = timeStart.ToString();
        }
    }
    

    public void Answer2Btn()
    {
        if (timeStart >= 0)
        {
            timeStart = 10f;
            countdownText.text = timeStart.ToString();
        }
    }


    public void Answer3Btn()
    {
        if (timeStart >= 0)
        {
            timeStart = 10f;
            countdownText.text = timeStart.ToString();
        }
    }


    public void Answer4Btn()
    {
        if (timeStart >= 0)
        {
            timeStart = 10f;
            countdownText.text = timeStart.ToString();
        }

    }

    public void High()
    {
        if (timeStart >= 0)
        {
            timeStart = 10f;
            countdownText.text = timeStart.ToString();
        }

    }
    public void Middle()
    {
        if (timeStart >= 0)
        {
            timeStart = 10f;
            countdownText.text = timeStart.ToString();
        }

    }
    public void Low()
    {
        if (timeStart >= 0)
        {
            timeStart = 10f;
            countdownText.text = timeStart.ToString();
        }

    }

    public void Restart()
    {
        if (timeStart >= 0)
        {
            timeStart = 10f;
            countdownText.text = timeStart.ToString();
        }
    }


}
