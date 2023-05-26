using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactuable : MonoBehaviour
{
    public string message;
    public bool used;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public virtual void Interact(FPSController player) 
    {

    }
    public virtual string GetMessage(FPSController player) 
    {
        return message;
    }
}
