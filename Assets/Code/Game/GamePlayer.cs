using System.Collections.Generic;
using UnityEngine;

public enum Power { Default = 0, Pisces, Aquarius, Capricorn }

public class GamePlayer : GameEvent
{
    public Power PowerType = Power.Default; //0 = 사람 1 = 물고기 2 = 물병자리 3 = 염소
    private Vector3 _change;
    private int timer;
    private List<string> fronts = new List<string>();

    void Awake()
    {
        // 애니메이터와 방향과 기본속도를 설정
        Animator = GetComponent<Animator>();
        Direction = Vector3.down;
        Sight = 1;
        timer = 0;
        Speed = 3;
        Frequency = 5;
        Type = "Player";
    }

    public void SetPower(Power power)
    {
        PowerType = power;
        Animator.SetBool("Aquarius", false);
        Animator.SetBool("Pisces", false);
        Animator.SetBool("Capricorn", false);
        if (power == Power.Default)
        {
            
        }
        else if (power == Power.Pisces)
        {
            Animator.SetBool("Pisces", true);
        }
        else if (power == Power.Aquarius)
        {
            Animator.SetBool("Aquarius", true);
        }
        else if (power == Power.Capricorn)
        {
            Animator.SetBool("Capricorn", true);
        }
    }

    void Update()
    {
        timer++;
        if (1 / Time.deltaTime * 0.025 < timer) // 0.1초마다 이동
        {
            timer = 0;
            if (!isMove)
            {
                PlayerMove();
            }
        }
        _change = Vector3.zero;
    }

    // 입력핸들러
    public void InputHandler(Action action)
    {
        if (GameManager.isMenu == true || GameManager.isItemMenu == true)
        {
            UIManager.InputHandler(action);
        }
        else
        {
            if (GameManager.isTalking == false)
            {
                if (GameManager.isItemMenu == false)
                {
                    if (action == Action.UP)
                    {
                        _change.y = 1;
                    }
                    if (action == Action.DOWN)
                    {
                        _change.y = -1;
                    }
                    if (action == Action.LEFT)
                    {
                        _change.x = -1;
                    }
                    if (action == Action.RIGHT)
                    {
                        _change.x = 1;
                    }
                    if (action == Action.B)
                    {
                        UIManager.InputHandler(action);
                    }

                }
            }
        }
        if (action == Action.A && !isMove)
        {
            if (Scan != null)
            {
                //GameEvent data = scan.GetComponent<GameEvent>();
                for (int i = 0; i < Scan.Length; i++)
                {
                    if (Scan[i].collider.gameObject.layer == (int)TileCollider.Event) // 이벤트 일때
                    {
                        Scan[i].collider.gameObject.GetComponent<GameEvent>().EventAction();
                    }
                }
            }
        }
        if (action == Action.B)
        {
            GameMenu.Menu();
        }
    }

    void FixedUpdate()
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, Direction, Sight);
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider != null)
            {
                Scan = hit;
            }
            else
            {
                Scan = null;
            }
        }
        CheckMap();
    }

    public void PlayerMove()
    {
        if (_change != Vector3.zero)
        {
            RaycastHit2D[] hit;
            Vector3 movex = transform.position;
            Vector3 movey = transform.position;
            Vector3 move = transform.position;
            movex.x += _change.x * 0.5f;
            movey.y += _change.y * 0.5f;
            move.x += _change.x * 0.5f;
            move.y += _change.y * 0.5f;
            if (!isFixedDirection)
            {
                Animator.SetFloat("MoveX", _change.x);
                Animator.SetFloat("MoveY", _change.y);
            }

            Direction = _change;
            Animator.SetBool("Box", false);
            isFixedDirection = false;

            hit = Physics2D.LinecastAll(new Vector3(movex.x, movex.y - 0.4f), new Vector3(movex.x, movex.y + 0.4f));
            movex = transform.position;

            if (IsPassable(hit))
            {
                movex.x += _change.x * GameManager.Pixel * Speed;
            }

            hit = Physics2D.LinecastAll(new Vector3(movey.x + 0.4f, movey.y), new Vector3(movey.x - 0.4f, movey.y));
            movey = transform.position;
            if (IsPassable(hit))
            {
                movey.y += _change.y * GameManager.Pixel * Speed;
            }

            //움직임이 없다면
            if (transform.position.x == movex.x && transform.position.y == movey.y)
            {
                Animator.SetBool("Moving", false);
                return;
            }
            switch (PowerType)
            {
                case Power.Default:
                    Animator.SetBool("Moving", true);
                    break;
                case Power.Pisces: //물일때
                    Animator.SetBool("Fish", true);
                    break;
                default:
                    break;
            }
            transform.position = new Vector3(movex.x, movey.y);
            
            Animator.SetBool("Moving", true);
        }
        else
        {
            Animator.SetBool("Moving", false);
            Animator.SetBool("Box", false);
        }
    }

    bool IsPassable(RaycastHit2D[] hit)//통행가능하면 참을 반환하는 함수
    {
        // 앞에 있는 이벤트목록을 초기화
        fronts.Clear();
        bool check = true;
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.gameObject.layer == (int)TileCollider.Water)
            {
                fronts.Add("Water");
            }
            else if (hit[i].collider.gameObject.layer == (int)TileCollider.Wall)
            {
                return false;
                //fronts.Add("Water");
            }
            else if (hit[i].collider.gameObject.layer == (int)TileCollider.Event)
            {
                GameEvent gameEvent = hit[i].collider.gameObject.GetComponent<GameEvent>();
                if (gameEvent.Type != null && gameEvent.isNotPass)
                {
                    fronts.Add(gameEvent.Type);
                }
                else if (gameEvent.isNotPass)
                {
                    fronts.Add("NPC");
                }
            }
        }

        if (fronts.Contains("Box") && fronts.Contains("Water"))
        {
            fronts.Remove("Water");
            fronts.Remove("Box");
        }
        
        if (fronts.Contains("Box"))
        {
            Animator.SetFloat("MoveX", _change.x);
            Animator.SetFloat("MoveY", _change.y);
            Animator.SetBool("Box", true);
            isFixedDirection = true;
        }

        if (fronts.Count > 0)
        {
            for (var i = 0; i < fronts.Count; i++)
            {
                check = false;
            }
        }
        return check;
    }

    private void CheckMap()
    {
        if (!GameManager.isAction)
        {
            if (transform.position.x < MapManager.Map.X)
            {
                MapManager.Instance.ChangeMap(Vector3.left);
            }
            else if (transform.position.x > MapManager.Map.X + MapManager.Map.Width)
            {
                MapManager.Instance.ChangeMap(Vector3.right);
            }
            if (transform.position.y > MapManager.Map.Y)
            {
                MapManager.Instance.ChangeMap(Vector3.up);
            }
            else if (transform.position.y < -MapManager.Map.Height + MapManager.Map.Y)
            {
                MapManager.Instance.ChangeMap(Vector3.down);
            }
        }
    }
}