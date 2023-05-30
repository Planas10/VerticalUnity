using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : Interactuable
{
    public int cost;
    public AudioClip cashClip;

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Interact(FPSController player)
    {
        if (!player.HasScore(cost))
            return;
        base.Interact(player);
        player.Buy(cost);
        WeaponInfo_UI.instance.interactionText.gameObject.SetActive(false);
        EventManager.current.PlayEffect(cashClip);
        EventManager.current.player.InteractAction = null;
        Destroy(gameObject);
    }
    public override string GetMessage(FPSController player)
    {
      string m = base.GetMessage(player);
      m = "Open " + cost.ToString();
     return m;
    }
}
