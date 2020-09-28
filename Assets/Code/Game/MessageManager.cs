using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageManager
{ 
    public static bool isPrintMessage;

    //private Dictionary<int, string[]> _messages = new Dictionary<int, string[]>();
    private Dictionary<int, object> _messages = new Dictionary<int, object>();
    private List<Dictionary<string, object>> _textData;
    private Queue<int> _textList = new Queue<int>();

    private GameObject _dialog; // 전체 다이얼로그 객체
    private Image _messageBoxImage; // 메세지 박스 이미지
    private Image _actorBoxImage; // 액터 박스 이미지
    private Text _dialogMessage; // 메세지 텍스트
    private Text _dialogActor; // 액터 텍스트
    private string _currentActor; // 현재 액터
    private string _currentMessage; // 현재 메세지

    public enum DialogPosition { Up = 0, Middle, Down, VeryDown };

    public MessageManager()
    {
        _dialog = GameObject.Find("Dialog");
        _messageBoxImage = GameObject.Find("Dialog").transform.Find("MessageBox").GetComponent<Image>();
        _actorBoxImage = GameObject.Find("Dialog").transform.Find("ActorBox").GetComponent<Image>();
        _dialogMessage = _dialog.transform.Find("Message").GetComponent<Text>();
        _dialogActor = _dialog.transform.Find("Actor").GetComponent<Text>();
        _dialog.SetActive(false);
        //_messageBoxImage.gameObject.SetActive(false);
        //_actorBoxImage.gameObject.SetActive(false);
        isPrintMessage = false;

        _dialogMessage.text = string.Empty;
        _dialogActor.text = string.Empty;
        GenerateData();
    }

    // 대화창의 위치를 변경하는 메소드
    public void SetDialogPosition(DialogPosition messagePosition)
    {
        if (messagePosition == DialogPosition.Up)
        {
            _dialog.transform.localPosition = new Vector3(0, 48);
        }
        else if (messagePosition == DialogPosition.Middle)
        {
            _dialog.transform.localPosition = new Vector3(0, 0);
        }
        else if (messagePosition == DialogPosition.Down)
        {
            _dialog.transform.localPosition = new Vector3(0, -48);
        }
        else if (messagePosition == DialogPosition.VeryDown)
        {
            _dialog.transform.localPosition = new Vector3(0, -72);
        }
    }

    public void SetTextAlign(TextAnchor align)
    {
        if (align == TextAnchor.UpperLeft)
        {
            _dialogMessage.alignment = TextAnchor.UpperLeft;
        }
        else if (align == TextAnchor.UpperCenter)
        {
            _dialogMessage.alignment = TextAnchor.UpperCenter;
        }
        else if (align == TextAnchor.UpperRight)
        {
            _dialogMessage.alignment = TextAnchor.UpperRight;
        }
        else if (align == TextAnchor.MiddleCenter)
        {
            _dialogMessage.alignment = TextAnchor.MiddleCenter;
        }
    }

    public void SetTextColor(string color)
    {
        if (color.CompareTo("Black") == 0)
        {
            _dialogMessage.color = Color.black;
        }
        else if(color.CompareTo("White") == 0)
        {
            _dialogMessage.color = Color.white;
        }
    }

    public void SetDialogImage(bool value)
    {
        _messageBoxImage.gameObject.SetActive(value);
        _messageBoxImage.gameObject.SetActive(value);
    }

    // GetMessage 메소드와 비슷한 기능
    //public string[] GetMessage(int index) // key(index)값을 받으면 해당 line 반환
    public string GetMessage(int index) // key(index)값을 받으면 해당 line 반환
    {
        if (_messages.ContainsKey(index))
        {
            //return _messages[index];
        }
        return null;
        //return string.Empty;
    }

    // 텍스트 파일을 불러오는 메소드
    private void GenerateData()
    {
        _textData = CSVReader.Read("Korean");
        for (var i = 0; i < _textData.Count; i++)
        {
            int index = (int)_textData[i]["index"];
            string actor = (string)_textData[i]["actor"];
            string msg = (string)_textData[i]["message"];
            string[] message = {actor, msg};
            _messages.Add(index, message);
        }
    }

    public void Message(Queue<int> texts)
    {
        GameManager.isAction = true;
        GameManager.isTalking = true;
        _textList = texts;
        _dialog.SetActive(true);
        Next();
    }

    public void Skip()
    {
        EventManager.Instance.StopShowTextCoroutine();
        _dialogMessage.text = _currentMessage;
        isPrintMessage = false;
    }

    public void Next()
    {
        if (_textList.Count > 0)
        {
            var texts = (string[])_messages[_textList.Dequeue()];
            _currentActor = texts[0];
            _currentMessage = texts[1];
            //_currentMessage = _messages[_textList.Dequeue()][1];
            //_currentText = _messages[_textList.Dequeue()];
            EventManager.Instance.StartShowTextCoroutine(_currentMessage);
        }
        else
        {
            _dialog.SetActive(false);
            GameManager.isTalking = false;
            GameManager.isAction = false;
        }
    }

    public IEnumerator ShowText(string text)
    {
        WaitForSeconds wait = new WaitForSeconds(.1f);
        isPrintMessage = true;
        _dialogMessage.text = "";
        if (_currentActor.CompareTo(string.Empty) == 0)
        {
            _actorBoxImage.gameObject.SetActive(false);
            _dialogActor.text = string.Empty;
        }
        else
        {
            _actorBoxImage.gameObject.SetActive(_messageBoxImage.enabled);
            _dialogActor.text = _currentActor;
        }

        for (var i = 0; i <= text.Length; i++)
        {
            _dialogMessage.text = text.Substring(0, i);
            yield return wait;
        }
        //_dialogText.text = "음... <color=cyan>나는 피카츄다!</color>";
        isPrintMessage = false;
    }
}