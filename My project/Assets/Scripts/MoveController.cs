using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MoveController : MonoBehaviour
{
    //manager, �񵿱������� ȣ���� ���������� ����
    //controller, updata��� ���������� ȣ���� ���� �ʴ��� Ÿ ����� �ҷ��� ����ϴ� ��찡 ����

    [Header("�÷��̾� �̵� �� ����")]
    Rigidbody2D rigid;//null
    CapsuleCollider2D coll;
    Animator anim;//null
    Vector3 moveDir;//0, 0, 0
    float verticalVelocity = 0f;//�������� �������� ��

    [SerializeField] float jumpForce;
    [SerializeField] float moveSpeed;

    [SerializeField] bool showGroundCheck;
    [SerializeField] float groundCheckLength;//�� ���̰� ���ӿ��� �󸶸�ŭ�� ���̷� �������� �������� ������������ �� ���� ����
    [SerializeField] Color colorGroundCheck;

    [SerializeField] bool isGround;//�ν����Ϳ��� �÷��̾ �÷���Ÿ�Ͽ� ���� �ߴ���
    bool isJump;

    private void OnDrawGizmos()//üũ�� �뵵
    {
        if(showGroundCheck == true)
        {
            Debug.DrawLine(transform.position, transform.position - new Vector3(0, groundCheckLength), colorGroundCheck);

            //float sphereRange = 5; Gizmos �׽�Ʈ
            //Vector3 cubeSize = new Vector3(3, 10, 7);
            //Gizmos.color = colorGroundCheck;
            //Gizmos.DrawWireSphere(transform.position, sphereRange);
            //Gizmos.DrawWireCube(transform.position, cubeSize);
            
        }

        //Debug.DrawLine(); ����׵� üũ�뵵�� �� ī�޶� ���� �׷��� �� ����
        //Gizmos.DrawSphere(); ����׺��� �� ���� �ð�ȿ���� ����
        //Handles.DrawWireArc
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        
    }
    void Update()
    {
        checkGround();

        moving();
        jump();

        chedkGravity();

        doAnim();
    }

    private void checkGround()
    {
        isGround = false;

        if (verticalVelocity > 0f)
        {
            return;
        }

        //float.PositiveInfinity //����Ƽ��, �װ�Ƽ��(�ݴ����) �������� �߻� 
        //Layer int�� ����� ���̾ ����
        //Layer�� int�� ���������� Ȱ���ϴ� int�� �ٸ�
        //Wall Layer, Ground Layer
        RaycastHit2D hit = 
        Physics2D.Raycast(transform.position, Vector2.down, groundCheckLength, LayerMask.GetMask("Ground")); //���� ��ġ�� ����(origin), ����(direction), Vector2.donw == new vcetor(0, -1)

        if(hit)//Raycast Ray�� ��Ҵٸ� true
        {
            isGround = true;
        }
    }

    private void moving()
    {
        //�¿�Ű�� ������ �¿�� �����δ�
        moveDir.x = Input.GetAxisRaw("Horizontal") * moveSpeed;//a, Left Key -1, d, Right Key 1, �ƹ��͵� �Է����� ������ 0 
        moveDir.y = rigid.velocity.y;
        //���ð��� ���鋚�� ������Ʈ�� �ڵ忡 ���ؼ� �����̵� �ϰ� ��������� �̹����� ������ ���ؼ� �̵�     
        rigid.velocity = moveDir;//y 0 Time.deltaTime
    }

    private void jump()
    {
        //if (isGround == true && Input.GetKeyDown(KeyCode.Space)) //Getkey �ѹ��� �ϳ��� �޾Ƶ���, ��� üũ��
        //{
        //    //Vector2 force = rigid.velocity;
        //    //force.y = 0;
        //    //rigid.velocity = force;
        //    rigid.AddForce(new Vector2(0,jumpForce), ForceMode2D.Impulse); //������ �̴� ������ ����� 
        //}
        //rigid.�������

        //3D���� �����߷� �����ؼ� ����
        if (isGround == false )
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Space) == true)
        {
            isJump = true;
        }
    }

    private void chedkGravity()
    {
        if(isGround == false)//���߿� ���ִ� ����
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime; //-9.81 

            if (verticalVelocity < -10f) //�߷� -10���Ϸ� ������ �� ����
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

