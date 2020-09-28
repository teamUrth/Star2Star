using UnityEngine;

public class Box : GameEvent
{
    private bool isBroken; // 파괴 됐는지
    private bool isFallInWater; // 물에 빠졌는지

    void FixedUpdate()
    {
        if (!isMove)
        {
            if (!isFallInWater)
            {
                RaycastHit2D[] hit = GetTag();
                for (var i = 0; i < hit.Length; i++)
                {
                    if (hit[i].collider.gameObject.layer == (int)TileCollider.Water)
                    {
                        FallInWater();
                    }
                }
            }
        }   
    }

    public override void Initialize()
    {
        Animator = GetComponent<Animator>();
        Type = "Box";
        isBroken = false;
        isFallInWater = false;
    }

    // 상자를 미는 행동
    private void Push()
    {
        var playerFrequency = GameManager.PlayerCode.Frequency;
        var playerSpeed = GameManager.PlayerCode.Speed;
        GameManager.PlayerCode.Frequency = Frequency;
        GameManager.PlayerCode.Speed = Speed;
        var playerDirection = GameManager.PlayerCode.GetDirection();
        if (playerDirection != null)
        {
            if (IsPassable(GameManager.PlayerCode.Direction))
            {
                Command(playerDirection);
                GameManager.PlayerCode.Command(playerDirection);
            }
        }
        GameManager.PlayerCode.Frequency = playerFrequency;
        GameManager.PlayerCode.Speed = playerSpeed;
    }

    // 상자가 파괴되는 행동
    private void Break()
    {
        isBroken = true;
        isNotPass = false;
        Animator.SetBool("Break", true);
    }

    private void FallInWater()
    {
        isFallInWater = true;
        isNotPass = false;
        Animator.SetBool("Water", true);
        Debug.Log((int)(transform.position.x - .5f) + "과" + (int)(-transform.position.y - .5f));
        MapManager.DeleteCollider((int)(-MapManager.Map.X + transform.position.x - .5f), (int)(MapManager.Map.Y - transform.position.y - .5f));
        GetComponent<SpriteRenderer>().sortingLayerID = 0;
        GetComponent<SpriteRenderer>().sortingOrder = 1;
    }

    public override void EventAction()
    {
        if (isBroken || isFallInWater)
        {
            return;
        }

        if (GameManager.PlayerCode.PowerType == Power.Default)
        {
            Push();
        }
        else if (GameManager.PlayerCode.PowerType == Power.Capricorn)
        {
            Break();
        }
    }
}
