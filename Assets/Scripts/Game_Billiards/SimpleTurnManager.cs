using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTurnManager : MonoBehaviour
{
    //�������� (��� ���� �����ؼ� ��� �� �� ����)
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
            Debug.Log("�� ����! �ٽ� ĥ �� �ֽ��ϴ�.");
        }
    }

    void CheckAllBalls()       //��� ���� ������� Ȯ��
    {
        SimpleBallController[] allBalls = FindObjectsOfType<SimpleBallController>();  //���� �ִ� ���ú���Ʈ�ѷ��� ����ϴ� ��� ������Ʈ�� �迭�� �ִ´�.
        anyBallMoving = false;               //�ʱ�ȭ �����ش�
              
        foreach(SimpleBallController ball in allBalls)     //�迭 ��ü Ŭ������ ��ȯ�ϸ鼭
        {
            if(ball.IsMoving())           //���� �����̰� �ִ��� Ȯ���ϴ� �Լ��� ȣ��
            {
                anyBallMoving = true;
                break;
            }
        }

    }

    public static void OnBallHit()            //���� �÷��� ���� �� ȣ��
    {
        canPlay = false;             //�ٸ� ������ �� �����̰�
        anyBallMoving = true;
        Debug.Log("�� ����! ���� ���� ������ ��ٸ�����.");

    }
}
