using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;

public class LobbyManager : MonoBehaviourPunCallbacks

{
    //요약: 로비 관리. 플레이어 매칭, 사용자 닉네임 변경, 로그아웃 등
    //해야하는 것: 접속, 플레이어2명 입장 시 시간 딜레이 주기 -> 업데이트에서 코루틴

    private readonly string gameVersion = "1.0";
    [SerializeField] TMP_Text version;
    [SerializeField] TMP_Text userId;
    [SerializeField] TMP_Text userPoint;
    [SerializeField] Button playButton;
    [SerializeField] TMP_Text mainText;
    [SerializeField] float waitTime = 2f;

    [SerializeField] AudioClip mainSFX;
    AudioSource audioSource;

    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;

    public DatabaseReference databaseReference;

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

    public class PlayerData
    {
        public int point;

        public PlayerData(int point)
        {
            this.point = point;
        }
    }

    public void LoadData(string UserId)
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
                    userPoint.text = $"{data.Value}";
                }
            }
        });

        //이메일을 찾았는데 없으면 -> 첫번째 세이브 데이터를 만든다. 처음 점수 100점

        //아니면 이메일이 존재할 때 있는 세이브 데이터를 불러와 여기에 저장한다.
        //매치 카운터에서는? -> 현재 가지고 있는 이메일 데이터가 파이어베이스에 존재하면, 그것을 바탕으로 점수 조절

        //PlayerData playerData = new PlayerData();
    }

    void Start()
    {
        playButton.interactable = false;
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
        InitializeFirebase();
        LoadData(user.UserId);
        
        user = auth.CurrentUser;

        version.text = "Ver " + gameVersion;
        userId.text = "ID: " + user.UserId;

        mainText.text = "서버에 연결중입니다...";
    }

    public override void OnConnectedToMaster()
    {
        playButton.interactable = true;
        mainText.text = "온라인 연결됨";
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        playButton.interactable = false;
        mainText.text = "연결이 해제되었습니다. 다시 연결 중...";

        PhotonNetwork.ConnectUsingSettings();
    }

    //playButton 클릭 되었을 때...
    public void OnClickPlayButton()
    {
        playButton.interactable = false;

        if (PhotonNetwork.IsConnected)
        {
            mainText.text = "상대를 찾고 있습니다...";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            mainText.text = "연결이 해제되었습니다. 다시 연결 중...";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // base.OnJoinRandomFailed(returnCode, message);
        mainText.text = "새로운 방을 생성중입니다...";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        mainText.text = "매치에 입장합니다!";

        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(waitTime);
        PhotonNetwork.LoadLevel("MatchCounter");
    }
}
