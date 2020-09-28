using System.Collections.Generic;
using UnityEngine;

public class Grand : GameEvent
{
    public override void Initialize()
    {
        Animator = GetComponent<Animator>();
        Type = "NPC";
    }

    public override void EventAction()
    {
        EventManager.AddSyncCommand("Message,25");
        //EventManager.AddSyncCommand("ShakeScreen,1,3");
    }
}