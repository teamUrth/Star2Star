using UnityEngine;

public class Star: GameEvent
{
    public override void Initialize()
    {
        Animator = GetComponent<Animator>();
        Type = "Star";
    }

    public override void EventAction()
    {
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
        gameObject.SetActive(false);
    }
}