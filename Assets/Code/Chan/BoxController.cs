using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    GameObject Player;
    Animator _animatorBox;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");

        _animatorBox = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        //충돌 판정

        Vector2 p = transform.position; //박스 중심 좌표
        Vector2 p1 = Player.transform.position;   //플레이어 중심 좌표
        Vector2 dir = p - p1; //플레이어&박스

        float d = dir.magnitude;

        float r = 0.5f;  //박스 반경
        float r1 = 0.5f; //플레이어 반경

        /*
        if (d < r + r1) //플레이어 & 박스 충돌되었을 때
        {
            if (Player.GetComponent<GamePlayer>().PowerType == 3)
            {
                if (Input.GetKeyDown(KeyCode.G))
                {
                    if (_animatorBox.GetBool("box_destroy"))
                    {
                        _animatorBox.SetBool("box_destroy", false);
                    }
                    else
                        _animatorBox.SetBool("box_destroy", true);
                }
            }
        }
        */
    }
}
