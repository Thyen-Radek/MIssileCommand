using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = true;
    }
    
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
