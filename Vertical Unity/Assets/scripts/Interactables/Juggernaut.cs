using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Juggernaut : Interactuable
{
    public int cost;
    public AudioClip cashClip;
    public float hpMultiplyer;

    public override void Interact(FPSController player)
    {
        if (!player.HasScore(cost))
            return;
        base.Interact(player);
        player.Buy(cost);
        EventManager.current.PlayEffect(cashClip);
        player.maxhp *= hpMultiplyer;
        used = true;
        WeaponInfo_UI.instance.interactionText.gameObject.SetActive(false);
    }
    public override string GetMessage(FPSController player)
    {
        string m = base.GetMessage(player);
        m = "Buy Strong Health " + cost.ToString();
        return m;
    }
}
