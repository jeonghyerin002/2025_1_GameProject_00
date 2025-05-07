using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    //������ ���ҽ�
    public GameObject cardPrefab;                   //ī�� ������
    public Sprite[] cardImages;                     //ī�� �̹��� �迭
    //���� Transfrom
    public Transform deckArea;                      //�� ����
    public Transform handArea;                      //���� ����
    //UI ���
    public Button drawButton;                       //��ο� ��ư
    public TextMeshProUGUI deckCountText;           //���� �� ī�� �� ǥ�� �ؽ�Ʈ 
    //���� ��
    public float cardSpacing = 2.0f;                //ī�� ����
    public int maxHandSize = 6;                     //�ִ� ���� ũ�� 

    //�迭 ����
    public GameObject[] deckCards;                  //�� ī�� �迭
    public int deckCount;                           //���� ���� �ִ� ī�� ��

    public GameObject[] handCards;                  //���� �迭
    public int handCount;                           //���� ���п� �ִ� ī�� ��

    //�̸� ���ǵ� �� ī�� ��� (���ڸ�)
    public int[] prefedinedDeck = new int[]
    {
        1,1,1,1,1,1,1,1,                //1�� 8��
        2,2,2,2,2,2,                    //2�� 6��
        3,3,3,3,                        //3�� 4��
        4,4                             //4�� 2��
    };

    public Transform mergeArea;
    public Button mergeButton;
    public int maxMergeSize = 3;

    public GameObject[] mergeCards;
    public int mergeCount;

    // Start is called before the first frame update
    void Start()
    {
        //�迭 �ʱ�ȭ
        deckCards = new GameObject[prefedinedDeck.Length];
        handCards = new GameObject[maxHandSize];
        mergeCards = new GameObject[maxMergeSize];

        //�� �ʱ�ȭ �� ����
        InitializeDeck();
        ShuffleDeck();

        if (drawButton != null)              //��ư ������ üũ 
        {
            drawButton.onClick.AddListener(OnDrawButtonClicked); //���� ��� ��ư�� ������ OnDrawButtonClicked �Լ� ���� 
        }

        if (mergeButton != null)            
        {
            mergeButton.onClick.AddListener(OnMergeButtonClicked); 
            mergeButton.interactable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    //�� ����
    void ShuffleDeck()          //Fisher-Yates ���� �˰��� 
    {
        for (int i = 0; i < deckCount - 1; i++)
        {
            int j = Random.Range(i, deckCount);
            //�迭 �� ī�� ��ȯ
            GameObject temp = deckCards[i];
            deckCards[i] = deckCards[j];
            deckCards[j] = temp;
        }
    }

    //�� �ʱ�ȭ - ������ ī�� ����
    void InitializeDeck()
    {
        deckCount = prefedinedDeck.Length;

        for (int i = 0; i < prefedinedDeck.Length; i++)
        {
            int value = prefedinedDeck[i];                  //ī�� �� �������� 
            //�̹��� �ε��� ���(���� ���� �ٸ� �̹��� ���)
            int imageIndex = value - 1;                     //���� 1���� �����ϹǷ� �ε����� 0����
            if (imageIndex >= cardImages.Length || imageIndex < 0)
            {
                imageIndex = 0;                 //�̹����� �����ϰų� �ε����� �߸��� ��� ù ��° �̹��� ���                      
            }
            //ī�� ������Ʈ ���� (�� ��ġ)
            GameObject newCardObj = Instantiate(cardPrefab, deckArea.position, Quaternion.identity);
            newCardObj.transform.SetParent(deckArea);
            newCardObj.SetActive(false);                //ó������ ��Ȱ��ȭ 
            //ī�� ������Ʈ �ʱ�ȭ
            Card cardComp = newCardObj.GetComponent<Card>();
            if (cardComp != null)
            {
                cardComp.InitCard(value, cardImages[imageIndex]);
            }
            deckCards[i] = newCardObj;              //�迭�� ���� 
        }
    }

    //���� ���� �Լ�
    public void ArrangeHand()
    {
        if (handCount == 0)                 
            return;

        float startX = -(handCount - 1) * cardSpacing / 2;         

        for (int i = 0; i < handCount; i++)
        {
            if (handCards[i] != null)
            {
                Vector3 newPos = handArea.position + new Vector3(startX + i * cardSpacing, 0, 0);
                handCards[i].transform.position = newPos;
            }

        }
    }

    public void ArrangeMerge()
    {
        if (mergeCount == 0)               
            return;

        float startX = -(mergeCount - 1) * cardSpacing / 2;             

        for (int i = 0; i < mergeCount; i++)
        {
            if (handCards[i] != null)
            {
                Vector3 newPos = handArea.position + new Vector3(startX + i * cardSpacing, 0, 0);
                handCards[i].transform.position = newPos;
            }

        }
    }

    void OnDrawButtonClicked()              //��ο� ��ư Ŭ�� �� ������ ī�� �̱�
    {
        DrawCardToHand();
    }

    public void DrawCardToHand()             //������ ī�带 �̾� ���з� �̵�
    {
        if (handCount + mergeCount >= maxHandSize)           //���а� ���� á���� Ȯ��
        {
            Debug.Log("ī�� ���� �ִ��Դϴ�. ������ Ȯ���ϼ���!");
            return;
        }
        if (deckCount <= 0)                     //���� ī�尡 �����ִ��� Ȯ��
        {
            Debug.Log("���� �� �̻� ī�尡 �����ϴ�.");
            return;
        }
        GameObject drawnCard = deckCards[0];                    //������ �� ���� ī�带 �������� 

        for (int i = 0; i < deckCount - 1; i++)                  //�� �迭 ���� (������ ��ĭ�� ����)
        {
            deckCards[i] = deckCards[i + 1];
        }
        deckCount--;

        drawnCard.SetActive(true);                              //ī�� Ȱ��ȭ
        handCards[handCount] = drawnCard;                       //���п� ī�� �߰�
        handCount++;

        drawnCard.transform.SetParent(handArea);                //ī���� �θ� ���� �������� ����

        ArrangeHand();                                          //���� ����
    }

    public void MoveCardToMerge(GameObject card)             //ī�带 ���� �������� �̵�
    {
        if (mergeCount >= maxMergeSize)           //���� ������ ���� á���� Ȯ��
        {
            Debug.Log("���� ������ ���� á���ϴ�!");
            return;
        }


        for (int i = 0; i < handCount; i++)                  //ī�尡 ���п� �ִ��� Ȯ���ϰ� ����
        {
            if (handCards[i] == card)
            {
                for(int j = i; j < handCount - 1; j++)       //ī�带 �����ϰ� �迭 ����
                {
                    handCards[j] = handCards[j + 1];
                }
                handCards[handCount - 1] = null;          //�ڵ带 null ���� �ִ´�.
                handCount--;                              //ī��Ʈ�� ���δ�

                ArrangeHand();               //���� ����
                break;                       //for ���� �������´�
            }
        }

        mergeCards[mergeCount] = card;
        mergeCount++;

        card.transform.SetParent(mergeArea);
        ArrangeMerge();
        UpdateMergeButtonState();
    }

    void UpdateMergeButtonState()
    {
        if (mergeButton != null)
        {
            mergeButton.interactable = (mergeCount == 2 || mergeCount == 3);
        }
    }

    void MergeCards()
    {
        if (mergeCount != 2 && mergeCount != 3)
        {
            Debug.Log("������ �Ϸ��� ī�尡 2�� �Ǵ� 3���� �ʿ��մϴ�!");
                return;
        }

        int firstCard = mergeCards[0].GetComponent<Card>().cardValue;
        for (int i = 1; i < mergeCount; i++)
        {
            Card card = mergeCards[i].GetComponent<Card>();
            if(card == null || card.cardValue != firstCard)
            {
                Debug.Log("���� ������ ī�常 ���� �� �� �ֽ��ϴ�");
                return;
            }
            
        }
        int newValue = firstCard + 1;
        
        if(newValue > cardImages.Length)
        {
            Debug.Log("�ִ� ī�� ���� ���� �߽��ϴ�.");
            return;

        }

        for(int i = 0; i < mergeCount; i++)
        {
            if (mergeCards[i] != null)
            {
                mergeCards[i].SetActive(false);
            }
        }

        GameObject newCard = Instantiate(cardPrefab, mergeArea.position, Quaternion.identity);

        Card newCardTemp = newCard.GetComponent<Card>();
        if(newCardTemp != null)
        {
            int imageIndex = newValue - 1;
            newCardTemp.InitCard(newValue, cardImages[imageIndex]);
        }

        for (int i = 0; i < maxMergeSize; i++)
        {
            mergeCards[i] = null; ;
        }
        mergeCount = 0;

        UpdateMergeButtonState();

        handCards[handCount] = newCard;
        handCount++;
        newCard.transform.SetParent(handArea);

        ArrangeHand();
    }

    void OnMergeButtonClicked()
    {
        MergeCards();
    }


}
