using UnityEngine;
using static UIManager;

public class GameMenu
{
    public static MenuButton[] buttonList;
    public static int selectedButton = 0;
    private static GameObject MenuPannel;

    public static void InputHandler(Action action)
    {
        if (action == Action.DOWN)
        {
            MoveToNextDownButton();
        }
        if (action == Action.UP)
        {
            MoveToPreviousUpButton();
        }
        if (action == Action.RIGHT)
        {
            MoveToNextRightButton();
        }
        if (action == Action.LEFT)
        {
            MoveToNextLeftButton();
        }
        if (action == Action.A)
        {
            buttonList[selectedButton].action();
        }
        if (action == Action.B)
        {
            setItemMenuFalse();
        }
    }

    public GameMenu()
    {
        MenuPannel = GameObject.Find("ItemMenu");

        buttonList = new MenuButton[13];

        buttonList[0].image = GameObject.Find("Item0Btn").transform.Find("line").gameObject;
        buttonList[1].image = GameObject.Find("Item1Btn").transform.Find("line").gameObject;
        buttonList[2].image = GameObject.Find("Item2Btn").transform.Find("line").gameObject;
        buttonList[3].image = GameObject.Find("Item3Btn").transform.Find("line").gameObject;
        buttonList[4].image = GameObject.Find("Item4Btn").transform.Find("line").gameObject;
        buttonList[5].image = GameObject.Find("Item5Btn").transform.Find("line").gameObject;
        buttonList[6].image = GameObject.Find("Item6Btn").transform.Find("line").gameObject;
        buttonList[7].image = GameObject.Find("Item7Btn").transform.Find("line").gameObject;
        buttonList[8].image = GameObject.Find("Item8Btn").transform.Find("line").gameObject;
        buttonList[9].image = GameObject.Find("Item9Btn").transform.Find("line").gameObject;
        buttonList[10].image = GameObject.Find("Item10Btn").transform.Find("line").gameObject;
        buttonList[11].image = GameObject.Find("Item11Btn").transform.Find("line").gameObject;
        buttonList[12].image = GameObject.Find("Item12Btn").transform.Find("line").gameObject;

        /*buttonList[1].image = GameObject.Find("Item1Btn").GetComponent<Image>();*/

        for (int i = 1; i < 13; i++)
        {
            GameObject line = GameObject.Find("Item" + i + "Btn").transform.Find("line").gameObject;
            line.SetActive(false);
            GameObject selected0 = GameObject.Find("Item0Btn").transform.Find("selected").gameObject;
            selected0.SetActive(false); //line은 첫번째버튼은 끄면안돼서 첫번째버튼 selected를 따로 꺼줌
            GameObject selected = GameObject.Find("Item" + i + "Btn").transform.Find("selected").gameObject;
            selected.SetActive(false);
            /*buttonList[i].image.color = Color.white;*/
        }

        buttonList[0].action = Btn0Action;
        buttonList[1].action = Btn1Action;
        buttonList[2].action = Btn2Action;
        buttonList[3].action = Btn3Action;
        buttonList[4].action = Btn4Action;
        buttonList[5].action = Btn5Action;
        buttonList[6].action = Btn6Action;
        buttonList[7].action = Btn7Action;
        buttonList[8].action = Btn8Action;
        buttonList[9].action = Btn9Action;
        buttonList[10].action = Btn10Action;
        buttonList[11].action = Btn11Action;
        buttonList[12].action = Btn12Action;

        MenuPannel.SetActive(false);
    }

    public static void Menu()
    {
        //GameManager.isItemMenu = !GameManager.isItemMenu;
        GameManager.isItemMenu = true;
        MenuPannel.SetActive(GameManager.isItemMenu);
    }

