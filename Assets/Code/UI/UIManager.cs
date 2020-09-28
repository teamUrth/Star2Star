using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public delegate void ButtonAction();
    public static GameMenu GameMenu;
    public static TitleManager TitleManager;
    public static ImageManager ImageManager;
    public static MessageManager MessageManager;
    private static Fade _fade;

    public List<Sprite> Images;

    public struct MenuButton
    {
        public GameObject image;
        public ButtonAction action;
    }

    private void Awake()
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

        GameMenu = new GameMenu();
        TitleManager = new TitleManager();
        ImageManager = new ImageManager();
        MessageManager = new MessageManager();

        _fade = new Fade();
    }

    public static void SetImage(string name)
    {
        for (var i = 0; i < Instance.Images.Count; i++)
        {
            if (Instance.Images[i] == null)
            {
                continue;
            }
            if (Instance.Images[i].name.Equals(name))
            {
                ImageManager.SetImage(Instance.Images[i]);
                return;
            }
        }
        Debug.Log(name + "이미지 파일을 찾을 수 없습니다.");
    }

    public static void HideImage()
    {
        ImageManager.HideImage();
    }

    public static void SetBlack()
    {
        _fade.SetBlack();
    }

    public static void FadeIn()
    {
        Instance.StartCoroutine(_fade.FaderIn());
    }

    public static void FadeOut()
    {
        Instance.StartCoroutine(_fade.FaderOut());
    }

    public static void InputHandler(Action action)
    {
        if (GameManager.isTalking)
        {
            if (action == Action.A)
            {
                if (MessageManager.isPrintMessage)
                {
                    MessageManager.Skip();
                }
                else
                {
                    MessageManager.Next();
                }
            }
        }
        else if (GameManager.isItemMenu)
        {
            GameMenu.InputHandler(action);
        }
    }
}