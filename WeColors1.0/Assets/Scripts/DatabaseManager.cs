using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Auth;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using Firebase.Extensions;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager instance = null;

    public class PlayerData
    {
        public int point;

        public PlayerData(int point)
        {
            this.point = point;
        }
    }

    public DatabaseReference databaseReference;

    public PlayerData currentPlayerData;

    public string userId;

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

    public void LoadData(string UserId)
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        databaseReference.Child(UserId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            userId = UserId;

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

                if(!snapshot.Exists)
                {
                    FirstSaveData(UserId);
                }

                foreach(DataSnapshot data in snapshot.Children)
                {
                    IDictionary playerInfo = (IDictionary)data.Value;
                    currentPlayerData = new PlayerData((int)playerInfo["point"]);
                }
            }
        });

        //이메일을 찾았는데 없으면 -> 첫번째 세이브 데이터를 만든다. 처음 점수 100점

        //아니면 이메일이 존재할 때 있는 세이브 데이터를 불러와 여기에 저장한다.
        //매치 카운터에서는? -> 현재 가지고 있는 이메일 데이터가 파이어베이스에 존재하면, 그것을 바탕으로 점수 조절

        //PlayerData playerData = new PlayerData();
    }

    public void FirstSaveData(string UserId)
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        PlayerData playerData = new PlayerData(100);
        currentPlayerData = playerData;

        var json = JsonUtility.ToJson(playerData);
        databaseReference.Child(UserId).SetRawJsonValueAsync(json);
    }

    public void WritePointData(string UserId, int addpoint)
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        databaseReference.Child(UserId).GetValueAsync().ContinueWithOnMainThread(task =>
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
                Debug.Log("OKWrite");
                DataSnapshot snapshot = task.Result;

                IDictionary player = (IDictionary)snapshot.Children;

                PlayerData playerData = new PlayerData((int)(player["point"]) + addpoint);

                currentPlayerData = playerData;

                var json = JsonUtility.ToJson(playerData);
                databaseReference.Child(UserId).SetRawJsonValueAsync(json);
            }
        });
    }
}
