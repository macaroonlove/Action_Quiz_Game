using Photon.Pun;
using System.Collections.Generic;
using System.Collections;
using SystemShuffle;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public PhotonView PV;
    public GameObject DollyCart;
    private Animator anim;

    private bool DollyOn = false;

    public static string[] PlayerList = new string[4];

    public List<QuestionAndAnswers> QnA;
    public GameObject[] options;
    public int currentQuestion;

    public GameObject TimerObj;
    private float SettingTime = 10;
    private float Timer = 0;
    private float TimerNum = 0;
    public bool TimerStart = false;
    private int TimerState;

    public TextMeshProUGUI QuestionTxt;

    public GameObject QuizPanel;
    public GameObject QuizResultPanel;
    public Sprite[] AnsOrErr;

    public GameObject AttackOrDefend;
    public GameObject P1AttackOrDefendText;
    public GameObject P2AttackOrDefendText;
    public Sprite[] AtkOrDef;
    private int P1S = 5, P2S = 5;
    private bool ActionLogicBool = true;
    string ActionState;
    public static bool ReG = false;

    public GameObject GameEndPanel;
    private int solvedProblem = 0;

    public List<GameObject> P1HP;
    public List<GameObject> P2HP;

    void Start()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            PlayerList[i] = PhotonNetwork.PlayerList[i].NickName;

        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
            if (PlayerList[i] == PhotonNetwork.LocalPlayer.NickName)
                anim = GameObject.Find("Character" + (i + 1) + "(Clone)").transform.GetChild(0).GetComponent<Animator>();

        if (PhotonNetwork.IsMasterClient)
            generateQuestion();
        else
            AttackOrDefend.GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, 180f);
    }

    void GameEndPS()
    {
        GameEndPanel.SetActive(true);
    }

    void Update()
    {
        if(P1HP.Count == 0 || P2HP.Count == 0)
        {
            Invoke("GameEndPS", 5f);
            TimerObj.SetActive(false);
            QuizPanel.SetActive(false);
            TimerStart = false;
            if (P1HP.Count == 0)
            {
                if (PhotonNetwork.IsMasterClient)//p1
                {
                    GameEndPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "<#D90D00>패배</color>";
                    anim.SetBool("Lose", true);
                }
                else//p2
                {
                    GameEndPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "<#BDBE00>승리</color>";
                    anim.SetBool("Victory", true);
                }
            }
            else if (P2HP.Count == 0)
            {
                if (PhotonNetwork.IsMasterClient)//p1
                {
                    GameEndPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "<#BDBE00>승리</color>";
                    anim.SetBool("Victory", true);
                }
                else//p2
                {
                    GameEndPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "<#D90D00>패배</color>";
                    anim.SetBool("Lose", true);
                }
            }
        }

        #region 타이머
        if (TimerStart)
        {
            Timer -= Time.deltaTime;
            int CurrentTimer = ((int)Timer % 60);
            TimerObj.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = CurrentTimer.ToString();
            TimerObj.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = (Timer % 60) / TimerNum;

            if (CurrentTimer < 0)
                TimerObj.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "0";
            if (CurrentTimer < 0 && TimerState == 1)
            {
                PV.RPC("RP1HP", RpcTarget.All);
                PV.RPC("RP2HP", RpcTarget.All);
                TimerStart = false;
                QuizResultPanel.GetComponent<Image>().sprite = AnsOrErr[2];
                QuizResultPanel.SetActive(true);
                StartCoroutine("Concentration");
            }
            else if(CurrentTimer < 0 && TimerState == 2)
            {
                TimerStart = false;
                WinOrLose();
            }
            else if (CurrentTimer < 0 && TimerState == 3)
            {
                TimerStart = false;
                AttackOrDefend.SetActive(false);
                WAOD_Action();
            }
        }
        #endregion

        #region 승패판단 후 로직2
        if (DollyOn)
        {
            DollyCart.transform.GetChild(0).rotation = Quaternion.Slerp(DollyCart.transform.GetChild(0).rotation, Quaternion.Euler(0, 180f, 0), 0.01f);
        }

        if (DollyCart.GetComponent<Cinemachine.CinemachineDollyCart>().m_Position == 9)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (QuizResultPanel.GetComponent<Image>().sprite.name == "정답")
                {
                    P1AttackOrDefendText.GetComponent<Image>().sprite = AtkOrDef[0];
                    P2AttackOrDefendText.GetComponent<Image>().sprite = AtkOrDef[1];
                }
                else
                {
                    P1AttackOrDefendText.GetComponent<Image>().sprite = AtkOrDef[1];
                    P2AttackOrDefendText.GetComponent<Image>().sprite = AtkOrDef[0];
                }
            }
            else if (!PhotonNetwork.IsMasterClient)
            {
                if (QuizResultPanel.GetComponent<Image>().sprite.name == "정답")
                {
                    P1AttackOrDefendText.GetComponent<Image>().sprite = AtkOrDef[1];
                    P2AttackOrDefendText.GetComponent<Image>().sprite = AtkOrDef[0];
                }
                else
                {
                    P1AttackOrDefendText.GetComponent<Image>().sprite = AtkOrDef[0];
                    P2AttackOrDefendText.GetComponent<Image>().sprite = AtkOrDef[1];

                }
            }

            if (ActionLogicBool)
            {
                AttackOrDefend.SetActive(true);
                P1AttackOrDefendText.SetActive(true);
                P2AttackOrDefendText.SetActive(true);
                PV.RPC("TimerPun", RpcTarget.AllViaServer, 5, 3);
                
                ActionLogicBool = false;
            }
        }
        #endregion

        if (ReG)
        {
            ReGQuestion();
            ReG = false;
        }
    }

    IEnumerator Concentration()
    {
        yield return new WaitForSeconds(3f);
        QuizResultPanel.SetActive(false);
        generateQuestion();
    }

    #region 타이머 설정
    [PunRPC]
    void TimerPun(int a, int b)
    {
        Timer = a;
        TimerNum = a;
        TimerState = b;
        TimerStart = true;

        if (b == 1)
            TimerObj.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(0, 0, 1, 0.46f);
        else if (b == 2)
            TimerObj.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(1, 1, 0, 0.46f);
        else if (b == 3)
            TimerObj.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(0, 1, 0, 0.46f);
    }
    #endregion

    #region 문제 나눠주기
    public void generateQuestion()
    {
        if (QnA.Count > 0) // 문제가 있으면
        {
            currentQuestion = Random.Range(0, QnA.Count); // 랜덤으로

            QuestionTxt.text = "<color=orange>문제</color>\n" + QnA[currentQuestion].Qusetion; // 문제 텍스트 넣기
            SetAnswers();
        }
        else
        {
            Debug.Log("Out of Questions");
        }
    }

    public void SetAnswers()
    {
        List<int> ansn = new List<int>() { 0, 1, 2, 3 };
        List.shuffle(ansn);

        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<AnswerScript>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = i + 1 + ". " + QnA[currentQuestion].Answers[ansn[i]];
            if (0 == ansn[i])
            {
                options[i].GetComponent<AnswerScript>().isCorrect = true;
            }
        }
        PV.RPC("TimerPun", RpcTarget.AllViaServer, 10, 1);
    }
    #endregion

    #region 승패 판단 & 후 로직
    public void correct()
    {
        TimerStart = false;
        //QnA.RemoveAt(currentQuestion);
        QuizResultPanel.GetComponent<Image>().sprite = AnswerScript.Correct ? AnsOrErr[0] : AnsOrErr[1];
        if(AnswerScript.Correct)
            PV.RPC("AttackState", RpcTarget.Others, false);
        else
            PV.RPC("AttackState", RpcTarget.Others, true);
        QuizPanel.SetActive(false);
        QuizResultPanel.SetActive(true);

        PV.RPC("TimerPun", RpcTarget.AllViaServer, 2, 2);
    }

    [PunRPC]
    void AttackState(bool BPState)
    {
        QuizResultPanel.GetComponent<Image>().sprite = BPState ? AnsOrErr[0] : AnsOrErr[1];
        QuizPanel.SetActive(false);
        QuizResultPanel.SetActive(true);
    }

    
    void WinOrLose()

    {
        GameEndPanel.transform.GetChild(3).GetChild(0).GetChild(0).GetChild(solvedProblem).gameObject.SetActive(true);
        GameEndPanel.transform.GetChild(3).GetChild(0).GetChild(0).GetChild(solvedProblem).GetChild(0).GetComponent<TextMeshProUGUI>().text = (solvedProblem+1) + ".";
        GameEndPanel.transform.GetChild(3).GetChild(0).GetChild(0).GetChild(solvedProblem).GetChild(2).GetComponent<TextMeshProUGUI>().text = QnA[currentQuestion].Qusetion + "(" + QnA[currentQuestion].Answers[0] + ")";
        if (QuizResultPanel.GetComponent<Image>().sprite.name == "정답")
        {
            GameEndPanel.transform.GetChild(3).GetChild(0).GetChild(0).GetChild(solvedProblem).GetChild(1).GetComponent<TextMeshProUGUI>().text = "<color=\"green\">정답</color>"; 
        }
        else if(QuizResultPanel.GetComponent<Image>().sprite.name == "오답")
        {
            GameEndPanel.transform.GetChild(3).GetChild(0).GetChild(0).GetChild(solvedProblem).GetChild(1).GetComponent<TextMeshProUGUI>().text = "<color=\"red\">오답</color>";
        }
        QnA.RemoveAt(currentQuestion);
        solvedProblem++;
        QuizResultPanel.SetActive(false);
        DollyCart.SetActive(true);
        DollyOn = true;
    }
    #endregion

    public void WhereAttackOrDefend(int i)
    {
        if (PhotonNetwork.IsMasterClient)
            PV.RPC("P1sSyn", RpcTarget.AllViaServer, i);
        else
            PV.RPC("P2sSyn", RpcTarget.AllViaServer, i * -1);
    }

    [PunRPC]
    void P1sSyn(int i) => P1S = i;

    [PunRPC]
    void P2sSyn(int i) => P2S = i;

    void WAOD_Action()
    {
        int WAOD = PhotonNetwork.IsMasterClient ? P1S : P2S;
        if (WAOD == 5)
            WAOD = Random.Range(-1, 2);
        if (WAOD == 1)
        {
            if (QuizResultPanel.GetComponent<Image>().sprite.name == "정답")
                ActionState = "UpHit";
        }
        if (WAOD == 0)
        {
            if (QuizResultPanel.GetComponent<Image>().sprite.name == "정답")
                ActionState = "MiddleHit";
        }
        if (WAOD == -1)
        {
            if (QuizResultPanel.GetComponent<Image>().sprite.name == "정답")
                ActionState = "DownHit";
        }

        StartCoroutine(WAOD_InAction(WAOD, P1S, P2S));
    }

    IEnumerator WAOD_InAction(int WAOD, int P1S, int P2S)
    {
        yield return new WaitForSeconds(0.5f);
        if (WAOD == 1)
        {
            if (QuizResultPanel.GetComponent<Image>().sprite.name == "정답")
            {
                anim.SetBool("UpAttack", true);
            }
            else if (QuizResultPanel.GetComponent<Image>().sprite.name == "오답")
            {
                if (P1S == P2S)
                {
                    anim.SetBool("UpDefence", true);
                }
                else
                {
                    anim.SetBool(ActionState, true);
                    HP();
                }
            }
        }
        else if (WAOD == 0)
        {
            if (QuizResultPanel.GetComponent<Image>().sprite.name == "정답")
            {
                anim.SetBool("MiddleAttack", true);
            }
            else if (QuizResultPanel.GetComponent<Image>().sprite.name == "오답")
            {
                if (P1S == P2S)
                {
                    anim.SetBool("MiddleDefence", true);
                }
                else
                {
                    anim.SetBool(ActionState, true);
                    HP();
                }
            }
        }
        else if (WAOD == -1)
        {
            if (QuizResultPanel.GetComponent<Image>().sprite.name == "정답")
            {
                anim.SetBool("DownAttack", true);
            }
            else if (QuizResultPanel.GetComponent<Image>().sprite.name == "오답")
            {
                if (P1S == P2S)
                {
                    anim.SetBool("DownDefence", true);
                }
                else
                {
                    anim.SetBool(ActionState, true);
                    HP();
                }
            }
        }
    }

    void HP()
    {
        if (PhotonNetwork.IsMasterClient)
            PV.RPC("RP1HP", RpcTarget.All);
        else
            PV.RPC("RP2HP", RpcTarget.All);
    }

    [PunRPC]
    void RP1HP()
    {
        P1HP[P1HP.Count - 1].SetActive(false);
        P1HP.RemoveAt(P1HP.Count - 1);
    }

    [PunRPC]
    void RP2HP()
    {
        P2HP[P2HP.Count - 1].SetActive(false);
        P2HP.RemoveAt(P2HP.Count - 1);
    }

    void ReGQuestion()
    {
        int rev = 1;
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++, rev *= -1)
        {
            if (PlayerList[i] == PhotonNetwork.LocalPlayer.NickName)
            {
                GameObject.Find("Character" + (i + 1) + "(Clone)").transform.GetChild(0).localPosition = new Vector3(0, -1.74f, 0);
                GameObject.Find("Character" + (i + 1) + "(Clone)").transform.GetChild(0).localRotation = Quaternion.Euler(0f, 180f * rev, 0f);
            }
        }

        DollyCart.SetActive(false);
        DollyCart.transform.GetChild(0).rotation = Quaternion.Euler(-15f, 180f, 0);
        DollyCart.GetComponent<Cinemachine.CinemachineDollyCart>().m_Position = 0;
        ActionLogicBool = true;
        P1AttackOrDefendText.SetActive(false);
        P2AttackOrDefendText.SetActive(false);

        P1S = 5;
        P2S = 5;

        QuizPanel.SetActive(true);
        generateQuestion();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentQuestion);
            stream.SendNext(QuestionTxt.text);
            for(int i = 0; i < options.Length; i++)
                stream.SendNext(options[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);
            stream.SendNext(ActionState);
        }
        else
        {
            this.currentQuestion = (int)stream.ReceiveNext();
            this.QuestionTxt.text = (string)stream.ReceiveNext();
            for (int i = 0; i < options.Length; i++)
                options[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (string)stream.ReceiveNext();
            ActionState = (string)stream.ReceiveNext();
        }
    }
}