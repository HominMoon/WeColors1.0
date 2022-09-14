using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using GooglePlayGames;
using TMPro;

public class AuthManager : MonoBehaviour
{
    static AuthManager instance = null;

    [SerializeField] Button GoogleLoginButton;

    FirebaseApp firebaseApp;
    FirebaseAuth firebaseAuth;
    FirebaseUser firebaseUser;

    bool isSignInProgress;
    bool isFirebaseReady;

    public static AuthManager Instance
    {
        get
        {
            return instance;
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

}
