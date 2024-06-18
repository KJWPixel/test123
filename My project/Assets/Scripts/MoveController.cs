using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class MoveController : MonoBehaviour
{
    //manager, 비동기적으로 호출이 왔을때에만 대응
    //controller, updata사용 동기적으로 호출이 오지 않더라도 타 기능을 불러서 사용하는 경우가 많음
    [Header("플레이어 이동 및 점프")]
    Rigidbody2D rigid;//null
    CapsuleCollider2D coll;
    BoxCollider2D box2d;
    Animator anim;//null
    Vector3 moveDir;//0, 0, 0
    float verticalVelocity = 0f;//수직으로 떨어지는 힘

    [SerializeField] float jumpForce;
    [SerializeField] float moveSpeed;

    [SerializeField] bool showGroundCheck;
    [SerializeField] float groundCheckLength;//이 길이가 게임에서 얼마만큼의 길이로 나오는지 육안으로 보기전까지는 알 수가 없음

    [SerializeField] Color colorGroundCheck;

    [SerializeField] bool isGround;//인스펙터에서 플레이어가 플랫폼타일에 착지 했는지
    bool isJump;

    Camera camMain;

    [Header("벽 점프")]
    [SerializeField] bool touchWall;
    bool isWallJump;
    [SerializeField] float wallJumpTime = 0.3f;
    float wallJumpTimer = 0.0f;//타이머

    private void OnDrawGizmos()//체크의 용도
    {
        if(showGroundCheck == true)
        {
            Debug.DrawLine(transform.position, transform.position - new Vector3(0, groundCheckLength), colorGroundCheck);

            //float sphereRange = 5; Gizmos 테스트
            //Vector3 cubeSize = new Vector3(3, 10, 7);
            //Gizmos.color = colorGroundCheck;
            //Gizmos.DrawWireSphere(transform.position, sphereRange);
            //Gizmos.DrawWireCube(transform.position, cubeSize);
        }
        //Debug.DrawLine(); 디버그도 체크용도로 씬 카메라에 선을 그려줄 수 있음
        //Gizmos.DrawSphere(); 디버그보다 더 많은 시각효과를 제공
        //Handles.DrawWireArc
    }

    //private void OnTriggerEnter2D(Collider2D collision)//상대방의 콜라이더를 가져옴, 누가 실행시킨지는 모름
    //{
    //    if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
    //    {
    //        touchWall = true;
    //    }
    //}
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.layer == LayerMask.NameToLayer("Wall")) 
    //    {
    //        touchWall = false;
    //    }
    //}

    public void TriggerEnter(HitBox.ehitboxType _type, Collider2D _collision)
    {
        if(_type == HitBox.ehitboxType.WallCheck)
        {
            if (_collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                touchWall = true;
            }
        }
    }

    public void TriggerExit(HitBox.ehitboxType _type, Collider2D _collision)
    {
        if (_type == HitBox.ehitboxType.WallCheck)
        {
            if (_collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                touchWall = false;
            }
        }
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        box2d = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        camMain = Camera.main;
    }
    void Update()
    {
        checkGround();

        moving();
        checkAim();
        doAnim();
        jump();

        chedkGravity();
    }

    private void checkGround()
    {
        isGround = false;

        if (verticalVelocity > 0f)
        {
            return;
        }

        //float.PositiveInfinity //포지티브, 네거티브(반대방향) 무한으로 발사 
        //Layer int로 대상의 레이어를 구분
        //Layer의 int와 공통적으로 활용하는 int와 다름
        //Wall Layer, Ground Layer
        RaycastHit2D hit = 
        //Physics2D.Raycast(transform.position, Vector2.down, groundCheckLength, LayerMask.GetMask("Ground")); //최초 위치로 부터(origin), 방향(direction), Vector2.donw == new vcetor(0, -1)

       

        Physics2D.BoxCast(box2d.bounds.center, box2d.bounds.size, 0f, Vector2.down, groundCheckLength, LayerMask.GetMask("Ground"));

        if (hit)//Raycast Ray가 닿았다면 true
        {
            isGround = true;
        }
    }

    private void moving()
    {
        //좌우키를 누르면 좌우로 움직인다
        moveDir.x = Input.GetAxisRaw("Horizontal") * moveSpeed;//a, Left Key -1, d, Right Key 1, 아무것도 입력하지 않으면 0 
        moveDir.y = rigid.velocity.y;
        //슈팅게임 만들떄는 오브젝트를 코드에 의해서 순간이동 하게 만들었지만 이번에는 물리에 의해서 이동     
        rigid.velocity = moveDir;//y 0 Time.deltaTime
    }

    private void checkAim()
    {
        //방향키에 따른 플레이어 애니메이션 방향
        //Vector3 scale = transform.localScale;
        //if (moveDir.x < 0 && scale.x != 1.0f)//왼쪽
        //{
        //    scale.x = 1.0f;
        //    transform.localScale = scale;
        //    Debug.Log("<color=blue>동작</color>");
        //}
        //else if (moveDir.x > 0 && scale.x != -1.0f)//오른쪽 
        //{
        //    scale.x = -1.0f;
        //    transform.localScale = scale;
        //    Debug.Log("<color=rad>동작</color>");
        //}      

        //마우스커서에 따른 플레이어 애니메이션 방향
        //WorldPoint로 하지 않을 시 캔버스 기준으로 좌표를 찍음
        //WorldPoint의 경우 정중앙이 x 0 y 0으로 찍힘
        Vector2 mouseWorldPos = camMain.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = transform.position;
        Vector2 fixedPos = mouseWorldPos - playerPos;

        Vector3 plalyerScale = transform.localScale;
        if (fixedPos.x > 0 && plalyerScale.x != -1.0f)
        {
            plalyerScale.x = -1.0f;
        }
        else if (fixedPos.x < 0 && plalyerScale.x != 1.0f)
        {
            plalyerScale.x = 1.0f;
        }
        transform.localScale = plalyerScale;

    }

    private void jump()
    {
        //if (isGround == true && Input.GetKeyDown(KeyCode.Space)) //Getkey 한번에 하나씩 받아들임, 계속 체크함
        //{
        //    //Vector2 force = rigid.velocity;
        //    //force.y = 0;
        //    //rigid.velocity = force;
        //    rigid.AddForce(new Vector2(0,jumpForce), ForceMode2D.Impulse); //지긋이 미는 힘으로 적용됨 
        //}
        //rigid.여러기능

        //3D에는 직접중력 관련해서 정리
        if (isGround == false )//공중에 떠있는 상태라면
        {
            // 벽에 붙어있고, 그리고 벽을 향해 플레이어가 방향키를 누르고 있는데 점프키를 눌렀다면
            if(touchWall == true && moveDir.x != 0f && Input.GetKeyDown(KeyCode.Space))
            {
                isWallJump = true;
            }

            return;
        }

        if(Input.GetKeyDown(KeyCode.Space) == true)
        {
            isJump = true;
        }
    }

    private void chedkGravity()
    {
        if (isWallJump == true)
        {
            isWallJump = false;

            Vector2 dir = rigid.velocity;
            dir.x *= -1f;//반대방향
            rigid.velocity = dir;

            verticalVelocity = jumpForce * 0.5f;
            //일정시간 유저가 입력할 수 없어야 벽을 발로찬 x값을 볼 수 있음
            //입력불가 타이머를 작동시켜야함
        }
        else if(isGround == false)//공중에 떠있는 상태
        {
             verticalVelocity += Physics.gravity.y * Time.deltaTime; //-9.81 

             if (verticalVelocity < -10f) //중력 -10이하로 내려갈 수 없음
             {
                verticalVelocity = -10f;
             }
        }
        else if (isJump == true)
        {
            isJump= false;
            verticalVelocity = jumpForce;
        }
        else if (isGround == true)
        {
            verticalVelocity = 0;
        }

        rigid.velocity = new Vector2(rigid.velocity.x, verticalVelocity);
    }

    private void doAnim()
    {
        anim.SetInteger("Horizontal", (int)moveDir.x);
        anim.SetBool("IsGround", isGround);
    }

}

