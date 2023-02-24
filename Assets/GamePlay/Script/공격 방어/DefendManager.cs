using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefendManager : MonoBehaviour
{
    [SerializeField] Text hpText;

    public Text Result;


    public string[] Choices;
    public Button High, Middle, Low;

    public int myHp = 100;
    public int damage = 25;



    public GameObject DefendPanel;

    public GameObject DefendBtnHighO;
    public GameObject DefendBtnHighX;
    public GameObject DefendBtnMiddleO;
    public GameObject DefendBtnMiddleX;
    public GameObject DefendBtnLowO;
    public GameObject DefendBtnLowX;




    public void Play(string myChoice)
    {
        
        
        string randomChoice = Choices[Random.Range(0, Choices.Length)];

        switch (randomChoice)
        {
            case "High":
                switch (myChoice)
                {
                    case "High":
                        Result.text = "����� ������ ���ҽ��ϴ�!";
                        hpText.text = "Hp : " + myHp;
                        StartCoroutine(Delay());
                        DefendBtnHighX.SetActive(true);

                        break;
                    case "Middle":
                        Result.text = "��밡 ������ �����Ͽ����ϴ�!";
                        myHp = myHp - damage;
                        hpText.text =  "Hp : " + myHp;
                        StartCoroutine(Delay());
                        DefendBtnHighO.SetActive(true);
                        if (myHp == 0)
                        {
                            Result.text = "����� �й��ϼ̽��ϴ�.";
                        }

                        break;
                    case "Low":
                        Result.text = "��밡 ������ �����Ͽ����ϴ�!";
                        myHp = myHp - damage;
                        hpText.text =  "Hp : " + myHp;
                        StartCoroutine(Delay());
                        DefendBtnHighO.SetActive(true);
                        if (myHp == 0)
                        {
                            Result.text = "����� �й��ϼ̽��ϴ�.";
                        }

                        break;
                }

                break;
            case "Middle":
                switch (myChoice)
                {
                    case "High":
                        Result.text = "��밡 ������ �����Ͽ����ϴ�!";
                        myHp = myHp - damage;
                        hpText.text =  "Hp : " + myHp;
                        StartCoroutine(Delay());
                        DefendBtnMiddleO.SetActive(true);
                        if (myHp == 0)
                        {
                            Result.text = "����� �й��ϼ̽��ϴ�";
                        }

                        break;
                    case "Middle":
                        Result.text = "����� ������ ���ҽ��ϴ�!";
                        hpText.text =  "Hp : " + myHp;
                        StartCoroutine(Delay());
                        DefendBtnHighX.SetActive(true);

                        break;
                    case "Low":
                        Result.text = "��밡 ������ �����Ͽ����ϴ�!";
                        myHp = myHp - damage;
                        hpText.text =  "Hp : " + myHp;
                        StartCoroutine(Delay());
                        DefendBtnHighO.SetActive(true);
                        if (myHp == 0)
                        {
                            Result.text = "����� �й��ϼ̽��ϴ�";
                        }

                        break;
                }

                break;
            case "Low":
                switch (myChoice)
                {
                    case "High":
                        Result.text = "��밡 ������ �����Ͽ����ϴ�!";
                        myHp = myHp - damage;
                        hpText.text =  "Hp : " + myHp;
                        StartCoroutine(Delay());
                        DefendBtnLowO.SetActive(true);
                        if (myHp == 0)
                        {
                            Result.text = "����� �й��ϼ̽��ϴ�";
                        }
                        break;
                    case "Middle":
                        Result.text = "fail Attacking";
                        myHp = myHp - damage;
                        hpText.text =  "Hp : " + myHp;
                        StartCoroutine(Delay());
                        DefendBtnLowO.SetActive(true);
                        if (myHp == 0)
                        {
                            Result.text = "����� �й��ϼ̽��ϴ�";
                        }
                        break;
                    case "Low":
                        Result.text = "����� ������ ���ҽ��ϴ�!";
                        hpText.text =  "Hp : " + myHp;
                        StartCoroutine(Delay());
                        DefendBtnLowX.SetActive(true);

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
