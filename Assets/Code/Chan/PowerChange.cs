using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PowerChange : MonoBehaviour
{
    //   int type = 0; //0 = 사람 1 = 물고기 2 = 물병자리 3 = 염소
    //  GameObject Player;
    

    // Start is called before the first frame update
    void Start()
    {
        //    Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //테스트용
        //R키 눌렀을 때 물고기라고 판정
        if (Input.GetKeyDown(KeyCode.R))
        {
            powerFish();
        }
        //T누르면 기본상태로 변경
        if (Input.GetKeyDown(KeyCode.T))
        {
            this.GetComponent<GamePlayer>().PowerType = 0;
        }

        //D눌렀을때 물병자리
        if (Input.GetKeyDown(KeyCode.D))
        {
            powerAquarius();
        }

        //F눌렀을때 염소자리
        if (Input.GetKeyDown(KeyCode.F))
        {
            powerCap();
        }

    }

    void powerFish()
    {
        //물고기 애니메이션 변경
        /*
        //물 콜라이더 isTrigger로 변경
        GameObject waterTile;
        waterTile = GameObject.Find("waterTile");
        BoxCollider2D boxcollider = waterTile.AddComponent<BoxCollider2D>();
        boxcollider.isTrigger = true;
        */
        //this.GetComponent<GamePlayer>().PowerType = 1;

    }

    //물병자리
    void powerAquarius()
    {
        //this.GetComponent<GamePlayer>().PowerType = 2;
    }

    //염소자리
    void powerCap()
    {
        //this.GetComponent<GamePlayer>().PowerType = 3;
    }
}
