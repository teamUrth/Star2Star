using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageManager
{
    public List<Image> Images = new List<Image>();
    public static bool isViewImage = false;

    //private static GameObject _imagePannel; // 이미지 표시용 패널
    private static Image _imagePannel;
    private Queue<Image> _queueImages = new Queue<Image>();

    public ImageManager()
    {
        // 이미지 표시용 패널 초기화
        //_imagePannel = GameObject.Find("ImagePannel");
        //_imagePannel.SetActive(false);
        _imagePannel = GameObject.Find("ImagePannel").GetComponent<Image>();
        _imagePannel.sprite = null;
        _imagePannel.gameObject.SetActive(false);
    }

    public void SetImage(Sprite image)
    {
        _imagePannel.sprite = image;
        _imagePannel.gameObject.SetActive(true);
    }

    public void HideImage()
    {
        _imagePannel.gameObject.SetActive(false);
    }

    public void InputHandler()
    {
        if (_queueImages.Count > 0)
        {
            
        }
    }
}
