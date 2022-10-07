using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class Game2Manager : MonoBehaviourPunCallbacks
{
    private static Game2Manager instance = null;

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

    public static Game2Manager Instance
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

    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Transform[] spawnPositions;

    [SerializeField] TMP_Text infoText;
    [SerializeField] float countTimer = 5f;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        Time.timeScale = 1f;

        if (PhotonNetwork.PlayerList.Length == 2)
        {
            StartCoroutine(GameStart());
        }

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
