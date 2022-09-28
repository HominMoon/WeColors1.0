using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class MatchCounter : MonoBehaviourPunCallbacks
{
    //방장은 상대 플레이어가 입장을 완료할 때 까지 기다려줘야 한다. -> 대기시간 때문에 게임 시작시간에 차이 발생함 
    // 방장은 플레이어2가 들어오자 마자 LoadLevel이 실행, 플레이어2는 3초 대기 후 입장하고 LoadLevel 실행됨 -> 약 3초 차이 발생
    // 어떻게 loadlevel을 동시에 실행시킬 것인가? 
    // 아이디어1. 방장을 3초 더 기다리게 한다. (쉬움)
    // 이거로-> 아이디어2. 플레이어가 준비 되었는지 확인하기 위한 버튼을 만들고 두 플레이어 모두 준비되었을 때 loadlevel 실행한다.

    [SerializeField] float waitTime = 3f;
    [SerializeField] TMP_Text infoText;

    [SerializeField] Button playerReady;
    [SerializeField] TMP_Text player1ReadyText;
    [SerializeField] TMP_Text player2ReadyText;

    [SerializeField] TMP_Text player1PointText;
    [SerializeField] TMP_Text player2PointText;

    bool isPlayerNumber2 = false;
    static bool isInGame = false;
    static int matchNumber = 0;

    public static bool isPlayer1Ready = false;
    public static bool isPlayer2Ready = false;
    public static int player1Point = 0;
    public static int player2Point = 0;

    private void Start()
    {
        if(isInGame)
        {
            infoText.text = "다음 게임이 시작됩니다!";
            StartCoroutine(LoadLevel());
        }
        
    }

    private void Update()
    {
        if(!isPlayer1Ready || !isPlayer2Ready)
        {
            infoText.text = "상대를 기다리는 중입니다...";
        }
        else if (isPlayer1Ready && isPlayer2Ready && matchNumber == 0)
        {
            infoText.text = "게임이 시작됩니다!";
            StartCoroutine(LoadLevel());
        }
    }

    public void PlayerReady()
    {
        //rpc를 사용하면 actornumber가 -1되어 전달된다 수정 필요

        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            photonView.RPC("Player1Ready", RpcTarget.All);
        }
        else if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            photonView.RPC("Player2Ready", RpcTarget.All);
        }
    }

    [PunRPC]
    void Player1Ready()
    {
        player1ReadyText.gameObject.SetActive(true);
        isPlayer1Ready = true;
    }

    [PunRPC]
    void Player2Ready()
    {
        player2ReadyText.gameObject.SetActive(true);
        isPlayer2Ready = true;
    }

    IEnumerator LoadLevel()
    {   
        matchNumber++;
        isInGame = true;
        Debug.Log("매치넘버" + matchNumber);
        yield return new WaitForSeconds(waitTime);
        PhotonNetwork.LoadLevel($"Game{matchNumber}");
    }

}
