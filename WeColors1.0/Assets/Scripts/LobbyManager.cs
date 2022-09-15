using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks

{
    private readonly string gameVersion = "1.0";
    [SerializeField] Button playButton;
    [SerializeField] TMP_Text mainText;

    void Start()
    {
        playButton.interactable = false;

        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();

        mainText.text = "서버에 연결중입니다...";
    }

    public override void OnConnectedToMaster()
    {
        playButton.interactable = true;
        mainText.text = "온라인 연결됨";
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        playButton.interactable = false;
        mainText.text = "연결이 해제되었습니다. 다시 연결 중...";

        PhotonNetwork.ConnectUsingSettings();
    }

    //playButton 클릭 되었을 때...
    public void OnClickPlayButton()
    {
        playButton.interactable = false;

        if (PhotonNetwork.IsConnected)
        {
            mainText.text = "상대를 찾고 있습니다...";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            mainText.text = "연결이 해제되었습니다. 다시 연결 중...";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        mainText.text = "상대를 찾았습니다!";
        PhotonNetwork.LoadLevel("MatchStart");
    }
}
