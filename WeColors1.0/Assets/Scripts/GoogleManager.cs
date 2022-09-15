using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GooglePlayGames;
using UnityEngine.SceneManagement;

public class GoogleManager : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    public static string authCode;

    // Start is called before the first frame update
    void Start()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        GooglePlayGames.BasicApi.PlayGamesClientConfiguration config = new GooglePlayGames.BasicApi
        .PlayGamesClientConfiguration.Builder().RequestServerAuthCode(false).Build();
        
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
    }

    public void Login()
    {
        if (PlayGamesPlatform.Instance.localUser.authenticated == false)
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    authCode = PlayGamesPlatform.Instance.GetServerAuthCode();

                    AuthManager.instance.TryFireBaseAuth();

                    text.text = $"{Social.localUser.id} \n {Social.localUser.userName}";
                    text.text = $"Entering Main Lobby...";
                    SceneManager.LoadScene("MainLobby");
                }
                else
                {
                    text.text = "Login Failed";
                }
            });
        }
    }

    public void LogOut()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();
    }

    void Wait()
    {
        Debug.Log("지연 중...");
    }
}
