using UnityEngine;

public class Plant : GameEvent
{
    private int _level;
    public override void Initialize()
    {
        Animator = GetComponent<Animator>();
        Type = "Plant";
        _level = 0;
        
    }

    public override void EventAction()
    {
        if (GameManager.PlayerCode.PowerType == Power.Aquarius)
        {
            ++_level;
            if (_level == 1)
            {
                isNotPass = true;
            }
            else
            {
                isNotPass = false;
            }
            Animator.SetInteger("Grow", _level);
        }
        //var playerDirection = GameManager.PlayerCode.GetDirection();
    }
}