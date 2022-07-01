using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerData : MonoBehaviour
{
    private int CheckLoginGame;
    public int PlayerFakeLevel;
    public int PlayerUIShowLevel;
    public int RetryLevel;

    private void Awake()
    {
        LoadScene();
    }
    private void LoadScene()
    {
        PlayerUIShowLevel = PlayerPrefs.GetInt("PlayerShowLevel");
        CheckLoginGame = PlayerPrefs.GetInt("Login");
        if (CheckLoginGame == 0)
        {
            print("a");
            PlayerPrefs.SetInt("Login", 1);
            SceneManager.LoadScene(GetLevel(false));
        }
    }

    public int GetLevel(bool RetryLevel)
    {
        if (RetryLevel)
        {
            print("b");
            PlayerFakeLevel = PlayerPrefs.GetInt("RetryLevel");
        }
        else if (PlayerUIShowLevel > 8)
        {
            print("c");
            PlayerFakeLevel = Random.Range(0, 9);
            PlayerPrefs.SetInt("RetryLevel", PlayerFakeLevel);
        }
        else
        {
            print("d");
            PlayerFakeLevel = PlayerPrefs.GetInt("PlayerLevel");
            PlayerPrefs.SetInt("RetryLevel", PlayerFakeLevel);
        }

        return PlayerFakeLevel;
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Login", 0);
    }

    private void OnApplicationPause()
    {
        PlayerPrefs.SetInt("Login", 0);
    }
}
