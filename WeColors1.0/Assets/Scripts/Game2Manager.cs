using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class Game2Manager : MonoBehaviourPunCallbacks
{
    private static Game2Manager instance = null;

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

    public static Game2Manager Instance
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
    [SerializeField] GameObject wall;

    [SerializeField] TMP_Text infoText;
    [SerializeField] TMP_Text countText;

    [SerializeField] int countTimer = 3;

    [SerializeField] int numberofItemSpawn = 8;

    int[] playerScore = new int[PhotonNetwork.PlayerList.Length];
    [SerializeField] GameObject[] playerList;
    public GameObject winner;

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
            Destroy(wall);
        }
        else if(!isGameEnd)
        {
            countText.text = " ";
        }

        yield return new WaitForSeconds(1f);
        countTimer -= 1;
        StartCoroutine(StartCounter());
    }

    IEnumerator GameStart()
    {
        yield return new WaitUntil(() => winner != null); //60초간 게임 진행

        StartCoroutine(GameStop());

    }

    IEnumerator GameStop()
    {
        //게임 시간을 멈추지 말고 플레이어만 멈추자
        //playerMovement에서 플레이어 정지(또는 비활성화) 메서드 호출
        GamePlayerStop();

        isGameEnd = true;
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
            playerList[i].GetComponent<PlayerMovement>().speed = 0f;
        }
    }

    IEnumerator WinnerAnnounce()
    {
        if( winner.GetPhotonView().OwnerActorNr == 1)
        {
            countText.text = "Player1 Win!";
            MatchCounter.player1Point++;
        }
        else if( winner.GetPhotonView().OwnerActorNr == 2)
        {
            countText.text = "Player2 Win!";
            MatchCounter.player2Point++;
        }

        DestroyPlayers();

        yield return new WaitForSeconds(3f);

        StartCoroutine(WaitLoadLevel());
    }

    void DestroyPlayers()
    {
        for (int i = 0; i < playerList.Length; i++)
        {
            Destroy(playerList[i]);
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

    //뒤로가기 또는 버튼 눌렸을 때 창 띄워서 물은 후 실행하도록
    public override void OnLeftRoom()
    {

        base.OnLeftRoom();

        SceneManager.LoadScene("MainLobby");
    }

}
