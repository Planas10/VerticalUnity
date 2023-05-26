using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

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

    public Int2Event updateBulletsEvent = new Int2Event();
    public UnityEvent<int> UpdateScoreEvent = new UnityEvent<int>();
    public UnityEvent<int> UpdateRoundEvent = new UnityEvent<int>();
    public UnityEvent<int> OnGainPoints = new UnityEvent<int>();
    public UnityEvent OnKillEvent = new UnityEvent();
    public UnityEvent OnHitEvents = new UnityEvent();

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
}
