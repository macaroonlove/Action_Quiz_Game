using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviourPunCallbacks
{
    public PhotonView PV;

    GameObject CharacterPrefab;
    public static string[] PlayerList = new string[4];
    public int OriginPlayerNum = 0;
    public TextMeshProUGUI RoomName;
    public TextMeshProUGUI RoomPlayerSetting;
    public GameObject PlayerFace;
    public GameObject StartButton;

    [Header("Chating")]
    public GameObject ChatInput;
    public InputField WillSendChat;
    public Text[] ChatText;

    [Header("준비&시작")]
    public Button ReadyStartButton;
    public Sprite[] ReadyButton;
    private bool Ready = false;
    private int ReadyNum;

    //public void CharacterSelect(int i)
    //{
    //    PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "캐릭터", i } });

    //    int k = i == 1 ? 0 : 3;
    //    for (int j = 0; j < PhotonNetwork.PlayerList.Length; j++, k += 3)
    //    {
    //        if (PhotonNetwork.LocalPlayer.NickName == PlayerList[j])
    //        {
    //            GameObject.Find("Character" + (j + 1) + "(Clone)").transform.GetChild(k).gameObject.SetActive(true);
    //        }
    //    }
    //}

    void Awake()
    {
        ReadyNum = PhotonNetwork.CurrentRoom.PlayerCount;
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            PlayerList[i] = PhotonNetwork.PlayerList[i].NickName;
        float xpo = -3;
        for (int i = 0; i < 4; i++, xpo += 1.5f)
        {
            if (PlayerList[i] == PhotonNetwork.LocalPlayer.NickName)
            {
                CharacterPrefab = PhotonNetwork.Instantiate("Character" + (i + 1), new Vector3(xpo, 1.73f, 3.5f), Quaternion.Euler(0f, 0f, 0f), 0);
                CharacterPrefab.GetComponent<Player>().enabled = true;
                //CharacterPrefab.name = PhotonNetwork.LocalPlayer.NickName;
                CharacterPrefab.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = PlayerList[i];
            }
        }
        OriginPlayerNum = PhotonNetwork.PlayerList.Length;
        PV.RPC("StartEndInvokeFunc", RpcTarget.AllBuffered);

        RoomName.text = "방 이름: " + PhotonNetwork.CurrentRoom.Name;
        if(PhotonNetwork.CurrentRoom.MaxPlayers == 2)
        {
            PlayerFace.transform.GetChild(2).gameObject.SetActive(false);
            PlayerFace.transform.GetChild(3).gameObject.SetActive(false);
        }
    }

    [PunRPC]
    void StartEndInvokeFunc()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            PlayerList[i] = PhotonNetwork.PlayerList[i].NickName;
        RoomPlayerSetting.text = PhotonNetwork.CurrentRoom.PlayerCount + "/<color=red>" + PhotonNetwork.CurrentRoom.MaxPlayers + "</color>";
        Invoke("StartEnd", 0.1f);
    }

    void StartEnd()
    {
        for (int i = 0; i < 4; i++)
            if (PlayerList[i] != null && PlayerList[i] != PhotonNetwork.LocalPlayer.NickName)
                GameObject.Find("Character" + (i + 1) + "(Clone)").transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = PlayerList[i];
    }

    void Update()
    {
        ReturnChat();
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

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        RoomRenewal();
    }

    public override void OnJoinedRoom()
    {
        RoomRenewal();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        RoomRenewal();
        RoomPlayerSetting.text = PhotonNetwork.CurrentRoom.PlayerCount + "/<color=red>" + PhotonNetwork.CurrentRoom.MaxPlayers + "</color>";
    }

    void RoomRenewal()
    {
        if (!PhotonNetwork.IsMasterClient)
            ReadyStartButton.GetComponent<Image>().sprite = ReadyButton[0];
    }

    public void ReturnLobby() => PhotonNetwork.LeaveRoom();

    public override void OnLeftRoom() => SceneManager.LoadScene("Lobby");

    public void ReadyStart()
    {
        if (PhotonNetwork.IsMasterClient)// && ReadyNum == 0
        {
            PhotonNetwork.LoadLevel("GameDisplay");
        }
        if (!PhotonNetwork.IsMasterClient)
        {
            if (Ready == false)
            {
                ReadyStartButton.GetComponent<Image>().sprite = ReadyButton[1];
                PV.RPC("OnReady", RpcTarget.AllBuffered);
                Ready = true;
            }
            else
            {
                ReadyStartButton.GetComponent<Image>().sprite = ReadyButton[0];
                PV.RPC("OffReady", RpcTarget.AllBuffered);
                Ready = false;
            }
        }
    }

    [PunRPC]
    void OnReady() => ReadyNum--;
    [PunRPC]
    void OffReady() => ReadyNum++;
}
