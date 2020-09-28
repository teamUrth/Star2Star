using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : MonoBehaviour {
    public static Dictionary<int, GameEvent> GameEvents = new Dictionary<int, GameEvent>(); // 전체 이벤트를 관리하는 리스트
    public static int IDCount;
    public int Id; // 이벤트의 아이디
    public int Speed = 1; // 이동 속도
    public int Frequency = 1; // 이동 빈도
    public float Sight = 1; // 시야
    public string Type; // 이 이벤트의 종류 "Player", "Box"

    public bool isNotPass = false; // 이 이벤트와 겹쳐질 수 없으면 true
    public bool isFixedDirection = false; // 방향전환이 불가능하면 true
    public bool isFixedMove = false; // 걸음이 없으면 true
    public bool isMove = false; // 움직이는 중 일때 true
    public bool isAction = false;
    public bool isAnimationFixed = false; // 애니메이터에서 반복시킬 때

    public Animator Animator = null;
    public RaycastHit2D[] Scan;
    public Vector3 Direction;

    protected Queue<string> _syncCommands = new Queue<string>();
    protected List<string> _asyncCommands = new List<string>();
    protected int _commandIndex = 0;

    void Awake()
    {
        _syncCommands = new Queue<string>();
        _asyncCommands = new List<string>();
        Id = GameEvents.Count;
        GameEvents.Add(GameEvents.Count, this);
        Speed = 1;
        Frequency = 1;
        Sight = 1;
        Initialize();
    }

    void Update()
    {
        if (isAction)
        {
            return;
        }
        if (_syncCommands.Count > 0)
        {
            Debug.Log("실행함");
            EventManager.AddSyncCommand(_syncCommands.Dequeue() + "," + Id);
            return;
        }
        if (_asyncCommands.Count > 0)
        {
            if (_commandIndex >= _asyncCommands.Count)
            {
                _commandIndex = 0;
            }
            EventManager.AddAsyncCommand(_asyncCommands[_commandIndex++] + "," + Id);
        }
    }

    public virtual void Initialize()
    {
    }
    
    public virtual void EventAction()
    {
    }

    public Vector3 GetDirection()
    {
        if (Direction == Vector3.left)
        {
            return Vector3.left;
        }
        else if (Direction == Vector3.right)
        {
            return Vector3.right;
        }
        else if (Direction == Vector3.down)
        {
            return Vector3.down;
        }
        else if (Direction == Vector3.up)
        {
            return Vector3.up;
        }
        return Vector3.zero;
    }

    public void Command(Vector3 dir)
    {
        StartCoroutine(Move(dir));
    }

    public void Command(Vector3 dir, bool value)
    {
        StartCoroutine(Move(dir, value));
    }

    // 이벤트가 현재 어떤 콜라이더 위에 올라가있는지 반환하는 메소드
    public RaycastHit2D[] GetTag()
    {
        return Physics2D.RaycastAll(transform.position, Vector3.zero);
    }

    public bool IsPassable(Vector3 direction)
    {
        var check = true;
        var tagList = new List<TileCollider>();
        var evnetList = new List<GameEvent>();

        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position + direction, direction, Sight);
        for (var i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider)
            {
                tagList.Add((TileCollider)hit[i].collider.gameObject.layer);
                if (hit[i].collider.gameObject.layer == (int)TileCollider.Event)
                {
                    if (hit[i].collider.gameObject.GetComponent<GameEvent>().isNotPass)
                    {
                        return false;
                    }
                }
            }
        }
        return check;
    }

    // 한 타일만큼 이동하는 코루틴
    private IEnumerator Move(Vector3 direction)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(.1f - Frequency * .02f);
        var count = GameManager.PPU / Speed;
        
        isMove = true; // 행동중을 참으로

        // 애니메이터가 있을 때 모션을 취하도록
        if (Animator != null && !isFixedMove)
        {
            Animator.SetBool("Moving", true);
        }

        // 한 타일을 이동하도록 함
        for (var i = 0; i < count; i++)
        {
            transform.Translate(direction * GameManager.Pixel * Speed);
            yield return waitForSeconds;
        }

        // 애니메이터가 있을 때 모션을 종료하도록
        if (Animator != null && !isFixedMove)
        {
            Animator.SetBool("Moving", false);
        }
        isMove = false;
    }

    // 한 타일만큼 이동하는 코루틴
    private IEnumerator Move(Vector3 direction, bool value)
    {
        Debug.Log("루틴실행중");
        WaitForSeconds waitForSeconds = new WaitForSeconds(.1f - Frequency * .02f);
        var count = GameManager.PPU / Speed;
        if (value)
        {
            GameManager.isAction = true;
        }
        isMove = true; // 행동중을 참으로

        // 애니메이터가 있을 때 모션을 취하도록
        if (Animator != null && !isFixedMove)
        {
            Animator.SetBool("Moving", true);
        }

        // 한 타일을 이동하도록 함
        for (var i = 0; i < count; i++)
        {
            transform.Translate(direction * GameManager.Pixel * Speed);
            yield return waitForSeconds;
        }

        // 애니메이터가 있을 때 모션을 종료하도록
        if (Animator != null && !isFixedMove)
        {
            Animator.SetBool("Moving", false);
        }
        if (value)
        {
            GameManager.isAction = false;
        }
        isMove = false;
    }
}