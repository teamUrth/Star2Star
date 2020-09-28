using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Language { Korean = 0, English, Japanse };
public delegate void MyDelegate(Action action);

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // 싱글턴 객체

    public static readonly int PPU = 16;
    public static readonly float Pixel = 1f / PPU;

    public static int Language = 0;
    public static int Star = 0;
    public static bool isTransition = false;
    public static bool isMenu = false;
    public static bool isItemMenu = false;
    public static bool isImage = false;
    public static bool isTalking = false;
    public static bool MapChange = false;
    public static bool isAction = false; //
    public static bool isFadeOut = false; // 페이드 아웃 돼 있을 때 true
    public static PerfectPixelCamera Camera; // 카메라 코드
    public static GamePlayer PlayerCode; // 플레이어 코드
    private static GameObject _player; // 플레이어 오브젝트

    private MyDelegate _input; // 입력을 넘겨주기위한 델리게이트
    
    public Font[] Fonts;

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
        Initialize();
    }

    // 게임 내에서 가장 먼저 실행되는 부분
    // F12를 누르면 해당 메소드의 정의부분으로 이동할 수 있다.
    private void Start()
    {
        UIManager.TitleManager.Test();
    }

    private void Initialize()
    {
        // 폰트가 Pixel Perfect 하게 보이도록 설정
        for (var i = 0; i < Fonts.Length; i++)
        {
            Fonts[i].material.mainTexture.filterMode = FilterMode.Point; // 없어도 똑같은듯
        }

        // 플레이어 객체 검색, 초기화 하는 과정
        _player = GameObject.Find("Player");
        PlayerCode = _player.gameObject.GetComponent<GamePlayer>();

        _input = new MyDelegate(PlayerCode.InputHandler);
        
        Camera = GameObject.Find("MainCamera").GetComponent<PerfectPixelCamera>();
    }

    private void Update()
    {
        if (Star >= 4)
        {
            EventManager.AddSyncCommand("Wait,1");
            EventManager.AddSyncCommand("FadeOut");
            EventManager.AddSyncCommand("Image,Ending");
            EventManager.AddSyncCommand("Message,26");
            EventManager.AddSyncCommand("FadeIn");
            EventManager.AddSyncCommand("Wait,1");
            EventManager.AddSyncCommand("Message,27,28,29,30");
            Star = 0;
        }
    }

    public void InputHandler(Action action)
    {
        if (isTalking)
        {
            UIManager.InputHandler(action);
            return;
        }
        if (isAction)
        {
            return;
        }
        if (isItemMenu)
        {
            UIManager.InputHandler(action);
            return;
        }
        if (isImage)
        {
            UIManager.InputHandler(action);
            return;
        }
        if (MapChange)
        {

        }
        else
        {
            _input(action);
        }
    }

    public static void ScrollMap(Vector3 direction)
    {
        Camera.Scroll(direction);
    }

    public static void ScrollMap(Vector3 direction, string name)
    {
        Camera.Scroll(direction, name);
    }

    public static void ShakeScreen(int value, int duration)
    {
        Debug.Log(value + " : " + duration);
        Camera.ShakeScreen(value, duration);
    }
}

public struct GameSetting
{
    public static Language GameLanguage = Language.Korean;
    public static int Volume = 100;
    public static int SEVolume = 100;
}