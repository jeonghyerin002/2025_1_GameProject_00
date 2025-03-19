using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float GameTimer = 3.0f;                               // ���� Ÿ�̸Ӹ� �����Ѵ�.
    public GameObject MonsterGo;                                 // ���� ���� ������Ʈ�� �����Ѵ�.
   

    // Update is called once per frame
    void Update()
    {
        GameTimer -= Time.deltaTime;                   // �ð��� �� �����Ӹ��� ��� ��Ų�� (deltaTime �����Ӱ��� �ð� ������ �ǹ��Ѵ�.)

        if (GameTimer  <= 0)                             // ���� timer�� ��ġ�� 0���Ϸ� ������ ���
        {
            GameTimer = 3.0f;                            //�ٽ� 3�ʷ� ���� �����ش�.

            GameObject Temp = Instantiate(MonsterGo);
            Temp.transform.position = new Vector3(Random.Range(-10, 10), Random.Range(-4, 4), 0.0f);
            // X-10 ~ 10, Y-4 ~ 4 �� �������� �������� ��ġ ��Ų��.
        }
        if (Input.GetMouseButtonDown(0))                   // ���콺 ��ư�� ������
        {
            RaycastHit hit;                                 // Ray ����
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);        // ī�޶󿡼� ���̸� ���� �����Ѵ�.
            // 3D ���ӿ��� ������Ʈ�� ���� �� �� ����Ѵ� (ȭ�鿡 ���̴� ���׸� �����ϱ� ���ؼ� ���)

            if (Physics.Raycast(ray, out hit))                                 // Hit�� ������Ʈ�� �����Ѵ�.
            {
                if (hit.collider != null)                                      // Hit�� ������Ʈ�� �ִ� ���
                {
                    hit.collider.GetComponent<Monster>().CharacterHit(50);                             // �α׷� �����ش�
                }
            }
        }

    }
}
