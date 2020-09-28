using System.Collections.Generic;
using UnityEngine;
using static UIManager;

public class TitleManager
{
    public static MenuButton[] buttonList;
    public static int selectedButton = 0;
    private GameObject TitlePannel;

    public void InputHandler(Action action)
    {
        if (action == Action.DOWN)
        {
            MoveToNextButton();
        }
        if (action == Action.UP)
        {
            MoveToPreviousButton();
        }
        if (action == Action.A)
        {
            buttonList[selectedButton].action();
        }
    }

    public TitleManager()
    {
        TitlePannel = GameObject.Find("Title");
        /*
        buttonList = new MenuButton[3];
        // 새 게임 시작
        buttonList[0].image = GameObject.Find("NewGameBtn");
        GameObject select = GameObject.Find("NewGameBtn").transform.Find("select").gameObject;
        select.SetActive(false);
        buttonList[0].action = NewGameBtnAction;

        //불러오기
        buttonList[1].image = GameObject.Find("GetSavedBtn");
        select = GameObject.Find("GetSavedBtn").transform.Find("select").gameObject;
        select.SetActive(false);
        buttonList[1].action = GetSavedBtnAction;

        //옵션
        buttonList[2].image = GameObject.Find("OptionBtn");
        select = GameObject.Find("OptionBtn").transform.Find("select").gameObject;
        select.SetActive(false);
        buttonList[2].action = OptionBtnAction;*/
        TitlePannel.SetActive(false);
    }

    public void Test()
    {

        //EventManager.AddSyncCommand("Wait,10");
        //EventManager.AddSyncCommand("ShakeScreen,5,1");
    }

    public void Intro()
    {
        EventManager.AddSyncCommand("TextAlign,Center");
        EventManager.AddSyncCommand("SetBlack");
        EventManager.AddSyncCommand("Wait,2");
        EventManager.AddSyncCommand("Image,Logo");
        EventManager.AddSyncCommand("FadeIn");
        EventManager.AddSyncCommand("Wait,2");
        EventManager.AddSyncCommand("FadeOut");
        EventManager.Method(Prologue);
    }

    public void Prologue()
    {
        EventManager.AddSyncCommand("SetCamera,Camera");
        EventManager.AddSyncCommand("TextColor,White");
        EventManager.AddSyncCommand("DialogImageOff");
        EventManager.AddSyncCommand("DialogPosition,VeryDown");

        EventManager.AddSyncCommand("FadeOut");
        EventManager.AddSyncCommand("Image,Prologue1");
        EventManager.AddSyncCommand("Wait,2");
        EventManager.AddSyncCommand("FadeIn");
        EventManager.AddSyncCommand("Wait,1");

        EventManager.AddSyncCommand("Message,0,1");
        EventManager.AddSyncCommand("Wait,1");

        EventManager.AddSyncCommand("FadeOut");
        EventManager.AddSyncCommand("Image,Prologue2");
        EventManager.AddSyncCommand("Wait,1");
        EventManager.AddSyncCommand("FadeIn");
        EventManager.AddSyncCommand("Wait,1");

        EventManager.AddSyncCommand("Message,2,3");
        EventManager.AddSyncCommand("Wait,1");

        EventManager.AddSyncCommand("FadeOut");
        EventManager.AddSyncCommand("Image,Prologue3");
        EventManager.AddSyncCommand("Wait,1");
        EventManager.AddSyncCommand("FadeIn");
        EventManager.AddSyncCommand("Wait,1");

        EventManager.AddSyncCommand("Message,4,5");
        EventManager.AddSyncCommand("Wait,1");
        EventManager.Method(Prologue2);
    }

    public void Prologue2()
    {
        GameManager.Camera.SetCamera();
        EventManager.AddSyncCommand("FadeOut");
        EventManager.AddSyncCommand("HideImage");
        EventManager.AddSyncCommand("Wait,2");
        EventManager.AddSyncCommand("FadeIn");
        EventManager.AddSyncCommand("Wait,1");
        EventManager.Method(CameraWalk);

        EventManager.AddSyncCommand("FadeOut");
        EventManager.AddSyncCommand("Wait,1");
        EventManager.AddSyncCommand("SetCamera,Player");
        EventManager.AddSyncCommand("Message,6,7");
        EventManager.AddSyncCommand("FadeIn");

        EventManager.AddSyncCommand("TextColor,Black");
        EventManager.AddSyncCommand("DialogImageOn");
        EventManager.AddSyncCommand("DialogPosition,Down");
        EventManager.AddSyncCommand("TextAlign,Left");

        EventManager.AddSyncCommand("Message,8,9,10,11,12,13");
        EventManager.AddSyncCommand("Wait,1");

        EventManager.AddSyncCommand("Message,14,15");
        EventManager.AddSyncCommand("Message,16");
        EventManager.AddSyncCommand("Wait,1");
        EventManager.AddSyncCommand("ShakeScreen,5,5");
        EventManager.AddSyncCommand("Message,31");
        EventManager.AddSyncCommand("Message,17");

        EventManager.AddSyncCommand("TextAlign,Center");
        EventManager.AddSyncCommand("DialogImageOff");
        EventManager.AddSyncCommand("Message,18");
        EventManager.AddSyncCommand("Message,20");
        EventManager.AddSyncCommand("TextAlign,Left");
        EventManager.AddSyncCommand("DialogImageOn");
        //EventManager.AddAsyncCommand("SetPlayer,25.5,-10.5");
    }

    public void CameraWalk()
    {
        EventManager.AddAsyncCommand("Set,25.5,-10.5");
        for (var i = 0; i <= 10; i++)
        {
            EventManager.AddSyncCommand("MoveDown");
        }
        for (var i = 0; i <= 14; i++)
        {
            EventManager.AddSyncCommand("MoveRight");
        }
        for (var i = 0; i <= 3; i++)
        {
            EventManager.AddSyncCommand("MoveUp");
        }
        EventManager.AddSyncCommand("Wait,1");
    }

    public void Title()
    {
        TitlePannel.SetActive(true);
    }

    //Main Menu
    public void MoveToNextButton()
    {
        GameObject selected = buttonList[selectedButton].image.transform.Find("select").gameObject;
        selected.SetActive(false);
        selectedButton++;
        if (selectedButton >= buttonList.Length)
        {
            selectedButton = 0;
        }
        GameObject selected2 = buttonList[selectedButton].image.transform.Find("select").gameObject;
        selected2.SetActive(true);
    }

    public void MoveToPreviousButton()
    {
        GameObject selected = buttonList[selectedButton].image.transform.Find("select").gameObject;
        selected.SetActive(false);
        selectedButton--;
        if (selectedButton < 0)
        {
            selectedButton = (buttonList.Length - 1);
        }
        GameObject selected2 = buttonList[selectedButton].image.transform.Find("select").gameObject;
        selected2.SetActive(true);
    }

    public void NewGameBtnAction()
    {
        FadeIn();
        GameManager.isMenu = false;
        Debug.Log("Clicked new game");
        GameObject menuObject = GameObject.Find("Menu");
        menuObject.SetActive(false);
    }

    public void OptionBtnAction()
    {
        Debug.Log("Options");
    }

    public void GetSavedBtnAction()
    {
        Debug.Log("GetSaved");
    }
}
