using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public int fruitType;

    public bool hasMerged = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasMerged)
            return;

        Fruit otherFruit = collision.gameObject.GetComponent<Fruit>();

        if(otherFruit != null && !otherFruit.hasMerged && otherFruit.fruitType == fruitType)
        {
            hasMerged = true;
            otherFruit.hasMerged = true;

            Vector3 mergePosition = (transform.position + otherFruit.transform.position) / 2f;

            //게임 매니저에서 Merge 구현된 것을 호출
            FruitGame gameManager = FindObjectOfType<FruitGame>();
            if(gameManager != null)
            {
                gameManager.MergeFruits(fruitType, mergePosition);
            }


            //과일들 제거
            Destroy(otherFruit.gameObject);
            Destroy(gameObject);
        }
    }

}
