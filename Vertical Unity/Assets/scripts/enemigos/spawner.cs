using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
   
    public GameObject enemy;
    public float spawnInterval;
    private float waveInterval = 15f;
    private int enemiesSpawned;
    public List<GameObject> spawnPositions = new List<GameObject>();
    private int maxEnemies = 1;
    public int killedEnemies;
    public int round;
    [SerializeField]
    private float x_range;
    [SerializeField]
    private float y_range;
    private void Awake()
    {
        spawnPositions.AddRange(GameObject.FindGameObjectsWithTag("SpawnPoint"));
    }
    void Start()
    {
        EventManager.current.OnKillEvent.AddListener(KillEnemy);
        StartCoroutine(SpawnEnemy(spawnInterval, enemy));
    }

    private IEnumerator SpawnEnemy(float interval, GameObject enemy) 
    {
        while (true)
        {
            if (enemiesSpawned < maxEnemies)
            {
                Transform spawnT = spawnPositions[Random.Range(0, spawnPositions.Count)].transform;
                GameObject newEnemy = Instantiate(enemy, spawnT);
                
                newEnemy.transform.localPosition = Vector3.zero;
                newEnemy.transform.SetParent(null);
                enemiesSpawned++;
            }
            yield return new WaitForSeconds(interval);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, new Vector3(x_range, y_range, 1));
    }
    public void KillEnemy() 
    {
        killedEnemies++;
        if (killedEnemies >= maxEnemies) 
        {
            round++;
            maxEnemies = 6 + 4 * round;
            killedEnemies = 0;
            StartCoroutine(SpawnEnemy(spawnInterval, enemy));
            EventManager.current.UpdateRoundEvent.Invoke(round);
        }
    }

}
