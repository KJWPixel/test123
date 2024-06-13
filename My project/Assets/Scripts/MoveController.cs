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
    Animator anim;
    Vector3 moveDir;//0, 0, 0
    float verticalVelocity = 0f;//�������� �������� ��

    [SerializeField] float jumpForce;
    [SerializeField] float moveSpeed;

    [SerializeField] bool showGroundCheck;
    [SerializeField] float groundCheckLength;//�� ���̰� ���ӿ��� �󸶸�ŭ�� ���̷� �������� �������� ������������ �� ���� ����
    [SerializeField] Color colorGroundCheck;

    [SerializeField] bool isGround;//�ν����Ϳ��� �÷��̾ �÷���Ÿ�Ͽ� ���� �ߴ���

    private void OnDrawGizmos()//üũ�� �뵵
    {
        if(showGroundCheck == true)
        {
            Debug.DrawLine(transform.position, transform.position - new Vector3(0, groundCheckLength), colorGroundCheck);
        }

        //Debug.DrawLine(); ����׵� üũ�뵵�� �� ī�޶� ���� �׷��� �� ����
        //Gizmos.DrawSphere(); ����׺��� �� ���� �ð�ȿ���� ����
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
        else
        {
            isGround= false;
        }
    }

    private void moving()
    {
        //�¿�Ű�� ������ �¿�� �����δ�
        moveDir.x = Input.GetAxisRaw("Horizontal") * moveSpeed;//a, Left Key -1, d, Right Key 1, �ƹ��͵� �Է����� ������ 0 
        moveDir.y = rigid.velocity.y;
        //���ð��� ���鋚�� ������Ʈ�� �ڵ忡 ���ؼ� �����̵� �ϰ� ��������� �̹����� ������ ���ؼ� �̵�     
        rigid.velocity = moveDir;//y 0

        

    }
}
