using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlerControler : MonoBehaviour
{
    public void ButtonBack()
    {
        SceneManager.LoadScene(2);
    }
}
