using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyEnd : Interactuable
{
    public int cost;

    public override void Interact(FPSController player)
    {
        if (!player.HasScore(cost))
            return;
        base.Interact(player);
        player.Buy(cost);
        EventManager.current.EndGame();
        used = true;
        WeaponInfo_UI.instance.interactionText.gameObject.SetActive(false);
    }
    public override string GetMessage(FPSController player)
    {
        string m = base.GetMessage(player);
        m = "End Game " + cost.ToString();
        return m;
    }
}
