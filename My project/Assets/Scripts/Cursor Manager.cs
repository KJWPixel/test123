using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [Header("Ŀ�� �̹���")]
    [SerializeField, Tooltip("0,�� <color=red>����Ʈ</color>, 1�� <color=red>Ŭ��</color>")] List<Texture2D> cursor;
    Texture2D[] cursors;


    void Update()
    {
        if(Input.GetKey(KeyCode.Mouse0))//Ŭ���� ������
        {
            Cursor.SetCursor(cursor[1], new Vector2(cursor[1].width * 0.5f, cursor[1].height * 0.5f), CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(cursor[0], new Vector2(cursor[0].width * 0.5f, cursor[0].height * 0.5f), CursorMode.Auto);
        }
    }
}
