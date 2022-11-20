using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Database;

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

    [SerializeField] AudioClip matchSFX;

    AudioSource audioSource;

    bool isPlayerNumber2 = false;
    bool isHostLoadLevelSucess;
    bool isSFXPlaying = false;
    static bool isInGame = false;
    static int matchNumber = 0;

    public static bool isPlayer1Ready = false;
    public static bool isPlayer2Ready = false;
    public static int player1Point = 0;
    public static int player2Point = 0;

    public DatabaseReference databaseReference;

    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;

    void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
            }
        }
    }

    void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }

    private void Awake()
    {
        Time.timeScale = 1f;
    }

    public class PlayerData
    {
        public int point;

        public PlayerData(int point)
        {
            this.point = point;
        }
    }

    private void Start()
    {
        InitializeFirebase();

        audioSource = GetComponent<AudioSource>();

        playerReady.gameObject.SetActive(false);

        player1PointText.text = $"{player1Point}";
        player2PointText.text = $"{player2Point}";

        if (player1Point == 3 || player2Point == 3)
        {
            isInGame = false;
            infoText.text = "게임이 끝났습니다!";

            audioSource.PlayOneShot(matchSFX);
            isSFXPlaying = true;

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
                LoadData(user.UserId, 5);
            }
            else if (player1Point < 3 && player2Point == 3)
            {
                LoadData(user.UserId, -5);
            }
            else if (player1Point == 3 && player2Point == 3)
            {
                LoadData(user.UserId, 0);
            }
        }
        else
        {
            if (player1Point == 3 && player2Point < 3)
            {
                LoadData(user.UserId, -5);
            }
            else if (player1Point < 3 && player2Point == 3)
            {
                LoadData(user.UserId, 5);
            }
            else if (player1Point == 3 && player2Point == 3)
            {
                LoadData(user.UserId, 0);
            }
        }

        StartCoroutine(LoadLobby());
    }

    public void LoadData(string UserId, int addpoint)
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        databaseReference.Child(UserId).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("canceled");
            }
            else if (task.IsFaulted)
            {
                Debug.LogError("faulted");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("OKLoad");
                DataSnapshot snapshot = task.Result;

                foreach(DataSnapshot data in snapshot.Children)
                {
                    int playerPoint = System.Convert.ToInt32(data.Value);
                    int sum = addpoint + playerPoint;
                    WritePointData(UserId, sum);
                }
            }
        });

        //이메일을 찾았는데 없으면 -> 첫번째 세이브 데이터를 만든다. 처음 점수 100점

        //아니면 이메일이 존재할 때 있는 세이브 데이터를 불러와 여기에 저장한다.
        //매치 카운터에서는? -> 현재 가지고 있는 이메일 데이터가 파이어베이스에 존재하면, 그것을 바탕으로 점수 조절

        //PlayerData playerData = new PlayerData();
    }

    public void WritePointData(string UserId, int sum)
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        databaseReference.Child(UserId).GetValueAsync().ContinueWith(task =>
        {
            var playerData = new PlayerData(sum);
            string json = JsonUtility.ToJson(playerData);
            databaseReference.Child(UserId).SetRawJsonValueAsync(json);

        });
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
            if (!isSFXPlaying)
            {
                audioSource.PlayOneShot(matchSFX);
            }
            isSFXPlaying = true;
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
        //PhotonNetwork.LoadLevel($"Game3");
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
