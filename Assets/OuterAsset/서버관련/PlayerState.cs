using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerState : MonoBehaviourPunCallbacks
{

   /* public Text PlayerNameText;
    public Button PlayerReadyButton;
    public Image PlayerReadyImage;

    private bool isPlayerReady = false;


    public void Initialize(int playerID, string playerName)
    {
        PlayerNameText.text = playerName;

        if(PhotonNetwork.LocalPlayer.ActorNumber != playerID)
        {
            PlayerReadyButton.gameObject.SetActive(false);
        }
        else
        {
            ExitGames.Client.Photon.Hashtable initalProps = new ExitGames.Client.Photon.Hashtable() { { MultiplayerQuizgame.PLAYER_READY, isPlayerReady } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(initalProps);

            PlayerReadyButton.onClick.AddListener(() =>
                {
                    isPlayerReady = !isPlayerReady;
                    SetPlayerReady(isPlayerReady);

                    ExitGames.Client.Photon.Hashtable newProps = new ExitGames.Client.Photon.Hashtable() { { MultiplayerQuizgame.PLAYER_READY, isPlayerReady } };
                    PhotonNetwork.LocalPlayer.SetCustomProperties(newProps);


                });
        }
    }


    public void SetPlayerReady(bool playerReady)
    {
        PlayerReadyImage.enabled = playerReady;

        if(playerReady == true)
        {
            PlayerReadyButton.GetComponentInChildren<Text>().text = "Ready!";
        }
        else
        {
            PlayerReadyButton.GetComponentInChildren<Text>().text = "Ready?";
        }    
    }

    */




























    public InputField NickNameInput;
    // Start is called before the first frame update
    [ContextMenu("����")]
    void Info()
    {
        if (PhotonNetwork.InRoom)
        {
            print("���� �� �̸� : " + PhotonNetwork.CurrentRoom.Name);
            print("���� �� �ο��� : " + PhotonNetwork.CurrentRoom.PlayerCount);
            print("���� �� �ִ��ο��� : " + PhotonNetwork.CurrentRoom.MaxPlayers);

            string playerStr = "�濡 �ִ� �÷��̾� ��� : ";
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
            print(playerStr);
        }
        else
        {
            print("������ �ο� �� : " + PhotonNetwork.CountOfPlayers);
            print("�� ���� : " + PhotonNetwork.CountOfRooms);
            print("��� �濡 �ִ� �ο� �� : " + PhotonNetwork.CountOfPlayersInRooms);
            print("�κ� �ִ���? : " + PhotonNetwork.InLobby);
            print("����ƴ���? : " + PhotonNetwork.IsConnected);
        }
    }
}
