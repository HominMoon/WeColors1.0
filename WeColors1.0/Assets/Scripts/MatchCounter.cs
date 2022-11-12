using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    bool isHostLoadLevelSucess;
    static bool isInGame = false;
    static int matchNumber = 0;

    public static bool isPlayer1Ready = false;
    public static bool isPlayer2Ready = false;
    public static int player1Point = 0;
    public static int player2Point = 0;

    private void Awake()
    {
        Time.timeScale = 1f;
    }

    private void Start()
    {
        playerReady.gameObject.SetActive(false);

        player1PointText.text = $"{player1Point}";
        player2PointText.text = $"{player2Point}";

        if (player1Point == 3 || player2Point == 3)
        {
            isInGame = false;
            infoText.text = "게임이 끝났습니다!";

            if (!PhotonNetwork.LocalPlayer.IsMasterClient) { return; }
            PlayerPoint();
        }

        if (isInGame)
        {
            infoText.text = "다음 게임이 시작됩니다!";
            StartCoroutine(LoadLevel());
        }

    }

    void PlayerPoint()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            photonView.RPC("RPCPlayerPoint", RpcTarget.All);
        }


    }

    [PunRPC]
    void RPCPlayerPoint()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if (player1Point == 3 && player2Point < 3)
            {
                DatabaseManager.instance.WritePointData(DatabaseManager.fu, 5);
            }
            else if (player1Point < 3 && player2Point == 3)
            {
                DatabaseManager.instance.WritePointData(DatabaseManager.fu, -5);
            }
            else if (player1Point == 3 && player2Point == 3)
            {
                DatabaseManager.instance.WritePointData(DatabaseManager.fu, 0);
            }
        }
        else
        {
            if (player1Point == 3 && player2Point < 3)
            {
                DatabaseManager.instance.WritePointData(DatabaseManager.fu, -5);
            }
            else if (player1Point < 3 && player2Point == 3)
            {
                DatabaseManager.instance.WritePointData(DatabaseManager.fu, +5);
            }
            else if (player1Point == 3 && player2Point == 3)
            {
                DatabaseManager.instance.WritePointData(DatabaseManager.fu, 0);
            }
        }

        StartCoroutine(LoadLobby());


    }

    IEnumerator LoadLobby()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("MainLobby");
    }

    private void Update()
    {
        if (PhotonNetwork.PlayerList.Length == 2 && playerReady.IsActive() == false)
        {
            StartCoroutine(SetActivePlayerReadyButton());
        }

        if (!isPlayer1Ready || !isPlayer2Ready)
        {
            infoText.text = "상대를 기다리는 중입니다...";
        }
        else if (isPlayer1Ready && isPlayer2Ready && matchNumber == 0)
        {
            infoText.text = "게임이 시작됩니다!";
            isInGame = true;
            StartCoroutine(LoadLevel());
        }
    }

    IEnumerator SetActivePlayerReadyButton()
    {
        yield return new WaitForSeconds(3.5f);
        playerReady.gameObject.SetActive(true);
    }

    public void PlayerReady()
    {
        //rpc를 사용하면 actornumber가 -1되어 전달된다 수정 필요 ->해결

        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            photonView.RPC("Player1Ready", RpcTarget.AllViaServer);
        }
        else if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            photonView.RPC("Player2Ready", RpcTarget.AllViaServer);
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
        PhotonNetwork.AutomaticallySyncScene = true;

        if (matchNumber == 4)
        {
            matchNumber = Random.Range(1, 4);
        }
        else
        {
            matchNumber++;
        }

        isInGame = true;

        Debug.Log("매치넘버" + matchNumber);
        Debug.Log("waitTime:" + waitTime);

        yield return new WaitForSeconds(waitTime);

        HostLoadLevel();
        //StartCoroutine(HostLoadingLevel());
    }

    void HostLoadLevel()
    {
        if (!PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            return;
        }
        PhotonNetwork.LoadLevel($"Game{matchNumber}");
        //PhotonNetwork.LoadLevel($"Game4");
    }

    // IEnumerator HostLoadingLevel()
    // {
    //     yield return new WaitForSeconds(1f);

    //     ClientLoadLevel();
    // }

    // void ClientLoadLevel()
    // {
    //     if(PhotonNetwork.LocalPlayer.IsMasterClient)
    //     {
    //        return;
    //     }

    //     PhotonNetwork.LoadLevel($"Game{matchNumber}");
    // }
}
