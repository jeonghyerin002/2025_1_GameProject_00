using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("기본 이동 설정")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float turnSpeed = 10f;

    [Header("점프 개선 설정")]
    public float fallMutiplier = 2.5f;
    public float lowJumpMutiplier = 2.0f;

    [Header("지면 감지 설정")]
    public float coyoteTime = 0.15f;         //지면 관성 시간
    public float coyoteTimeCounter;          //관성 타이머
    public bool realGround = true;           //실제 지면 상태

    [Header("글라이더 설정")]
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
        //지면 감지 안정화
        UpdateGroundedState();

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical); //이동 방향 감지

        //입력이 있을 때만 회전
        if (movement.magnitude > 0.1f)       //입력이 있을 때만 회전
        {
            Quaternion tergetRotation = Quaternion.LookRotation(movement); //이동 방향을 바라보도록 부드럽게 회전
            transform.rotation = Quaternion.Slerp(transform.rotation, tergetRotation, turnSpeed * Time.deltaTime);

        }

        if(Input.GetKey(KeyCode.G) && !isGrounded && gliderTimeLeft > 0)
        {
            if(!isGliding)
            {
                //글라이더 활성화 함수 (아래 정의)
                EnableGlider();
            }

            //글라이더 사용시간 감소
            gliderTimeLeft -= Time.deltaTime;

            if (gliderTimeLeft <= 0)
            {
                //글라이더 비활성화 함수
                DisableGlider();
            }


        }
        else if (isGliding)
        {
            //G키를 때면 글라이더 비활성화
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

            //속도로 직접 이동
            rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);

        //착지 점프 높이 구현
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMutiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMutiplier - 1) * Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)    //&&두 값이 모두 만족할 때
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
            Debug.Log($"코인수집 : {coinCount}/{totalCoins}");

        }

        if(other.gameObject.tag == "Door" &&coinCount == totalCoins)
        {
            Debug.Log("게임 클리어");

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


