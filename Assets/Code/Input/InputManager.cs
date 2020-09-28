using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 게임에서의 조작키
public enum Action { UP, DOWN, LEFT, RIGHT, A, B, SPACE, KEYCOUNT }

public class InputManager : MonoBehaviour
{
    public delegate void ButtonAction();
    public static int selectedButton = 0;
    // 이 객체의 싱글턴
    public static InputManager instance;

    // 조작키와 키 코드를 연결해주는 딕셔너리
    Dictionary<Action, KeyCode> key = new Dictionary<Action, KeyCode>();
    private KeyCode[] defaultkey = new KeyCode[]
    {
        KeyCode.UpArrow,
        KeyCode.DownArrow,
        KeyCode.LeftArrow,
        KeyCode.RightArrow,
        KeyCode.Z,
        KeyCode.X,
        KeyCode.Space
    };

    void Awake()
    {
        //인스턴스 생성
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
        KeyInit();
    }

    private void KeyInit()
    {
        for (int i = 0; i < (int)Action.KEYCOUNT; i++)
        {
            key.Add((Action)i, defaultkey[i]);
        }
    }

    private void InputHandler(Action action)
    {
        GameManager.Instance.InputHandler(action);
    }

    void Update()
    {
        if (GameManager.isMenu == true || GameManager.isItemMenu == true)
        {
            if (Input.GetKeyUp(key[Action.UP]))
            {
                InputHandler(Action.UP);
            }
            if (Input.GetKeyUp(key[Action.DOWN]))
            {
                InputHandler(Action.DOWN);
            }
            if (Input.GetKeyUp(key[Action.LEFT]))
            {
                InputHandler(Action.LEFT);
            }
            if (Input.GetKeyUp(key[Action.RIGHT]))
            {
                InputHandler(Action.RIGHT);
            }
            if (Input.GetKeyUp(key[Action.SPACE]))
            {
                InputHandler(Action.SPACE);
            }
            if (Input.GetKeyUp(key[Action.A]))
            {
                InputHandler(Action.A);
            }
            if (Input.GetKeyUp(key[Action.B]))
            {
                InputHandler(Action.B);
            }
        }
        else
        {
            if (Input.GetKey(key[Action.SPACE]))
            {
                InputHandler(Action.SPACE);
            }
            if (Input.GetKey(key[Action.UP]))
            {
                InputHandler(Action.UP);
            }
            if (Input.GetKey(key[Action.DOWN]))
            {
                InputHandler(Action.DOWN);
            }
            if (Input.GetKey(key[Action.LEFT]))
            {
                InputHandler(Action.LEFT);
            }
            if (Input.GetKey(key[Action.RIGHT]))
            {
                InputHandler(Action.RIGHT);
            }
            if (Input.GetKeyUp(key[Action.A]))
            {
                InputHandler(Action.A);
            }
            if (Input.GetKeyUp(key[Action.B]))
            {
                InputHandler(Action.B);
            }
        }
    }
}