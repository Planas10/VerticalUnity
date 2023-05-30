using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject settings;
    public GameObject buttons;
    public void Resume()
    {
        EventManager.current.PauseGame();
    }

    public void Settings()
    {
        settings.SetActive(true);
        buttons.SetActive(false);
    }

    public void GoBackToMenu()
    {
        EventManager.current.GoMenu();
    }
}
