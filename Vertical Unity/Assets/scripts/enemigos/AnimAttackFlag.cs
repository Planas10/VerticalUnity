using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimAttackFlag : MonoBehaviour
{
    public AI_Enemy enemy;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void endAttack() 
    {
        enemy.CheckEnd();
    }
}
