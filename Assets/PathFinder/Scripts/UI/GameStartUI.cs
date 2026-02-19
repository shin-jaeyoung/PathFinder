using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartUI : MonoBehaviour
{
     
    public void NewGame()
    {
        GameManager.instance.StartGame(false);
    }
    public void LoadGame()
    {
        GameManager.instance.StartGame(true);
    }
    public void OnEnd()
    {
        Debug.Log("게임을 종료");
        Application.Quit();
    }
}
