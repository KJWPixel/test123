using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MoveController : MonoBehaviour
{
    //manager, 비동기적으로 호출이 왔을때에만 대응
    //controller, updata사용 동기적으로 호출이 오지 않더라도 타 기능을 불러서 사용하는 경우가 많음

    [Header("플레이어 이동 및 점프")]
    Rigidbody2D rigid;//null
    CapsuleCollider2D coll;
    Animator anim;
    Vector3 moveDir;//0, 0, 0
    float verticalVelocity = 0f;//수직으로 떨어지는 힘

    [SerializeField] float jumpForce;
    [SerializeField] float moveSpeed;

    [SerializeField] bool showGroundCheck;
    [SerializeField] float groundCheckLength;//이 길이가 게임에서 얼마만큼의 길이로 나오는지 육안으로 보기전까지는 알 수가 없음
    [SerializeField] Color colorGroundCheck;

    [SerializeField] bool isGround;//인스펙터에서 플레이어가 플랫폼타일에 착지 했는지

    private void OnDrawGizmos()//체크의 용도
    {
        if(showGroundCheck == true)
        {
            Debug.DrawLine(transform.position, transform.position - new Vector3(0, groundCheckLength), colorGroundCheck);
        }

        //Debug.DrawLine(); 디버그도 체크용도로 씬 카메라에 선을 그려줄 수 있음
        //Gizmos.DrawSphere(); 디버그보다 더 많은 시각효과를 제공
        //Handles.DrawWireArc
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        
    }
    void Update()
    {
        checkGround();

        moving();

    }

    private void checkGround()
    {
        //float.PositiveInfinity //포지티브, 네거티브(반대방향) 무한으로 발사 

        //Layer int로 대상의 레이어를 구분
        //Layer의 int와 공통적으로 활용하는 int와 다름
        //Wall Layer, Ground Layer
        RaycastHit2D hit = 
        Physics2D.Raycast(transform.position, Vector2.down, groundCheckLength, LayerMask.GetMask("Ground")); //최초 위치로 부터(origin), 방향(direction), Vector2.donw == new vcetor(0, -1)

        if(hit)//Raycast Ray가 닿았다면 true
        {
            isGround = true;
        }
        else
        {
            isGround= false;
        }
    }

    private void moving()
    {
        //좌우키를 누르면 좌우로 움직인다
        moveDir.x = Input.GetAxisRaw("Horizontal") * moveSpeed;//a, Left Key -1, d, Right Key 1, 아무것도 입력하지 않으면 0 
        moveDir.y = rigid.velocity.y;
        //슈팅게임 만들떄는 오브젝트를 코드에 의해서 순간이동 하게 만들었지만 이번에는 물리에 의해서 이동     
        rigid.velocity = moveDir;//y 0

        

    }
}
