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
        mainCam = Camera.main;//메인카메라
        //카메라가 2개 이상일 경우도 존재함
        //카메라는 태그: 메인카메라는 하나여야 한다, 카메라는 오디오 리스너 컴포넌트가 들어감 
        //오디오 리스너의 경우 하나만 켜져 있어야한다.
        //Camera.current 이런 방식으로 사용가능
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

        //fixedPos.x  > 0 또는 transform.localScale.x -1 => 오른쪽, 1 => 왼쪽

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
        trsHand.rotation = Quaternion.Euler(0, 0, angle); //rotation = Quaternion을 EulerRotation로 변경
        

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
