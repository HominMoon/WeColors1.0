using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class Game3Manager : MonoBehaviourPunCallbacks
{
    private static Game3Manager instance = null;

    private void Awake()
    {
        PhotonNetwork.IsMessageQueueRunning = true;

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static Game3Manager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    #region variable

    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Transform[] spawnPositions;

    [SerializeField] TMP_Text infoText;
    [SerializeField] TMP_Text countText;
    [SerializeField] TMP_Text timeText;

    [SerializeField] int countTimer = 3;
    [SerializeField] int gameTimer = 60;

    [SerializeField] int numberofItemSpawn = 8;

    int[] playerScore = new int[PhotonNetwork.PlayerList.Length];
    public GameObject[] playerList;
    [SerializeField] GameObject winner;

    int playerInstantiateCount = 0;

    float timer = 0;

    bool isGameEnd = false;

    #endregion

    void Start()
    {
        countText.text = " ";
        PhotonNetwork.IsMessageQueueRunning = true;
        
        StartCoroutine(WaitAndSpawn());
        StartCoroutine(WaitPlayer());
    }

    IEnumerator WaitAndSpawn()
    {
        yield return new WaitForSeconds(3f);
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        int localplayerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        Transform spawnPosition = spawnPositions[localplayerIndex];

        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition.position, spawnPosition.rotation);
            photonView.RPC("RPCplayerCount", RpcTarget.AllViaServer);
        }
        else if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition.position, spawnPosition.rotation);
            photonView.RPC("RPCplayerCount", RpcTarget.AllViaServer);
        }

        // ??? ???????????? ?????? ?????? ?????? ????????? ?????? ??????
    }

    [PunRPC]
    void RPCplayerCount()
    {
        playerInstantiateCount++;
    }

    IEnumerator WaitPlayer()
    {
        yield return new WaitUntil(() => playerInstantiateCount >= 2);
        StartCoroutine(TimeManager());
    }

    IEnumerator TimeManager()
    {
        StartCoroutine(StartCounter());
        yield return new WaitUntil(() => countTimer == 0);
        //??????????????? ???????????? ???????????? ???????????? ???????????? ??????. -> ????????? ??? ????????? ??? ??????

        GamePlayerRelease();

        StartCoroutine(GameStart()); //?????? ??????
    }

    private void GamePlayerRelease()
    {
        playerList = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < playerList.Length; i++)
        {
            playerList[i].GetComponent<PlayerMovement>().PlayerRelease();
            playerList[i].GetComponent<BrushTag>().SetPlayerBrushTag();
        }
    }

    IEnumerator StartCounter()
    {
        if (countTimer > 0)
        {
            countText.text = $"{countTimer}";
        }
        else if (countTimer == 0)
        {
            countText.text = "START!";
        }
        else if (!isGameEnd)
        {
            countText.text = " ";
            timeText.text = $"{gameTimer + countTimer}";
        }

        yield return new WaitForSeconds(1f);
        countTimer -= 1;
        StartCoroutine(StartCounter());
    }

    IEnumerator GameStart()
    {
        yield return new WaitUntil(() => gameTimer + countTimer == 0); //60?????? ?????? ??????

        timeText.gameObject.SetActive(false);
        StartCoroutine(GameStop());

    }

    IEnumerator GameStop()
    {
        //?????? ????????? ????????? ?????? ??????????????? ?????????
        //playerMovement?????? ???????????? ??????(?????? ????????????) ????????? ??????
        GamePlayerStop();

        isGameEnd = true;
        countText.text = "Game!";

        GetComponent<LeverDisable>().SetLeverDisable();

        yield return new WaitForSeconds(3f);
        StartCoroutine(CountFollower());
    }

    private void GamePlayerStop()
    {
        playerList = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < playerList.Length; i++)
        {
            playerList[i].GetComponent<PlayerMovement>().PlayerStop();
            playerList[i].GetComponent<PlayerMovement>().speed = 0f;
        }
    }

    IEnumerator CountFollower()
    {
        AddPoint();

        yield return new WaitForSeconds(3f);

        DestroyPlayers();

        StartCoroutine(WaitLoadLevel());
    }

    void AddPoint()
    {
        if(!PhotonNetwork.IsMasterClient) { return; }
        photonView.RPC("RPCAddPoint", RpcTarget.AllViaServer);
    }

    void DestroyPlayers()
    {
        for (int i = 0; i < playerList.Length; i++)
        {
            Destroy(playerList[i]);
        }
    }

    [PunRPC]
    void RPCAddPoint()
    {
        FollwerCount follwerCounter = GetComponent<FollwerCount>();
        follwerCounter.CountFollowerColor();

        infoText.text = $"{follwerCounter.player1FollowerCount} : {follwerCounter.player2FollowerCount}";

        if (follwerCounter.player1FollowerCount > follwerCounter.player2FollowerCount)
        {
            countText.text = "Player1 Win!";
            // rpc??? ??????
            MatchCounter.player1Point++;
        }
        else if (follwerCounter.player1FollowerCount < follwerCounter.player2FollowerCount)
        {
            countText.text = "Player2 Win!";
            MatchCounter.player2Point++;
        }
        else if (follwerCounter.player1FollowerCount == follwerCounter.player2FollowerCount)
        {
            countText.text = "Draw";
            MatchCounter.player1Point++;
            MatchCounter.player2Point++;
        }
    }

    IEnumerator WaitLoadLevel()
    {
        yield return new WaitForSeconds(2f);
        HostLoadLevel();
    }

    void HostLoadLevel()
    {
        if(!PhotonNetwork.LocalPlayer.IsMasterClient)
        {
           return;
        }
        PhotonNetwork.LoadLevel("MatchCounter");
    }

    //???????????? ?????? ?????? ????????? ??? ??? ????????? ?????? ??? ???????????????
    public override void OnLeftRoom()
    {

        base.OnLeftRoom();

        SceneManager.LoadScene("MainLobby");
    }

}
