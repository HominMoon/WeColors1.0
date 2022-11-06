using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game4Manager : MonoBehaviourPunCallbacks
{

    private static Game4Manager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static Game4Manager Instance
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

    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text infoText;
    [SerializeField] TMP_Text timeText;
    [SerializeField] TMP_Text countText;

    [SerializeField] int gameTimer = 60;
    [SerializeField] int countTimer = 3;

    [SerializeField] int numberofItemSpawn = 8;
    [SerializeField] float itemSpawnPeriod = 5f;

    int[] playerScore = new int[PhotonNetwork.PlayerList.Length];
    [SerializeField] GameObject[] playerList;

    int playerInstantiateCount = 0;
    [SerializeField] int player1Health = 3;
    [SerializeField] int player2Health = 3;
    public bool isPlayer1Damaged;
    public bool isPlayer2Damaged;


    float timer = 0;
    int winnerPlayerNum;

    bool isGameEnd = false;

    #endregion

    void Start()
    {
        countText.text = " ";
        PhotonNetwork.IsMessageQueueRunning = true;

        StartCoroutine(WaitAndSpawn());
        StartCoroutine(WaitPlayer());
    }

    void Update()
    {
        if(player1Health <= 0 || player2Health <= 0) { return; }

        if(player2Health <= 0)
        { 
            winnerPlayerNum = 1;
            isGameEnd = true;
        }
        else if(player1Health <= 0)
        { 
            winnerPlayerNum = 2;
            isGameEnd = true;
        }
    }

    //플레이어 체력에 대해
    //플레이어 체력을 플레이어에서 관리하지 말고 게임4매니저에서 관리하자(어차피 게임4 밖에 안씀)
    //그렇다면 플레이어가 닿았을 때 1초동안은 닿아도 다시 체력이 깎이지 않도록 하는 변수만 플레이어에서 관리한다.
    //플레이어 체력이 각각 동기화 되도록 rpc를 사용한다.
    //검은 큐브에 오브젝트 닿음 -> 큐브에 닿은 오브젝트 태그가 player 이고 -> 
    //photonview가 내것일 때, game4manager에서 내 player의 체력을 -1(rpc로)

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

        // 위 플레이어 생성 나눌 필요 있는지 검토 필요

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
        //플레이어가 생성되면 공중에서 움직이지 못하도록 한다. -> 카운트 후 움직일 수 있음

        GamePlayerRelease();

        SpawnItem();
        StartCoroutine(GameStart()); //게임 시작
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
        else if (countTimer < 0 || countTimer + gameTimer > 0)
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
        StartCoroutine(ItemManager());

        yield return new WaitUntil(() => isGameEnd == true); //60초간 게임 진행

        timeText.gameObject.SetActive(false);
        StartCoroutine(GameStop());

    }

    IEnumerator ItemManager()
    {
        yield return new WaitForSeconds(itemSpawnPeriod);
        SpawnItem();
        StartCoroutine(ItemManager());
    }

    IEnumerator GameStop()
    {
        //게임 시간을 멈추지 말고 플레이어만 멈추자
        //playerMovement에서 플레이어 정지(또는 비활성화) 메서드 호출
        GamePlayerStop();

        countText.text = "Game!";

        GetComponent<LeverDisable>().SetLeverDisable();

        yield return new WaitForSeconds(3f);
        StartCoroutine(WinnerAnnounce());
    }

    private void GamePlayerStop()
    {
        playerList = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < playerList.Length; i++)
        {
            playerList[i].GetComponent<PlayerMovement>().PlayerStop();
        }
    }

    IEnumerator WinnerAnnounce()
    {
        AddPoint();

        yield return new WaitForSeconds(3f);

        DestroyPlayers();

        StartCoroutine(WaitRPCLoadLevel());
    }

    void AddPoint()
    {
        if (!PhotonNetwork.IsMasterClient) { return; }
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
        if(winnerPlayerNum == 1)
        {
            countText.text = "Player1 Win!";
            MatchCounter.player1Point++;
        }
        else if(winnerPlayerNum == 2)
        {
            countText.text = "Player2 Win!";
            MatchCounter.player2Point++;
        }
    }

    IEnumerator WaitRPCLoadLevel()
    {
        yield return new WaitForSeconds(2f);

        HostLoadLevel();
    }

    void HostLoadLevel()
    {
        PhotonNetwork.LoadLevel("MatchCounter");
    }

    public void SpawnItem()
    {
        int xVal = Random.Range(0, 15);
        int zVal = Random.Range(0, 15);

        if (!PhotonNetwork.IsMasterClient) { return; }

        PhotonNetwork.Instantiate(itemPrefab.name, new Vector3(xVal, 1, zVal), Quaternion.identity); // y = 1 for floating

    }

    public void PlayerHealth()
    {
        if(PhotonNetwork.LocalPlayer.ActorNumber == 1 && isPlayer1Damaged == false)
        {
            photonView.RPC("RPCPlayer1HealthDecrease", RpcTarget.All);
        }
        else if(PhotonNetwork.LocalPlayer.ActorNumber == 2 && isPlayer2Damaged == false)
        { 
            photonView.RPC("RPCPlayer2HealthDecrease", RpcTarget.All);
        }
    }

    [PunRPC]
    void RPCPlayer1HealthDecrease()
    {
        player1Health--;
    }

    [PunRPC]
    void RPCPlayer2HealthDecrease()
    {
        player2Health--;
    }

    //뒤로가기 또는 버튼 눌렸을 때 창 띄워서 물은 후 실행하도록
    public override void OnLeftRoom()
    {

        base.OnLeftRoom();

        SceneManager.LoadScene("MainLobby");
    }
}
