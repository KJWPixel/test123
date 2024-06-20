using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;//메인카메라
        //카메라가 2개 이상일 경우도 존재함
        //카메라는 태그: 메인카메라는 하나여야 한다, 카메라는 오디오 리스너 컴포넌트가 들어감 
        //오디오 리스너의 경우 하나만 켜져 있어야한다.
        //Camera.current
    }

    void Update()
    {
        checkAim();
    }

    private void checkAim()
    {
        Vector2 mouseWorldPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
    }
}
