using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    Camera mainCam;
    [SerializeField] Transform trsHand;
    [SerializeField] GameObject objThrowWeapon;
    [SerializeField] Transform trsWeapon;
    private void Start()
    {
        mainCam = Camera.main;//����ī�޶�
        //ī�޶� 2�� �̻��� ��쵵 ������
        //ī�޶�� �±�: ����ī�޶�� �ϳ����� �Ѵ�, ī�޶�� ����� ������ ������Ʈ�� �� 
        //����� �������� ��� �ϳ��� ���� �־���Ѵ�.
        //Camera.current �̷� ������� ��밡��
    }

    void Update()
    {
        checkAim();
    }

    private void checkAim()
    {
        Vector2 mouseWorldPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = transform.position;
        Vector2 fixedPos = mouseWorldPos - playerPos;

        //fixedPos.x  > 0 �Ǵ� transform.localScale.x -1 => ������, 1 => ����

        //float angle;
        //if (transform.localScale.x == -1)
        //{
        //    angle = Quaternion.FromToRotation(Vector3.right, fixedPos).eulerAngles.z;
        //}
        //else
        //{
        //    angle = Quaternion.FromToRotation(Vector3.left, fixedPos).eulerAngles.z;
        //}
        float angle = Quaternion.FromToRotation(transform.localScale.x < 0 ? Vector3.right : Vector3.left, fixedPos).eulerAngles.z;//?
        trsHand.rotation = Quaternion.Euler(0, 0, angle); //rotation = Quaternion�� EulerRotation�� ����
        

    }

    private void checkCreate()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            createWeapon();
        }
    }

    private void createWeapon()
    {
        //GameObject go = Instantiate(objThrowWeapon, );
    }
}
