using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartUI : MonoBehaviour
{
     
    public void OnStart()
    {
        SceneManager.LoadScene(SceneType.Town.ToString());
    }
    public void OnEnd()
    {
        Debug.Log("게임을 종료");
        Application.Quit();
    }
}
