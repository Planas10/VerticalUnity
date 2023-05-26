using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponInfo_UI : MonoBehaviour
{
    public static WeaponInfo_UI instance;
    public TMP_Text CurrentBullets;
    public TMP_Text TotalBullets;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI interactionText;
    public Transform spawnPointsPoint;
    public GameObject scorePrefab;
    public Animator hit;
    private void Awake()
    {
        instance = this;
        EventManager.current.updateBulletsEvent.AddListener(UpdateBullets);
        EventManager.current.UpdateScoreEvent.AddListener(UpdateScore);
        EventManager.current.UpdateRoundEvent.AddListener(UpdateRound);
        EventManager.current.OnGainPoints.AddListener(SpawnScoreText);
        EventManager.current.OnHitEvents.AddListener(PlayHit);
    }

    //convertir el int de las balas a string
    public void UpdateBullets(int newCurrentBullet, int newTotalBullet) {
        CurrentBullets.text = newCurrentBullet.ToString();
        TotalBullets.text = newTotalBullet.ToString();
    }
    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }
    public void UpdateRound(int round)
    {
        roundText.text = "Round " + round.ToString();
    }
    public void SpawnScoreText(int score) 
    {
        GameObject obj = Instantiate(scorePrefab, spawnPointsPoint);
        obj.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0) + (Vector3)Random.insideUnitCircle * 25;
        obj.GetComponent<TextMeshProUGUI>().text = "+" + score;
        obj.GetComponent<Rigidbody2D>().AddForce(Vector2.up * Random.Range(100f, 120f) + Vector2.right * Random.Range(20, 50),ForceMode2D.Impulse);
        float lifetime = Random.Range(1.25f, 1.5f);
        obj.GetComponent<Animator>().speed = 1 / lifetime;
        Destroy(obj, lifetime);
    }
    public void SpawnScoreTextRed(int score)
    {
        GameObject obj = Instantiate(scorePrefab, spawnPointsPoint);
        obj.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0) + (Vector3)Random.insideUnitCircle * 25;
        obj.GetComponent<TextMeshProUGUI>().text = "-" + score;
        obj.GetComponent<TextMeshProUGUI>().color = Color.red;
        obj.GetComponent<Rigidbody2D>().AddForce(Vector2.up * Random.Range(100f, 120f) + Vector2.right * Random.Range(20, 50), ForceMode2D.Impulse);
        float lifetime = Random.Range(1.25f, 1.5f);
        obj.GetComponent<Animator>().speed = 1 / lifetime;
        Destroy(obj, lifetime);
    }
    public void SetInteractionText(string text) 
    {
        interactionText.gameObject.SetActive(true);
        interactionText.text = "Press E to " + text;
    }
    public void PlayHit() 
    {
        hit.SetTrigger("Hit");
    }
}
