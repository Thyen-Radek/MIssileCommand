using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    void Start(){
        Cursor.visible = true;
        SaveLoadManager.CreateFile();
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void RunGame()
    {
        SceneManager.LoadScene("Game");
    }

}
