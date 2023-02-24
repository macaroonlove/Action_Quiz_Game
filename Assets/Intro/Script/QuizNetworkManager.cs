using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class QuizNetworkManager : MonoBehaviourPunCallbacks
{
    public PhotonView PV;

    public GameObject OOOGames;
    public GameObject StartNickName;
    public GameObject StartNickNameField;
    public Text NickNameText;
    public InputField RoomName;
    public GameObject CRoomUI;

    List<RoomInfo> MyList = new List<RoomInfo>();
    int currentPage = 1;
    int maxPage, multiple;
    public Button[] JoinRoomButton;
    public Button PrevButton;
    public Button NextButton;

    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            StartNickName.SetActive(false);
            OOOGames.SetActive(false);
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
            Invoke("OOOGamesOff", 2f);
            PhotonNetwork.AutomaticallySyncScene = true;
        }
    }

    void Update()
    {
        NickNameText.text = PhotonNetwork.LocalPlayer.NickName;
    }

    #region 시작
    private void OOOGamesOff() => OOOGames.SetActive(false);

    public void SetNickName()
    {
        PhotonNetwork.LocalPlayer.NickName = StartNickNameField.transform.GetChild(1).GetComponent<InputField>().text;
        StartNickNameField.SetActive(false);
    }

    public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby();

    public override void OnJoinedLobby() => MyList.Clear();
    #endregion

    #region 방 관련
    public void CreateRoomUI(bool i)
    {
        if(i == false)
            CRoomUI.SetActive(true);
        else if(i == true)
            CRoomUI.SetActive(false);
    }

    public void CreateRoom()
    {
        int mpn = 0;
        mpn = CRoomUI.transform.GetChild(1).GetChild(1).GetChild(2).GetComponent<Dropdown>().value == 0 ? 2 : 4;
        PhotonNetwork.CreateRoom(RoomName.text == "" ? PhotonNetwork.LocalPlayer.NickName + "님의 방" : RoomName.text, new RoomOptions { MaxPlayers = ((byte)mpn) });
        CRoomUI.SetActive(false);
    }

    public override void OnCreatedRoom()
    {
        PhotonNetwork.LoadLevel("Room");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        RoomName.text = "";
        CreateRoom();
    }

    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        RoomName.text = "";
        CreateRoom();
    }

    public void LeaveRoom() => PhotonNetwork.LeaveRoom();
    #endregion

    #region 방 리스트 생성
    public void MyListClick(int num)
    {
        if (num == -2) --currentPage;
        else if (num == -1) ++currentPage;
        else PhotonNetwork.JoinRoom(MyList[multiple + num].Name);
        MyListRenewal();
    }

    void MyListRenewal()
    {
        maxPage = (MyList.Count % JoinRoomButton.Length == 0) ? MyList.Count / JoinRoomButton.Length : MyList.Count / JoinRoomButton.Length + 1;

        PrevButton.interactable = (currentPage <= 1) ? false : true;
        NextButton.interactable = (currentPage >= maxPage) ? false : true;
        multiple = (currentPage - 1) * JoinRoomButton.Length;
        for (int i = 0; i < JoinRoomButton.Length; i++)
        {
            JoinRoomButton[i].interactable = (multiple + i < MyList.Count) ? true : false;
            JoinRoomButton[i].transform.GetChild(0).GetComponent<Text>().text = (multiple + i < MyList.Count) ? MyList[multiple + i].PlayerCount + "/" + MyList[multiple + i].MaxPlayers : "";
            JoinRoomButton[i].transform.GetChild(1).GetComponent<Text>().text = (multiple + i < MyList.Count) ? MyList[multiple + i].Name : "";
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        int roomCount = roomList.Count;
        for (int i = 0; i < roomCount; i++)
        {
            if (!roomList[i].RemovedFromList)
            {
                if (!MyList.Contains(roomList[i]))
                    MyList.Add(roomList[i]);
                else
                    MyList[MyList.IndexOf(roomList[i])] = roomList[i];
            }
            else if (MyList.IndexOf(roomList[i]) != -1)
                MyList.RemoveAt(MyList.IndexOf(roomList[i]));
        }
        MyListRenewal();
    }
    #endregion

    #region 게임 종료
    public void GameExit()
    {
        PhotonNetwork.Disconnect();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    #endregion
}
