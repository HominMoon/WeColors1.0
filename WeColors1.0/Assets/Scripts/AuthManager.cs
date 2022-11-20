using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using GooglePlayGames;
using Firebase.Database;
using Firebase.Extensions;

public class AuthManager : MonoBehaviour
{
    //요약: 파이어베이스에 사용자를 등록한다. 사용자 관리 목적

    public static AuthManager instance = null;

    [SerializeField] Button GoogleLoginButton;

    public FirebaseApp firebaseApp;
    public FirebaseAuth firebaseAuth;
    public FirebaseUser firebaseUser;
    public Firebase.Auth.Credential credential;

    public bool isSignInProgress;
    public bool isFirebaseReady;

    public string UserId;

    private void Awake() {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if(instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        GoogleLoginButton.interactable = false;
        
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var result = task.Result;

            if(result != DependencyStatus.Available)
            {
                isFirebaseReady = false;
            }
            else
            {
                firebaseApp = FirebaseApp.DefaultInstance;
                firebaseAuth = FirebaseAuth.DefaultInstance;
                isFirebaseReady = true;

            }

            GoogleLoginButton.interactable = isFirebaseReady;
        });

    }

    public void TryFireBaseAuth()
    {
        credential = Firebase.Auth.PlayGamesAuthProvider.GetCredential(GoogleManager.authCode);

        firebaseAuth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
        {
            if(task.IsFaulted)
            {
                Debug.LogError("Error: " + task.Exception);
            }
            if(task.IsCanceled)
            {
                Debug.LogError("Task Canceled... please retry");
                GoogleLoginButton.interactable = true;
            }

            firebaseUser = task.Result;
            UserId = firebaseUser.UserId;
            DatabaseManager.instance.LoadData(UserId);
        });
    }
}
