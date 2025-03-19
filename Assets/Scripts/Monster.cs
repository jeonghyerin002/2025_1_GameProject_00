using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int Health = 100;                     // ü���� �����Ѵ�. (����)
    public float Timer = 1.0f;                   // Ÿ�̸Ӹ� �����Ѵ�.
    public int AttackPoint = 10;                 //���ݷ��� �����Ѵ�.

    // ù ������ ������ �ѹ� ����ȴ�.
    void Start()
    {
        Health += 100;                        // ù ������ ������ ���� �� �� 100ü���� �߰� �����ش�.
    }


    // �Ź� ������ �� ȣ��ȴ�.
    void Update()
    {
        CharacteeHealthUp();
        CheckDeath();                           // �Լ� ȣ��
        Timer -= Time.deltaTime;         // �ð��� �� �����Ӹ��� ���� ��Ų��. (deltaTime �����Ӱ��� �ð� ������ �ǹ��Ѵ�.)

        if(Timer <=0)
        {
            Timer = 1.0f;               // �ٽ� 1�ʷ� ���� �����ش�.
            Health += 20;               // 1�ʸ��� ü�� 20�� �÷��ش�.          (Health - Health + 20)
        }


        if (Input.GetKeyDown(KeyCode.Space))        // �����̽� Ű�� ������ ��
        {
            Health -= AttackPoint;                  // üũ ����Ʈ�� ���� ����Ʈ ��ŭ ���� �����ش�. (Health = Health - AttackPoint)
        }

        CheckDeath();                              // �Լ� ȣ��
    }
    void CharacteeHealthUp()
    {
        Timer -= Time.deltaTime;

        if (Timer <= 0)
        {
            Timer = 1.0f;
        }
    }
    public void CharacterHit(int Demage)                 // Ŀ���� �������� �޴� �Լ��� ����Ѵ�.
    {
        Health -= Demage;                         // ���� ���ݷ¿� ���� ü���� ���ҽ�Ų��.
    }
    void CheckDeath()                            // �Լ� ����
    {
        if (Health <= 0)                           // ü���� 0���Ϸ� �������� ���� ��Ų��.
            Destroy(gameObject);                   // �� ������Ʈ�� �ı��Ѵ�.

    }
}
