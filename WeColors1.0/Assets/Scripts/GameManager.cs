using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    //요약: Game1의 관리
    //개선 필요: 타임 매니저 동작 개선, 아이템 스폰

    public static GameManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<GameManager>();

            return instance;
        }
    }
    private static GameManager instance;

    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Transform[] spawnPositions;

    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text infoText;
    [SerializeField] TMP_Text timeText;
    [SerializeField] TMP_Text countText;

    [SerializeField] float gameTimer = 60f;
    [SerializeField] float countTimer = 3f;
    [SerializeField] int numberofItemSpawn = 8;
    [SerializeField] float itemSpawnPeriod = 5f;


    int[] playScores;
    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        countText.text = " ";
        if (PhotonNetwork.PlayerList.Length == 2)
        {
            StartCoroutine(GameStart());
            StartCoroutine(ItemManager());
        }
    }

    private void Update()
    {
        TimeManager();
    }

    IEnumerator GameStart()
    {
        yield return new WaitForSeconds(2f);
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        yield return new WaitUntil(() => countTimer <= 0);
        SpawnPlayer();
        SpawnItem();
    }

    IEnumerator ItemManager()
    {
        //아이템 스폰 일정 간격마다 실행
        yield return new WaitForSeconds(itemSpawnPeriod);
        SpawnItem();
        StartCoroutine(ItemManager());
    }

    private void TimeManager()
    {
        // 좀 더 일정한 간격으로 동작하게 수정

        timer += Time.deltaTime;
        countTimer -= Time.deltaTime;

        if (gameTimer <= 0)
        {
            Time.timeScale = 0f;
        }

        if ((int)countTimer != 0)
        {
            countText.text = $"{countTimer:N0}";
        }
        else
        {
            countText.text = "START!";
        }

        if (gameTimer <= 60 && timer >= 3)
        {
            countText.gameObject.SetActive(false);
            gameTimer -= Time.deltaTime;
            timeText.text = $"{gameTimer:N0}";
        }

        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    private void SpawnPlayer()
    {
        int localplayerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        Transform spawnPosition = spawnPositions[localplayerIndex];

        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition.position, spawnPosition.rotation);
        }
        else if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition.position, spawnPosition.rotation);
        }

    }

    public void SpawnItem()
    {
        int xVal = Random.Range(0, 15);
        int zVal = Random.Range(0, 15);

        if (!PhotonNetwork.IsMasterClient) { return; }

        PhotonNetwork.Instantiate(itemPrefab.name, new Vector3(xVal, 1, zVal), Quaternion.identity); // y = 1 for floating

    }

    public override void OnLeftRoom()
    {
        //뒤로가기 또는 버튼 눌렸을 때 창 띄워서 물은 후 실행하도록
        base.OnLeftRoom();

        SceneManager.LoadScene("MainLobby");
    }

}
