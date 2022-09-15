using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;

public class AuthManager : MonoBehaviour
{
    public static AuthManager instance = null;

    [SerializeField] Button GoogleLoginButton;

    FirebaseApp firebaseApp;
    FirebaseAuth firebaseAuth;
    FirebaseUser firebaseUser;
    Firebase.Auth.Credential credential;

    bool isSignInProgress;
    bool isFirebaseReady;

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
        
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
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

        firebaseAuth.SignInWithCredentialAsync(credential).ContinueWith(task =>
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
        });
    }

}
