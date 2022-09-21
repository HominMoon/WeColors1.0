using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<GameManager>();

            return instance;
        }
    }

    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform[] spawnPositions;
    [SerializeField] TMP_Text scoreText;

    private static GameManager instance;
    int[] playScores;

    // Start is called before the first frame update
    void Start()
    {
        playScores = new[] {9, 9};
        SpawnPlayer();
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
