using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTurnManager : MonoBehaviour
{
    //전역변수 (모든 공이 공유해서 사용 할 수 있음)
    public static bool canPlay = true;
    public static bool anyBallMoving = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckAllBalls();

        if(!anyBallMoving && !canPlay)
        {
            canPlay = true;
            Debug.Log("턴 종료! 다시 칠 수 있습니다.");
        }
    }

    void CheckAllBalls()       //모든 공이 멈췄는지 확인
    {
        SimpleBallController[] allBalls = FindObjectsOfType<SimpleBallController>();  //씬에 있는 심플볼컨트롤러를 사용하는 모든 오브젝트를 배열에 넣는다.
        anyBallMoving = false;               //초기화 시켜준다
              
        foreach(SimpleBallController ball in allBalls)     //배열 전체 클래스를 순환하면서
        {
            if(ball.IsMoving())           //공이 움직이고 있는지 확인하는 함수를 호출
            {
                anyBallMoving = true;
                break;
            }
        }

    }

    public static void OnBallHit()            //공을 플레이 했을 때 호출
    {
        canPlay = false;             //다른 공들이 못 움직이게
        anyBallMoving = true;
        Debug.Log("턴 시작! 공이 멈출 때까지 기다리세요.");

    }
}
