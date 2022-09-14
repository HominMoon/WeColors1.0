using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GooglePlayGames;
using UnityEngine.SceneManagement;

public class GoogleManager : MonoBehaviour
{
    [SerializeField] TMP_Text text;


    // Start is called before the first frame update
    void Start()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
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
                    text.text = $"{Social.localUser.id} \n {Social.localUser.userName}";
                    Invoke("Wait", 2.0f);
                    text.text = $"Entering Main Lobby...";
                    Invoke("Wait", 2.0f);
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
