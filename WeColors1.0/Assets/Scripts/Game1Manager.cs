using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game1Manager : MonoBehaviourPunCallbacks
{

    private static Game1Manager instance = null;

    private void Awake() {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject); 
        }  
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static Game1Manager Instance
    {
        get
        {
            if(instance == null)
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


    int[] playScores;
    float timer = 0;

    #endregion

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        countText.text = " ";

        StartCoroutine(Wait());
        StartCoroutine(WaitPlayer());
    }

    IEnumerator Wait()
    {
        SpawnPlayer();
        yield return new WaitForSeconds(2f);
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

    IEnumerator WaitPlayer()
    {
        yield return new WaitUntil(() => playerInstantiateCount == 2);
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
        }
    }

    IEnumerator StartCounter()
    {
        if(countTimer >= 0)
        {
            countText.text = $"{countTimer}";
        }
        else if(countTimer < 0)
        {
            timeText.text = $"{-countTimer}";
        }

        
        yield return new WaitForSeconds(1f);
        countTimer -= 1;
        StartCoroutine(StartCounter());
    }


    IEnumerator GameStart()
    {
        StartCoroutine(ItemManager());
        yield return new WaitUntil(() => -countTimer == gameTimer); //60초간 게임 진행
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

        yield return new WaitForSeconds(2f);
        StartCoroutine(CountCube());
    }

    private void GamePlayerStop()
    {
        playerList = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < playerList.Length; i++)
        {
            playerList[i].GetComponent<PlayerMovement>().PlayerStop();
        }
    }

    IEnumerator CountCube()
    {
        yield return new WaitForSeconds(1f);

        photonView.RPC("RPCAddPoint", RpcTarget.AllViaServer);

        photonView.RPC("RPCLoadLevel", RpcTarget.AllViaServer);
    }

    [PunRPC]
    void RPCLoadLevel()
    {
        PhotonNetwork.LoadLevel("MatchCounter");
    }

    [PunRPC]
    void RPCAddPoint()
    {
        CubeCounter cubeCounter = GetComponent<CubeCounter>();
        cubeCounter.CountCubeColor();

        infoText.text = $"{cubeCounter.player1CubeCount} : {cubeCounter.player2CubeCount}";

        if(cubeCounter.player1CubeCount > cubeCounter.player2CubeCount)
        {
            countText.text = "Player1 Win!";
            // rpc로 수행
            MatchCounter.player1Point++;
        }
        else if(cubeCounter.player1CubeCount < cubeCounter.player2CubeCount)
        {
            countText.text = "Player2 Win!";
            MatchCounter.player2Point++;
        }
        else if(cubeCounter.player1CubeCount == cubeCounter.player2CubeCount)
        {
            countText.text = "Draw";
            MatchCounter.player1Point++;
            MatchCounter.player2Point++;
        }
    }

    [PunRPC]
    void RPCplayerCount()
    {
        playerInstantiateCount++;
    }

    public void SpawnItem()
    {
        int xVal = Random.Range(0, 15);
        int zVal = Random.Range(0, 15);

        if (!PhotonNetwork.IsMasterClient) { return; }

        PhotonNetwork.Instantiate(itemPrefab.name, new Vector3(xVal, 1, zVal), Quaternion.identity); // y = 1 for floating

    }

    //뒤로가기 또는 버튼 눌렸을 때 창 띄워서 물은 후 실행하도록
    public override void OnLeftRoom()
    {
        
        base.OnLeftRoom();

        SceneManager.LoadScene("MainLobby");
    }
}
