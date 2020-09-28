using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenChecker : MonoBehaviour {
    public float x, y;
	// Use this for initialization
	void Start () {
		
	}
	
    // Update is called once per frame
    void Update()
    {
        Vector3 view = Camera.main.WorldToScreenPoint(this.transform.position);//월드 좌표를 스크린 좌표로 변형한다.
        //if (view.y < -50)
        {
            this.x = view.x;
            this.y = view.y;
            //Debug.Log(this.name + ": " + view.x + ", " + view.y);    //스크린 좌표가 -50 이하일시 삭제  
        }
    }
}