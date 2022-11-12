using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Auth;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager instance = null;

    public class PlayerData
    {
        public string nickname;
        public int point;

        public PlayerData(string nickname, int point)
        {
            this.nickname = nickname;
            this.point = point;
        }
    }

    public DatabaseReference databaseReference;

    public static PlayerData currentPlayerData;

    public static FirebaseUser fu;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    void start()
    {
    }

    public void LoadData(FirebaseUser firebaseUser)
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        fu = firebaseUser;

        databaseReference.Child(firebaseUser.UserId).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("canceled");
            }
            else if (task.IsFaulted)
            {
                Debug.LogError("faulted");
                FirstSaveData(firebaseUser);
            }
            else if (task.IsCompleted)
            {
                Debug.Log("OK");
                DataSnapshot snapshot = task.Result;

                IDictionary player = (IDictionary)snapshot.Children;

                currentPlayerData = new PlayerData((string)player["nickname"], (int)player["point"]);

                Debug.Log(currentPlayerData.nickname + " " + currentPlayerData.point);
            }
        });

        //이메일을 찾았는데 없으면 -> 첫번째 세이브 데이터를 만든다. 처음 점수 100점

        //아니면 이메일이 존재할 때 있는 세이브 데이터를 불러와 여기에 저장한다.
        //매치 카운터에서는? -> 현재 가지고 있는 이메일 데이터가 파이어베이스에 존재하면, 그것을 바탕으로 점수 조절

        //PlayerData playerData = new PlayerData();
    }

    public void FirstSaveData(FirebaseUser firebaseUser)
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        fu = firebaseUser;

        PlayerData playerData = new PlayerData("Player", 100);
        currentPlayerData = playerData;

        var json = JsonUtility.ToJson(playerData);
        databaseReference.Child(firebaseUser.UserId).SetRawJsonValueAsync(json);
    }

    public void WritePointData(FirebaseUser firebaseUser, int point)
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        databaseReference.Child(firebaseUser.UserId).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("canceled");
            }
            else if (task.IsFaulted)
            {
                Debug.LogError("faulted");
                FirstSaveData(firebaseUser);
            }
            else if (task.IsCompleted)
            {
                Debug.Log("OK");
                DataSnapshot snapshot = task.Result;

                IDictionary player = (IDictionary)snapshot.Children;

                PlayerData playerData = new PlayerData("Player", (int)(player["point"]) + point);

                var json = JsonUtility.ToJson(playerData);
                databaseReference.Child(firebaseUser.UserId).SetRawJsonValueAsync(json);
            }
        });

    }

}
