using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Prepare_Wi : WindowUi
{
    public void StartGame()
    {
        Screen_Loader.Load_Async_Scene(1);
    }
    public void StartMulti()
    {
        Screen_Loader.Load_Async_Scene(2);
    }
}