    //Item Menu
    public static void MoveToNextDownButton()
    {
        GameObject selected = GameObject.Find("Item" + selectedButton + "Btn").transform.Find("line").gameObject;
        selected.SetActive(false);

        /* buttonList[selectedButton].image.color = Color.white;*/
        if (selectedButton == 0 || selectedButton == 10)
        {
            selectedButton = selectedButton + 2;
        }
        else if (0 < selectedButton && selectedButton < 4)
        {
            selectedButton = selectedButton + 4;
        }
        else if (4 < selectedButton && selectedButton < 8)
        {
            selectedButton = selectedButton + 4;
        }
        else if (selectedButton == 12)
        {
            selectedButton = 0;
        }

        GameObject selected2 = GameObject.Find("Item" + selectedButton + "Btn").transform.Find("line").gameObject;
        selected2.SetActive(true);
        /*buttonList[selectedButton].image.color = Color.yellow;*/
    }

    public static void MoveToPreviousUpButton()
    {
        GameObject selected = GameObject.Find("Item" + selectedButton + "Btn").transform.Find("line").gameObject;
        selected.SetActive(false);
        /*buttonList[selectedButton].image.color = Color.white;*/
        if (selectedButton == 2 || selectedButton == 12) selectedButton = selectedButton - 2;
        else if (4 < selectedButton && selectedButton < 8)
        {
            selectedButton = selectedButton - 4;
        }
        else if (8 < selectedButton && selectedButton < 12)
        {
            selectedButton = selectedButton - 4;
        }
        else if (selectedButton == 0)
        {
            selectedButton = 12;
        }
        GameObject selected2 = GameObject.Find("Item" + selectedButton + "Btn").transform.Find("line").gameObject;
        selected2.SetActive(true);
        /*buttonList[selectedButton].image.color = Color.yellow;*/
    }

    public static void MoveToNextRightButton()
    {
        GameObject selected = GameObject.Find("Item" + selectedButton + "Btn").transform.Find("line").gameObject;
        selected.SetActive(false);
        /*buttonList[selectedButton].image.color = Color.white;*/
        selectedButton++;
        if (selectedButton > 12)
        {
            selectedButton = 0;
        }
        GameObject selected2 = GameObject.Find("Item" + selectedButton + "Btn").transform.Find("line").gameObject;
        selected2.SetActive(true);
        /*buttonList[selectedButton].image.color = Color.yellow;*/
    }

    public static void MoveToNextLeftButton()
    {
        GameObject selected = GameObject.Find("Item" + selectedButton + "Btn").transform.Find("line").gameObject;
        selected.SetActive(false);
        /*buttonList[selectedButton].image.color = Color.white;*/
        selectedButton--;
        if (selectedButton < 0)
        {
            selectedButton = (buttonList.Length - 1);
        }
        GameObject selected2 = GameObject.Find("Item" + selectedButton + "Btn").transform.Find("line").gameObject;
        selected2.SetActive(true);
        /*buttonList[selectedButton].image.color = Color.yellow;*/
    }

    public static void setItemMenuFalse()
    {
        GameManager.isItemMenu = false;
        Debug.Log("isItemMenu is false");
        GameObject menuObject = GameObject.Find("ItemMenu");
        menuObject.SetActive(false);
    }

    public static void Btn0Action() { Debug.Log("Item 0"); GameManager.PlayerCode.SetPower(Power.Default); }
    public static void Btn1Action() { Debug.Log("Item 1"); GameManager.PlayerCode.SetPower(Power.Capricorn); }
    public static void Btn2Action() { Debug.Log("Item 2"); }
    public static void Btn3Action() { Debug.Log("Item 3"); }
    public static void Btn4Action() { Debug.Log("Item 4"); GameManager.PlayerCode.SetPower(Power.Pisces); }
    public static void Btn5Action() { Debug.Log("Item 5"); GameManager.PlayerCode.SetPower(Power.Capricorn); }
    public static void Btn6Action() { Debug.Log("Item 6"); }
    public static void Btn7Action() { Debug.Log("Item 7"); }
    public static void Btn8Action() { Debug.Log("Item 8"); }
    public static void Btn9Action() { Debug.Log("Item 9"); }
    public static void Btn10Action() { Debug.Log("Item 10"); GameManager.PlayerCode.SetPower(Power.Aquarius); }
    public static void Btn11Action() { Debug.Log("Item 11"); }
    public static void Btn12Action() { Debug.Log("Item 12"); }
}
