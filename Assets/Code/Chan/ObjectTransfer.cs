using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTransfer : MonoBehaviour
{
    GameObject player;
  //  GameObject boxCheck;
    public bool b_transferCheck=false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
      //  boxCheck = GameObject.Find("boxCheck");
    }

    // Update is called once per frame
    void Update()
    {
        //충돌 판정
        Vector2 p1 = transform.position;               //박스 중심 좌표
        Vector2 p2 = player.transform.position;   //플레이어 중심 좌표
        Vector2 dir = p1 - p2;
      //  Vector3 b1 = this.transform.position;
        float d = dir.magnitude;
        float r1 = 0.5f;                 //박스반경
        float r2 = 0.5f;                //플레이어 반경
      //  RaycastHit2D[] hit = Physics2D.LinecastAll(b1 * 5, b1 * 5);

     //   for (int i = 0; i < hit.GetLength(0); i++)
      //  {
        //    RaycastHit2D select = hit[i];
         //   if (select.collider.gameObject.layer != 9)
        //    {
        
       // if(b_transferCheck == true)
      //  {
            if (d < r1 + r2) //충돌되었을 때
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow) && Input.GetKey(KeyCode.S))
                {
                    transform.Translate(-1, 0, 0); //오브젝트 왼쪽으로 1칸 이동
                    player.transform.Translate(-1, 0, 0); //플레이어 왼쪽으로 1칸 이동
                }

                if (Input.GetKeyDown(KeyCode.RightArrow) && Input.GetKey(KeyCode.S))
                {
                    transform.Translate(1, 0, 0); //오브젝트 오른쪽으로 1칸 이동
                    player.transform.Translate(1, 0, 0); //플레이어 오른쪽으로 1칸 이동
                }

                if (Input.GetKeyDown(KeyCode.UpArrow) && Input.GetKey(KeyCode.S))
                {
                    transform.Translate(0, 1, 0); //오브젝트 오른쪽으로 1칸 이동
                    player.transform.Translate(0, 1, 0); //플레이어 오른쪽으로 1칸 이동
                }

                if (Input.GetKeyDown(KeyCode.DownArrow) && Input.GetKey(KeyCode.S))
                {
                    transform.Translate(0, -1, 0); //오브젝트 오른쪽으로 1칸 이동
                    player.transform.Translate(0, -1, 0); //플레이어 오른쪽으로 1칸 이동
                }
            }
      //  }
        /*
        if (d < r1 + r2) //충돌되었을 때
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) && Input.GetKey(KeyCode.S))
            {
                boxCheck.transform.Translate(-1, 0, 0); //박스체크 왼쪽으로 1칸 이동
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) && Input.GetKey(KeyCode.S))
            {
                boxCheck.transform.Translate(1, 0, 0); //박스체크 오른쪽으로 1칸 이동
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) && Input.GetKey(KeyCode.S))
            {
                boxCheck.transform.Translate(0, 1, 0); //박스체크 오른쪽으로 1칸 이동
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) && Input.GetKey(KeyCode.S))
            {
                boxCheck.transform.Translate(0, -1, 0); //박스체크 오른쪽으로 1칸 이동
            }
        }*/

        /*
                if (d < r1 + r2) //충돌되었을 때
                {
                    if (Input.GetKeyDown(KeyCode.LeftArrow) && Input.GetKey(KeyCode.S))
                    {
                        transform.Translate(-1, 0, 0); //오브젝트 왼쪽으로 1칸 이동
                        player.transform.Translate(-1, 0, 0); //플레이어 왼쪽으로 1칸 이동
                    }

                    if (Input.GetKeyDown(KeyCode.RightArrow) && Input.GetKey(KeyCode.S))
                    {
                        transform.Translate(1, 0, 0); //오브젝트 오른쪽으로 1칸 이동
                        player.transform.Translate(1, 0, 0); //플레이어 오른쪽으로 1칸 이동
                    }

                    if (Input.GetKeyDown(KeyCode.UpArrow) && Input.GetKey(KeyCode.S))
                    {
                        transform.Translate(0, 1, 0); //오브젝트 오른쪽으로 1칸 이동
                        player.transform.Translate(0, 1, 0); //플레이어 오른쪽으로 1칸 이동
                    }

                    if (Input.GetKeyDown(KeyCode.DownArrow) && Input.GetKey(KeyCode.S))
                    {
                        transform.Translate(0, -1, 0); //오브젝트 오른쪽으로 1칸 이동
                        player.transform.Translate(0, -1, 0); //플레이어 오른쪽으로 1칸 이동
                    }
                }
                */
        //     }

        //   else
        //       Debug.Log("이동불가");
        //     }
    }

    public bool transferCheck(bool check)
    {
        b_transferCheck = check;
        return b_transferCheck;
    }
}
