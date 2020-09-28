using System.Collections.Generic;
using UnityEngine;

public class Fountain : GameEvent
{
    bool _self = true;
    public override void Initialize()
    {
        Animator = GetComponent<Animator>();
        Type = "NPC";
    }

    public override void EventAction()
    {
        if (_self)
        {
            _self = false;
            EventManager.AddSyncCommand("Message,22");
            EventManager.AddSyncCommand("Message,23");
            EventManager.AddSyncCommand("SetBool,Star,true");
            EventManager.AddSyncCommand("DialogImageOn");
            EventManager.AddSyncCommand("DialogPosition,Down");
            EventManager.AddSyncCommand("TextAlign,MiddleCenter");
            EventManager.AddSyncCommand("Message,19");
            EventManager.AddSyncCommand("DialogImageOn");
            EventManager.AddSyncCommand("DialogPosition,Down");
            EventManager.AddSyncCommand("TextAlign,Left");
            EventManager.AddSyncCommand("SetBool,Star,false");
            EventManager.AddSyncCommand("AddStar");
        }
        else
        {
            EventManager.AddSyncCommand("Message,24");
        }   
    }
}