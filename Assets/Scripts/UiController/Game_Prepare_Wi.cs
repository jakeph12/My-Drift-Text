using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Prepare_Wi : WindowUi
{
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void StartMulti()
    {
        SceneManager.LoadScene(2);
    }
}
