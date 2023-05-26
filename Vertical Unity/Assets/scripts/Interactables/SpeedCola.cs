using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedCola : Interactuable
{
    public int cost;
    public AudioClip cashClip;
    void Start()
    {
        
    }

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
        EventManager.current.PlayEffect(cashClip);
        player.reloadSpeedMult = 5;
        used = true;
        WeaponInfo_UI.instance.interactionText.gameObject.SetActive(false);
    }
    public override string GetMessage(FPSController player)
    {
        string m = base.GetMessage(player);
        m = "Buy Fast Reload " + cost.ToString();
        return m;
    }
}
