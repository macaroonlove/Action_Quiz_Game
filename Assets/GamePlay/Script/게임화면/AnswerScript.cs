using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerScript : MonoBehaviour
{
    public bool isCorrect = false;
    public static bool Correct = false;
    public QuizManager quizManager;
    
    public void Answer()
    {
        if(isCorrect)
        {
            Correct = true;
            quizManager.correct();
        }
        else
        {
            Correct = false;
            quizManager.correct();
        }
    }
}
