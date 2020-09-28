using System.Collections.Generic;
using UnityEngine;

public class Chicken : GameEvent
{
    public override void Initialize()
    {
        Animator = GetComponent<Animator>();
        Type = "NPC";
    }

    public override void EventAction()
    {
        EventManager.AddSyncCommand("Message,21");
    }
}