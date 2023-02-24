using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackManager : MonoBehaviour
{
    [SerializeField] Text hpText;

    public Text Result;


    public string[] Choices;
    public Button High, Middle, Low;

    public int aiHP = 100;
    public int damage = 25;

    
    public GameObject AttackPanel;
    
    public GameObject AttackBtnHighO;
    public GameObject AttackBtnHighX;
    public GameObject AttackBtnMiddleO;
    public GameObject AttackBtnMiddleX;
    public GameObject AttackBtnLowO;
    public GameObject AttackBtnLowX;




    public void Play(string myChoice)
    {

        
        string randomChoice = Choices[Random.Range(0, Choices.Length)];

        switch (randomChoice)
        {
            case "High":
                switch (myChoice)
                {
                    case "High":
                        Result.text = "상대가 공격을 막았습니다!";
                        hpText.text = "Hp : " + aiHP;
                        StartCoroutine(Delay());
                        AttackBtnHighX.SetActive(true);
                        
                        break;
                    case "Middle":
                        Result.text = "당신은 공격을 성공하였습니다!";
                        aiHP = aiHP - damage;
                        hpText.text = "Hp : " + aiHP;
                        StartCoroutine(Delay());
                        AttackBtnHighO.SetActive(true);
                        if (aiHP == 0)
                        {
                            Result.text = "당신은 승리하셨습니다.";
                        }
    
                        break;
                    case "Low":
                        Result.text = "당신은 공격을 성공하였습니다!";
                        aiHP = aiHP - damage;
                        hpText.text =  "Hp : " + aiHP;
                        StartCoroutine(Delay());
                        AttackBtnHighO.SetActive(true);
                        if (aiHP == 0)
                        {
                            Result.text = "당신은 승리하셨습니다.";
                        }
                        
                        break;
                }

                break;
            case "Middle":
                switch (myChoice)
                {
                    case "High":
                        Result.text = "당신은 공격을 성공하였습니다!";
                        aiHP = aiHP - damage;
                        hpText.text =  "Hp : " + aiHP;
                        StartCoroutine(Delay());
                        AttackBtnMiddleO.SetActive(true);
                        if (aiHP == 0)
                        {
                            Result.text = "당신은 승리하셨습니다.";
                        }
                       
                        break;
                    case "Middle":
                        Result.text = "상대가 공격을 막았습니다!";
                        hpText.text =  "Hp : " + aiHP;
                        StartCoroutine(Delay());
                        AttackBtnHighX.SetActive(true);

                        break;
                    case "Low":
                        Result.text = "당신은 공격을 성공하였습니다!";
                        aiHP = aiHP - damage;
                        hpText.text =  "Hp : " + aiHP;
                        StartCoroutine(Delay());
                        AttackBtnHighO.SetActive(true);
                        if (aiHP == 0)
                        {
                            Result.text = "당신은 승리하셨습니다.";
                        }
                        
                        break;
                }

                break;
            case "Low":
                switch (myChoice)
                {
                    case "High":
                        Result.text = "당신은 공격을 성공하였습니다!";
                        aiHP = aiHP - damage;
                        hpText.text =  "Hp : " + aiHP;
                        StartCoroutine(Delay());
                        AttackBtnLowO.SetActive(true);
                        if (aiHP == 0)
                        {
                            Result.text = "당신은 승리하셨습니다.";
                        }
                        break;
                    case "Middle":
                        Result.text = "fail Attacking";
                        aiHP = aiHP - damage;
                        hpText.text =  "Hp : " + aiHP;
                        StartCoroutine(Delay());
                        AttackBtnLowO.SetActive(true);
                        if (aiHP == 0)
                        {
                            Result.text = "당신은 승리하셨습니다.";
                        }
                        break;
                    case "Low":
                        Result.text = "상대가 공격을 막았습니다!";
                        hpText.text =  "Hp : " + aiHP;
                        StartCoroutine(Delay());
                        AttackBtnLowX.SetActive(true);

                        break;
                }


                break;
        }

    }



    IEnumerator Delay()
    {

        yield return new WaitForSeconds(2);

    }
}
