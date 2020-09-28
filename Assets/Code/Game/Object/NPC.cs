using System.Collections.Generic;
using UnityEngine;

public class NPC : GameEvent
{
    public override void Initialize()
    {
        Animator = GetComponent<Animator>();
        Type = "NPC";
    }

    public override void EventAction()
    {
        //EventManager.AddSyncCommand("Message,101,102");
    }
}