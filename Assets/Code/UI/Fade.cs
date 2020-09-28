using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fade
{
    private static Image _blackImage; // 페이드 인 아웃 표시용 이미지

    public Fade()
    {
        // 페이드 인 아웃 표시용 이미지 초기화
        _blackImage = GameObject.Find("Canvas").transform.Find("Black").GetComponent<Image>();
        _blackImage.gameObject.SetActive(false);
    }

    public void SetBlack()
    {
        Color alpha = _blackImage.color;
        alpha.a = 1f;
        _blackImage.color = alpha;
        _blackImage.gameObject.SetActive(true);
    }

    public IEnumerator FaderIn()
    {
        GameManager.isAction = true;
        var time = 0f;
        Color alpha = _blackImage.color;
        while (alpha.a > 0)
        {
            time += Time.deltaTime / 1f;
            alpha.a = Mathf.Lerp(1, 0, time);
            _blackImage.color = alpha;
            yield return null;
        }
        _blackImage.gameObject.SetActive(false);
        GameManager.isAction = false;
    }

    public IEnumerator FaderOut()
    {
        GameManager.isAction = true;
        var time = 0f;
        _blackImage.gameObject.SetActive(true);
        Color alpha = _blackImage.color;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / 1f;
            alpha.a = Mathf.Lerp(0, 1, time);
            _blackImage.color = alpha;
            yield return null;
        }
        GameManager.isAction = false;
    }
}
