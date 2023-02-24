using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPanel : MonoBehaviour
{
    public bool isCorrect = false;
    public QuizManager quizManager;
    public GameObject QuizPanel;

    public void Answer()
    {
        if (isCorrect)
        {
            QuizPanel.SetActive(false);
            
        }
        else
        {
            QuizPanel.SetActive(false);
        }
    }
}