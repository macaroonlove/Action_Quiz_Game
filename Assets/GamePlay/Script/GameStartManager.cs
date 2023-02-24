using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class GameStartManager : MonoBehaviourPunCallbacks
{
    public PhotonView PV;

    GameObject CharacterPrefab;
    public static string[] PlayerList = new string[4];
    public TextMeshProUGUI[] PlayerName;

    [Header("Chating")]
    public GameObject ChatInput;
    public InputField WillSendChat;
    public Text[] ChatText;

    [Header("Setting")]
    public GameObject InGameSettingUI;

    void Awake()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            PlayerList[i] = PhotonNetwork.PlayerList[i].NickName;
        float xpo = 2;
        int rev = 1;
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++, xpo -= 4f, rev *= -1)
        {
            if (PlayerList[i] == PhotonNetwork.LocalPlayer.NickName)
            {
                CharacterPrefab = PhotonNetwork.Instantiate("Character" + (i + 1), new Vector3(xpo, 5.77f, 7f), Quaternion.Euler(0f, 90f * rev, 0f), 0);
                //CharacterPrefab.GetComponent<Player>().enabled = false;
                CharacterPrefab.transform.GetChild(2).gameObject.SetActive(false);
                CharacterPrefab.transform.GetChild(0).gameObject.GetComponent<Animator>().SetBool("PlayIdle", true);
                CharacterPrefab.transform.GetChild(0).gameObject.GetComponent<Animator>().applyRootMotion = true;
            }
            
            PlayerName[i].text = PhotonNetwork.PlayerList[i].NickName;
        }

        Invoke("dd", 0.1f);
    }

    void dd()
    {
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
            if (PlayerList[i] != null && PlayerList[i] != PhotonNetwork.LocalPlayer.NickName)
            {
                GameObject.Find("Character" + (i + 1) + "(Clone)").transform.GetChild(2).gameObject.SetActive(false);
            }
    }

    void Start()
    {
        
    }

    void Update()
    {
        ReturnChat();
        Setting();
    }

    #region 채팅 구현
    void ReturnChat()
    {
        if (Input.GetKeyDown(KeyCode.Return) && ChatInput.activeSelf == false)
        {
            ChatInput.SetActive(true);
            WillSendChat.ActivateInputField();
        }
        else if (Input.GetKeyDown(KeyCode.Return) && ChatInput.activeSelf)
        {
            if (WillSendChat.text != "")
            {
                Send();
            }
            ChatInput.SetActive(false);
        }
    }

    public void Send()
    {
        PV.RPC("Chating", RpcTarget.All, "<color=green>" + PhotonNetwork.NickName + " :</color> " + WillSendChat.text);
        WillSendChat.text = "";

    }

    [PunRPC]
    void Chating(string msg)
    {
        bool isInput = false;
        for (int i = 0; i < ChatText.Length; i++)
        {
            if (ChatText[i].text == "")
            {
                isInput = true;
                ChatText[i].text = msg;
                break;
            }
        }
        if (!isInput)
        {
            for (int i = 1; i < ChatText.Length; i++)
                ChatText[i - 1].text = ChatText[i].text;
            ChatText[ChatText.Length - 1].text = msg;
        }
    }
    #endregion

    void Setting()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !InGameSettingUI.activeSelf)
        {
            InGameSettingUI.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && InGameSettingUI.activeSelf)
        {
            InGameSettingUI.SetActive(false);
        }
    }

    public void OutRoom()
    {
        PV.RPC("AllPlayerLoadRoom", RpcTarget.AllBuffered);
        PhotonNetwork.LeaveRoom();
    }

    public void ReturnRoom()
    {
        PhotonNetwork.LoadLevel("Room");
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }

    [PunRPC]
    void AllPlayerLoadRoom()
    {
        Invoke("RemainingPlayers", 0.5f);
    }

    void RemainingPlayers()
    {
        SceneManager.LoadScene("Room");
    }
}
