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


    int[] playScores;
    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        playScores = new[] {9, 9};
        countText.text = " ";
        StartCoroutine(Spawn());
    }

    private void Update()
    {
        TimeManager();

    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(5.0f);
        SpawnPlayer();
    }

    private void ItemManager()
    {

    }

    private void TimeManager()
    {
        timer += Time.deltaTime;

        if (gameTimer <= 0)
        {
            Time.timeScale = 0f;
        }

        if (timer >= 1f && timer <= 5f)
        {
            countTimer -= Time.deltaTime;

            if ((int)countTimer != 0)
            {
                countText.text = $"{countTimer:N0}";
            }
            else
            {
                countText.text = "START!";
            }

        }

        if (gameTimer <= 60 && timer >= 5)
        {
            countText.gameObject.SetActive(false);
            gameTimer -= Time.deltaTime;
            timeText.text = $"{gameTimer:N0}";
        }

        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    private void SpawnPlayer()
    {
        int localplayerIndex = PhotonNetwork.LocalPlayer.ActorNumber -1;
        Transform spawnPosition = spawnPositions[localplayerIndex];

        if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition.position, spawnPosition.rotation);
        }
        else if(PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition.position, spawnPosition.rotation);
        }
        
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        SceneManager.LoadScene("MainLobby");
    }

    public void SpawnItem()
    {
        int xVal = Random.Range(0,16);
        int zVal = Random.Range(0,16);

        if(!PhotonNetwork.IsMasterClient) { return; }

        PhotonNetwork.Instantiate(itemPrefab.name, new Vector3(xVal, 1 , zVal), Quaternion.identity); // y = 1 for floating

    }

    public void AddScore(int playerNumber, int score)
    {
        if(!PhotonNetwork.IsMasterClient) { return; }

        playScores[playerNumber - 1] += score;

        photonView.RPC("RPCUpdateScore", RpcTarget.All, playScores[0], playScores[1]);
    }

    [PunRPC]
    void RPCUpdateScore(int player1Score, int player2Score) {
        scoreText.text = $"{player1Score} : {player2Score}";
    }
}
