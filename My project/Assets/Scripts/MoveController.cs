using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoveController : MonoBehaviour
{
    //manager, �񵿱������� ȣ���� ���������� ����
    //controller, updata��� ���������� ȣ���� ���� �ʴ��� Ÿ ����� �ҷ��� ����ϴ� ��찡 ����
    [Header("�÷��̾� �̵� �� ����")]
    Rigidbody2D rigid;//null
    CapsuleCollider2D coll;
    BoxCollider2D box2d;
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
    bool twoJump;

    Camera camMain;

    [Header("�� ����")]
    [SerializeField] bool touchWall;
    bool isWallJump;
    [SerializeField] float wallJumpTime = 0.3f;
    float wallJumpTimer = 0.0f;//Ÿ�̸�

    [Header("���")]
    [SerializeField] private float dashTime = 0.3f;
    [SerializeField] private float dashSpeed = 20.0f;
    float dashTimer = 0.0f;//Ÿ�̸�
    TrailRenderer dashEffect;//null
    [SerializeField] private float dashCoolTime = 2f;
    float dashCoolTimer = 0.0f;//Ÿ�̸�

    [Header("��� UI")]
    [SerializeField] GameObject objDashCoolTime;
    [SerializeField] Image imgFill;
    [SerializeField] TMP_Text textCoolTime;

    [SerializeField] KeyCode DashKey;


    private void OnDrawGizmos()//üũ�� �뵵
    {
        ///gameObject.SetActive(false);

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

    //private void OnTriggerEnter2D(Collider2D collision)//������ �ݶ��̴��� ������, ���� �����Ų���� ��
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
        dashEffect = GetComponent<TrailRenderer>();
        dashEffect.enabled = false;
        initUI();
    }

    void Start()
    {
        camMain = Camera.main;
    }
    void Update()
    {

        checkTimers();
        checkGround();

        dash();

        moving();
        checkAim();  
        jump();
        doubleJump();

        chedkGravity();

        doAnim();
    }

    private void dash()
    {
        if (dashTimer == 0.0f && dashCoolTimer == 0.0f && (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.F)))
        {
            //Input.GetKeyDown(DashKey);
            dashTimer = dashTime;
            dashCoolTimer = dashCoolTime;
            verticalVelocity = 0;
            dashEffect.enabled = true;
            //if (transform.localScale.x > 0)//����
            //{
            //    rigid.velocity = new Vector2(-dashSpeed, verticalVelocity);
            //}
            //else//������
            //{
            //    rigid.velocity = new Vector2(dashSpeed, verticalVelocity);
            //}             

            //rigid.velocity = transform.localScale.x > 0 ? new Vector2(-dashSpeed, verticalVelocity) : new Vector2(dashSpeed, verticalVelocity); ���ߴ��׽�

            rigid.velocity = new Vector2(transform.localScale.x > 0 ? -dashSpeed : dashSpeed, 0.0f); //Vector2�ȿ��� ���ߴ��׽�
        }
    }

    private void checkTimers()
    {
        if (wallJumpTimer > 0.0f)
        {
            wallJumpTimer -= Time.deltaTime;//?
            if(wallJumpTimer < 0.0f)
            {
                wallJumpTimer = 0.0f;
            }
        }

        if(dashTimer > 0.0f)
        {
            dashTimer -= Time.deltaTime;
            if(dashTimer < 0.0f)
            {
                dashTimer = 0.0f;
                dashEffect.enabled = false;
                dashEffect.Clear(); //������ �ִ� ������ �����
            }
        }

        if(dashCoolTimer > 0.0f)
        {
            //objDashCoolTime.SetActive(true);//�̷��� ��밡��
            if (objDashCoolTime.activeSelf == false)
            {
                objDashCoolTime.SetActive(true);
            }

            dashCoolTimer -= Time.deltaTime;
            if(dashCoolTimer < 0.0f)
            {
                dashCoolTimer = 0.0f;
                objDashCoolTime.SetActive(false);
            }

            //dashCoolTime = 2��, ��ų�� ���� 0, ���� 1�� �Ǿ����
            //2(Ÿ�̸�)/2(�ִ�Ÿ�̸�) = 1, 0.5, 0
            //0, 0.5, 1

            imgFill.fillAmount = 1 - dashCoolTimer / dashCoolTime;//0, 0.5, 1
            textCoolTime.text = dashCoolTimer.ToString("F1");//�Ҽ��� ù° �ڸ��� ���
        }
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
        //Physics2D.Raycast(transform.position, Vector2.down, groundCheckLength, LayerMask.GetMask("Ground")); //���� ��ġ�� ����(origin), ����(direction), Vector2.donw == new vcetor(0, -1)
   
        Physics2D.BoxCast(box2d.bounds.center, box2d.bounds.size, 0f, Vector2.down, groundCheckLength, LayerMask.GetMask("Ground"));

        if (hit)//Raycast Ray�� ��Ҵٸ� true
        {
            isGround = true;
        }
    }

    private void moving()
    {
        if (wallJumpTimer > 0.0f || dashTimer > 0.0f)
        {
            return;
        }     
        //�¿�Ű�� ������ �¿�� �����δ�
        moveDir.x = Input.GetAxisRaw("Horizontal") * moveSpeed;//a, Left Key -1, d, Right Key 1, �ƹ��͵� �Է����� ������ 0 
        moveDir.y = rigid.velocity.y;
        //���ð��� ���鋚�� ������Ʈ�� �ڵ忡 ���ؼ� �����̵� �ϰ� ��������� �̹����� ������ ���ؼ� �̵�     
        rigid.velocity = moveDir;//y 0 Time.deltaTime
    }

    private void checkAim()
    {
        //����Ű�� ���� �÷��̾� �ִϸ��̼� ����
        //Vector3 scale = transform.localScale;
        //if (moveDir.x < 0 && scale.x != 1.0f)//����
        //{
        //    scale.x = 1.0f;
        //    transform.localScale = scale;
        //    Debug.Log("<color=blue>����</color>");
        //}
        //else if (moveDir.x > 0 && scale.x != -1.0f)//������ 
        //{
        //    scale.x = -1.0f;
        //    transform.localScale = scale;
        //    Debug.Log("<color=rad>����</color>");
        //}      

        //���콺Ŀ���� ���� �÷��̾� �ִϸ��̼� ����
        //WorldPoint�� ���� ���� �� ĵ���� �������� ��ǥ�� ����
        //WorldPoint�� ��� ���߾��� x 0 y 0���� ����
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
        //if (isGround == true && Input.GetKeyDown(KeyCode.Space)) //Getkey �ѹ��� �ϳ��� �޾Ƶ���, ��� üũ��
        //{
        //    //Vector2 force = rigid.velocity;
        //    //force.y = 0;
        //    //rigid.velocity = force;
        //    rigid.AddForce(new Vector2(0,jumpForce), ForceMode2D.Impulse); //������ �̴� ������ ����� 
        //}
        //rigid.�������

        //3D���� �����߷� �����ؼ� ����
        if (isGround == false )//���߿� ���ִ� ���¶��
        {
            // ���� �پ��ְ�, �׸��� ���� ���� �÷��̾ ����Ű�� ������ �ִµ� ����Ű�� �����ٸ�
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
        twoJump = false;
    }

    private void chedkGravity()
    {
        if (dashTimer > 0.0f)
        {
            return;
        }
        else if (isWallJump == true)
        {
            isWallJump = false;

            Vector2 dir = rigid.velocity;
            dir.x *= -1f;//�ݴ����
            rigid.velocity = dir;

            verticalVelocity = jumpForce * 0.5f;
            //�����ð� ������ �Է��� �� ����� ���� �߷��� x���� �� �� ����
            //�ԷºҰ� Ÿ�̸Ӹ� �۵����Ѿ���
            wallJumpTimer = wallJumpTime;      
        }
        else if(isGround == false)//���߿� ���ִ� ����
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

    private void initUI()
    {
        objDashCoolTime.SetActive(false);
        imgFill.fillAmount = 0;
        textCoolTime.text = "";
    }

    private void doubleJump()
    {
       if(isGround == false && isJump == false && twoJump == false && Input.GetKeyDown(KeyCode.Space))
        {
            verticalVelocity = jumpForce;
            twoJump = true;
        }
    }

}

