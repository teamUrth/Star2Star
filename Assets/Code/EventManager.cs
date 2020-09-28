using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;
    private static Coroutine _showTextCoroutine;
    private static Coroutine _wait;
    private static Queue<string> _syncCommands = new Queue<string>(); // 동기식 커맨드 큐
    // 동기식은 해당 이벤트가 끝날 때 까지 다른 이벤트를 진행하지 못하는 커맨드
    private static Queue<string> _asyncCommands = new Queue<string>(); // 비동기식 커맨드 큐
    // 비동기식은 이벤트의 대기 없이 다른 이벤트를 진행하는 커맨드

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // 매 프레임마다 실행
    void Update()
    {
        // 비동기식 커맨드가 있으면 커맨드를 실행
        if (_asyncCommands.Count > 0)
        {
            Command(_asyncCommands.Dequeue());
        }
        
        // 동기식 커맨드가 실행되어 있다면 메소드 종료
        if (GameManager.isAction)
        {
            return;
        }

        // 동기식 커맨드가 있으면 커맨드를 실행
        if (_syncCommands.Count > 0)
        {
            Command(_syncCommands.Dequeue()); 
        }
    }

    // 동기식 커맨드를 추가하는 부분
    public static void AddSyncCommand(string command)
    {
        _syncCommands.Enqueue(command);
    }

    // 비동기식 커맨드를 추가하는 부분
    public static void AddAsyncCommand(string command)
    {
        _asyncCommands.Enqueue(command);
    }

    // 커맨드를 인자로 받아서 실제 실행하는 부분
    private void Command(string command)
    {
        Debug.Log(command + "를 실행합니다.");

        // 입력받은 커맨드를 ","로 구분해서 파싱하여 문자열 배열로 commands에 저장한다.
        var commands = Regex.Split(command, ",");
        if (commands[0].CompareTo("Message") == 0)
        {
            var texts = new Queue<int>();
            for (var i = 1; i < commands.Length; i++)
            {
                texts.Enqueue(Convert.ToInt32(commands[i]));
            }
            UIManager.MessageManager.Message(texts);
        }
        else if (commands[0].CompareTo("Image") == 0)
        {
            UIManager.SetImage(commands[1]);
        }
        else if (commands[0].CompareTo("HideImage") == 0)
        {
            UIManager.HideImage();
        }
        else if (commands[0].CompareTo("SetBlack") == 0)
        {
            UIManager.SetBlack();
        }
        else if (commands[0].CompareTo("FadeIn") == 0)
        {
            UIManager.FadeIn();
        }
        else if (commands[0].CompareTo("FadeOut") == 0)
        {
            UIManager.FadeOut();
        }
        else if (commands[0].CompareTo("Wait") == 0)
        {
            WaitAction(Convert.ToInt32(commands[1]));
        }
        else if (commands[0].CompareTo("TextAlign") == 0)
        {
            if (commands[1].CompareTo("Left") == 0)
            {
                UIManager.MessageManager.SetTextAlign(TextAnchor.UpperLeft);
            }
            else if (commands[1].CompareTo("Center") == 0)
            {
                UIManager.MessageManager.SetTextAlign(TextAnchor.UpperCenter);
            }
            else if (commands[1].CompareTo("Right") == 0)
            {
                UIManager.MessageManager.SetTextAlign(TextAnchor.UpperRight);
            }
            else if (commands[1].CompareTo("MiddleCenter") == 0)
            {
                UIManager.MessageManager.SetTextAlign(TextAnchor.MiddleCenter);
            }
        }
        else if (commands[0].CompareTo("TextColor") == 0)
        {
            UIManager.MessageManager.SetTextColor(commands[1]);
        }
        else if (commands[0].CompareTo("DialogImageOff") == 0)
        {
            UIManager.MessageManager.SetDialogImage(false);
        }
        else if (commands[0].CompareTo("DialogImageOn") == 0)
        {
            UIManager.MessageManager.SetDialogImage(true);
        }
        else if (commands[0].CompareTo("DialogPosition") == 0)
        {
            if (commands[1].CompareTo("Up") == 0)
            {
                UIManager.MessageManager.SetDialogPosition(MessageManager.DialogPosition.Up);
            }
            else if (commands[1].CompareTo("Middle") == 0)
            {
                UIManager.MessageManager.SetDialogPosition(MessageManager.DialogPosition.Middle);
            }
            else if (commands[1].CompareTo("Down") == 0)
            {
                UIManager.MessageManager.SetDialogPosition(MessageManager.DialogPosition.Down);
            }
            else if (commands[1].CompareTo("VeryDown") == 0)
            {
                UIManager.MessageManager.SetDialogPosition(MessageManager.DialogPosition.VeryDown);
            }
        }

        if (commands[0].CompareTo("SetCamera") == 0)
        {
            if (commands[1].CompareTo("Camera") == 0)
            {
                GameManager.Camera.SetCamera();
            }
            else if (commands[1].CompareTo("Player") == 0)
            {
                GameManager.Camera.SetCameraPlayer();
            }
        }

        
        if (commands[0].CompareTo("Set") == 0)
        {
            GameManager.Camera.gameObject.transform.position = new Vector3(float.Parse(commands[1]), float.Parse(commands[2]));
        }
        if (commands[0].CompareTo("SetPlayer") == 0)
        {
            GameManager.PlayerCode.gameObject.transform.position = new Vector3(float.Parse(commands[1]), float.Parse(commands[2]));
        }
        if (commands[0].CompareTo("MoveUp") == 0)
        {
            GameManager.Camera.Command(Vector3.up, true);
        }
        else if (commands[0].CompareTo("MoveDown") == 0)
        {
            GameManager.Camera.Command(Vector3.down, true);
        }
        else if (commands[0].CompareTo("MoveLeft") == 0)
        {
            GameManager.Camera.Command(Vector3.left, true);
        }
        else if (commands[0].CompareTo("MoveRight") == 0)
        {
            GameManager.Camera.Command(Vector3.right, true);
        }

        /*
        if (commands[0].CompareTo("MoveUp") == 0)
        {
            if (int.TryParse(commands[1], out int n))
            {
                GameEvent.GameEvents[n].Command(Vector3.up);
            }
            else if (commands[1].CompareTo("Camera") == 0)
            {

            }
            GameEvent.GameEvents[Convert.ToInt32(commands[1])].Command(Vector3.up, true);
        }
        if (commands[0].CompareTo("MoveDown") == 0)
        {
            GameEvent.GameEvents[Convert.ToInt32(commands[1])].Command(Vector3.down, true);
        }
        if (commands[0].CompareTo("MoveLeft") == 0)
        {
            GameEvent.GameEvents[Convert.ToInt32(commands[1])].Command(Vector3.left, true);
        }
        if (commands[0].CompareTo("MoveRight") == 0)
        {
            GameEvent.GameEvents[Convert.ToInt32(commands[1])].Command(Vector3.right, true);
        }*/
        if (commands[0].CompareTo("SetBool") == 0)
        {
            Debug.Log(bool.Parse(commands[2]));
            GameManager.PlayerCode.Animator.SetBool(commands[1].Trim(), bool.Parse(commands[2].Trim()));
        }
        else if(commands[0].CompareTo("AddStar") == 0)
        {
            GameManager.Star++;
        }

        if (commands[0].CompareTo("ShakeScreen") == 0)
        {
            Debug.Log(int.Parse(commands[1]));
            //GameManager.ShakeScreen(int.Parse(commands[1]), int.Parse(commands[2]));
            GameManager.ShakeScreen(int.Parse(commands[1]), int.Parse(commands[2]));
        }
    }

    public static void Break(BoxCollider2D gam)
    {
        Destroy(gam);
    }

    public static void WaitAction(float time)
    {
        _wait = Instance.StartCoroutine(Wait(time));
    }

    public static void CancleWaitAction()
    {
        Instance.StopCoroutine(_wait);
        GameManager.isAction = false;
    }

    private static IEnumerator Wait(float time)
    {
        GameManager.isAction = true;
        yield return new WaitForSecondsRealtime(time);
        GameManager.isAction = false;
    }

    public delegate void MethodDelegate();

    public static void Method(MethodDelegate method)
    {
        Instance.StartCoroutine(WaitMethod(method));
    }

    private static IEnumerator WaitMethod(MethodDelegate method)
    {
        while (GameManager.isAction)
        {
            yield return null;
        }
        method();
    }

    public void StopShowTextCoroutine()
    {
        StopCoroutine(_showTextCoroutine);
    }

    public void StartShowTextCoroutine(string text)
    {
        _showTextCoroutine = StartCoroutine(UIManager.MessageManager.ShowText(text));
    }
}