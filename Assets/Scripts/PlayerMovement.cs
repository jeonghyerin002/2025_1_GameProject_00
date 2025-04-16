using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("�⺻ �̵� ����")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float turnSpeed = 10f;

    [Header("���� ���� ����")]
    public float fallMutiplier = 2.5f;
    public float lowJumpMutiplier = 2.0f;

    [Header("���� ���� ����")]
    public float coyoteTime = 0.15f;         //���� ���� �ð�
    public float coyoteTimeCounter;          //���� Ÿ�̸�
    public bool realGround = true;           //���� ���� ����

    [Header("�۶��̴� ����")]
    public GameObject gliderObject;
    public float gliderFallSpeed = 1.0f;
    public float gliderMoveSpeed = 7.0f;
    public float gliderMaxTime = 5.0f;
    public float gliderTimeLeft;
    public bool isGliding = false;

    public bool isGrounded = true;

    public int coinCount = 0;
    public int totalCoins = 5;

    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {

        if (gliderObject != null)
        {
            gliderObject.SetActive(false);
        }

        gliderTimeLeft = gliderMaxTime;
        coyoteTimeCounter = 0;
    }
    

    void ApplyGliderMovement(float horizontal, float vertical)
    {

        Vector3 gliderVelocity = new Vector3(
            horizontal * gliderMoveSpeed,
            -gliderFallSpeed,
            vertical * gliderMoveSpeed
        );

        rb.velocity = gliderVelocity;
    }


    // Update is called once per frame
    void Update()
    {
        //���� ���� ����ȭ
        UpdateGroundedState();

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical); //�̵� ���� ����

        //�Է��� ���� ���� ȸ��
        if (movement.magnitude > 0.1f)       //�Է��� ���� ���� ȸ��
        {
            Quaternion tergetRotation = Quaternion.LookRotation(movement); //�̵� ������ �ٶ󺸵��� �ε巴�� ȸ��
            transform.rotation = Quaternion.Slerp(transform.rotation, tergetRotation, turnSpeed * Time.deltaTime);

        }

        if(Input.GetKey(KeyCode.G) && !isGrounded && gliderTimeLeft > 0)
        {
            if(!isGliding)
            {
                //�۶��̴� Ȱ��ȭ �Լ� (�Ʒ� ����)
                EnableGlider();
            }

            //�۶��̴� ���ð� ����
            gliderTimeLeft -= Time.deltaTime;

            if (gliderTimeLeft <= 0)
            {
                //�۶��̴� ��Ȱ��ȭ �Լ�
                DisableGlider();
            }


        }
        else if (isGliding)
        {
            //GŰ�� ���� �۶��̴� ��Ȱ��ȭ
            DisableGlider();
        }
        
        if(isGliding)
        {
            ApplyGliderMovement(moveHorizontal, moveVertical);
        }
        else
        {
            rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);

            if (rb.velocity.magnitude < 0)
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (fallMutiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMutiplier - 1) * Time.deltaTime;
            }
        }

            //�ӵ��� ���� �̵�
            rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);

        //���� ���� ���� ����
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMutiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMutiplier - 1) * Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)    //&&�� ���� ��� ������ ��
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            realGround = false;
            coyoteTimeCounter = 0;

        }

        if(isGrounded)
        {
            if(isGliding)
            {
                DisableGlider();
            }

            gliderTimeLeft = gliderMaxTime;
        }

        

    }
    void EnableGlider()
    {
        isGliding = true;
        if (gliderObject != null)
        {
            gliderObject.SetActive(true);
        }

        rb.velocity = new Vector3(rb.velocity.x, -gliderFallSpeed, rb.velocity.z);
    }

    void DisableGlider()
    {
        isGliding = false;

        if (gliderObject != null)
        {
            gliderObject.SetActive(false);
        }
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }
  

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            realGround = true;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            realGround = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            realGround = false;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        
        if(other.CompareTag("Coin"))
        {
            coinCount++;
            Destroy(other.gameObject);
            Debug.Log($"���μ��� : {coinCount}/{totalCoins}");

        }

        if(other.gameObject.tag == "Door" &&coinCount == totalCoins)
        {
            Debug.Log("���� Ŭ����");

        }
    }
    void UpdateGroundedState()
    {
        if(realGround)
        {
            coyoteTimeCounter = coyoteTime;
            isGrounded = true;
        }
        else
        {
            if (coyoteTimeCounter > 0)
            {
                coyoteTimeCounter -= Time.deltaTime;
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }
    }
    
}


