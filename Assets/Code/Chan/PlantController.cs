using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantController : MonoBehaviour
{
    GameObject Player;
    Animator _animatorPlant;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");

        _animatorPlant = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        //충돌 판정

        Vector2 p = transform.position; //식물 중심 좌표
        Vector2 p1 = Player.transform.position;   //플레이어 중심 좌표
        Vector2 dir = p - p1; //플레이어&식물

        float d = dir.magnitude;

        float r = 0.5f;  //식물 반경
        float r1 = 0.5f; //플레이어 반경


        if (d < r + r1) //플레이어 & plant1 충돌되었을 때
        {
            /*
            if (Player.GetComponent<GamePlayer>().PowerType == 2)
            {
                if (Input.GetKeyDown(KeyCode.G))
                {
                    if (_animatorPlant.GetBool("plant_change"))
                    {
                        _animatorPlant.SetBool("plant_change", false);
                    }
                    else
                        _animatorPlant.SetBool("plant_change", true);
                }
            }
            */
        }
    }
}