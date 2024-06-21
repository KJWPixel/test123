using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [Header("커서 이미지")]
    [SerializeField, Tooltip("0,은 <color=red>디폴트</color>, 1은 <color=red>클릭</color>")] List<Texture2D> cursor;
    Texture2D[] cursors;


    void Update()
    {
        if(Input.GetKey(KeyCode.Mouse0))//클릭을 했을때
        {
            Cursor.SetCursor(cursor[1], new Vector2(cursor[1].width * 0.5f, cursor[1].height * 0.5f), CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(cursor[0], new Vector2(cursor[0].width * 0.5f, cursor[0].height * 0.5f), CursorMode.Auto);
        }
    }
}
