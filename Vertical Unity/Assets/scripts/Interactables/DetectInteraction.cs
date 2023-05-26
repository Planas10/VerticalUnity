using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectInteraction : MonoBehaviour
{
    FPSController player;
    private void Awake()
    {
        player = transform.parent.GetComponent<FPSController>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider collision)
    {
        Interactuable interactuable = collision.GetComponent<Interactuable>();
        if (interactuable) 
        {
            if (interactuable.used)
                return;
            WeaponInfo_UI.instance.SetInteractionText(interactuable.GetMessage(player));
            player.InteractAction = interactuable.Interact;
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        Interactuable interactuable = collision.GetComponent<Interactuable>();
        if (interactuable)
        {
            if (player.InteractAction == interactuable.Interact) 
            {
                WeaponInfo_UI.instance.interactionText.gameObject.SetActive(false);
                player.InteractAction = null;
            }
        }
    }
}
