using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [SerializeField] GameObject viewInventory;//인벤토리 뷰
    [SerializeField] GameObject fabItem;//인벤토리에 생성될 프리팹

    List<Transform> listTrsInventory = new List<Transform>(); 

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        initInventory();
    }
    private void initInventory()
    {
       listTrsInventory.Clear();

        Transform[] childs = viewInventory.GetComponentsInChildren<Transform>();//자식이 다수면 GetComponents //GetComponents는 자기자신을 포함함 
        
        listTrsInventory.AddRange(childs);//1개의 데이터면 Add, 2개 이상의 데이터면 AddRange
        listTrsInventory.RemoveAt(0);     //자기자신을 제외하고 인벤토리에 있는 데이터를 담음

    }

    /// <summary>
    /// 인벤토리가 열려있다면 닫힘, 닫혀있다면 열림
    /// </summary>
    public void InActiveInventory()
    {
        //if(viewInventory.activeSelf == true)
        //{
        //    viewInventory.SetActive(false);
        //}
        //else
        //{
        //    viewInventory.SetActive(true);
        //}

        viewInventory.SetActive(!viewInventory.activeSelf);
    }

    /// <summary>
    /// 비어있는 인벤토리 넘버를 리턴합니다. -1이 리턴된다면 비어있는 슬롯이 없다는 의미입니다.
    /// </summary>
    /// <returns>비어있는 아이템 슬롯 번호</returns>
    private int getEmptyItemSlot()
    {
        int count = listTrsInventory.Count;
        for(int iNum = 0; iNum < count; ++iNum)
        {
            Transform trsSlot = listTrsInventory[iNum];
            if(trsSlot.childCount == 0)
            {
                return iNum;
            }
        }
        return -1;
    }
}
