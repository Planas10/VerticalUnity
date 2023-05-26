using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsController : MonoBehaviour
{
    public void ButtonBack()
    {
        SceneManager.LoadScene(0);
    }
    public void ButtonControls()
    {
        SceneManager.LoadScene(3);
    }
}
