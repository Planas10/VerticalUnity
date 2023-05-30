using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Text;

[SerializeField]
public class Int2Event : UnityEvent<int, int> {
    
}
public class EventManager : MonoBehaviour
{
    #region Signgleton
    public static EventManager current;
    public FPSController player;
    public GameObject endGame;
    public GameObject playGameCanvas;
    public AudioSource effect;

    private void Awake()
    {
        if (current == null) {
            current = this;
        }
        else if (current != null) {
            Destroy(this);
        }
        player = FindObjectOfType<FPSController>();
    }
    #endregion

    public GameObject pauseMenu;
    public Int2Event updateBulletsEvent = new Int2Event();
    public UnityEvent<int> UpdateScoreEvent = new UnityEvent<int>();
    public UnityEvent<int> UpdateRoundEvent = new UnityEvent<int>();
    public UnityEvent<int> OnGainPoints = new UnityEvent<int>();
    public UnityEvent OnKillEvent = new UnityEvent();
    public UnityEvent OnHitEvents = new UnityEvent();
    public bool paused;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) 
        {
            PauseGame();
        }
    }
    public void EndGame() 
    {
        playGameCanvas.SetActive(false);
        endGame.SetActive(true);
        player.controlable = false;
        Cursor.lockState = CursorLockMode.None;
        Invoke("GoMenu", 5);

    }
    public void GoMenu() 
    {
        SceneManager.LoadScene("Main_Menu");
    }
    public void PlayEffect(AudioClip clip) 
    {
        effect.PlayOneShot(clip);
    }
    public void PauseGame() 
    {
        paused = !paused;
        Cursor.visible = paused;
        if(paused)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
        pauseMenu.gameObject.SetActive(paused);
        Time.timeScale = Convert.ToInt32(!paused);
        player.controlable = !paused;
        
    }

    public string convertroman(int number)
    {
        if (number.ToString() == "1")
            return "I";
        if (number.ToString() == "2")
            return "II";
        if (number.ToString() == "3")
            return "III";
        if (number.ToString() == "4")
            return "VI";
        if (number.ToString() == "5")
            return "V";
        if (number.ToString() == "6")
            return "VI";
        if (number.ToString() == "7")
            return "VII";
        if (number.ToString() == "8")
            return "VIII";
        if (number.ToString() == "9")
            return "IX";
        return "";
    }
}
