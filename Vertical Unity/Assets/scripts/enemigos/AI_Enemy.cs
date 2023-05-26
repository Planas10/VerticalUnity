using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum EnemyState
{
    chasing,
    attacking
}
public class AI_Enemy : MonoBehaviour
{
    private int maxHp = 10;
    private int hp;
    
    public Transform Objetivo;
    public float Velocidad;
    public NavMeshAgent IA;

    public Animator Anim;
    public string NombreAnimacionCaminar;
    public string NombreAnimacionAtacar;
    public int diePoints = 60;
    public float attackRange = 5;
    EnemyState state;
    private void Awake()
    {
        hp = maxHp;
        FindTarget();
    }
    void Update()
    {
        Anim.SetBool("Attack", state == EnemyState.attacking);
        Anim.SetBool("Running", IA.speed > 0);
        if (state == EnemyState.chasing)
        {
            FindTarget();
            if (Vector3.Distance(transform.position, EventManager.current.player.transform.position) <= attackRange)
            {
                state = EnemyState.attacking;
                IA.speed = 0;
            }
        }
    }
    public void CheckEnd()
    {
        if (!(Vector3.Distance(transform.position, EventManager.current.player.transform.position) <= attackRange))
            state = EnemyState.chasing;
    }
    public void ReciveDamage(int damage) 
    {
        hp -= damage;
        print("HP: " + hp + " Dmg: " + damage);
        if (hp <= 0)
        {
            EventManager.current.player.IncresePoints(diePoints);
            EventManager.current.OnKillEvent.Invoke();
            Destroy(gameObject);
        }
    }
    public void FindTarget() 
    {
        Objetivo = EventManager.current.player.transform;
        IA.SetDestination(Objetivo.position);
        IA.speed = Velocidad;
    }
}
