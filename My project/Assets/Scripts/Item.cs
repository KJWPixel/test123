using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    InventoryManager inventoryManager;
    [SerializeField] string itemIdx;

    private void Start()
    {
        inventoryManager = InventoryManager.Instance;

        //List<cItemData> listItem = new List<cItemData>();//리스트로 여러개의 아이템으로 쓸수 있는 형태

        //cItemData testData = new cItemData();//json화
        //testData.idx = "00000001";
        //testData.sprite = GetComponent<SpriteRenderer>().sprite.name;

        //cItemData testData2 = new cItemData();//json화
        //testData.idx = "00000001";
        //testData.sprite = GetComponent<SpriteRenderer>().sprite.name;

        //listItem.Add(testData);
        //listItem.Add(testData2);

        //string value = JsonConvert.SerializeObject(testData);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))//닿은대상이 플레이어라면
        {
            if(inventoryManager.GetItem(itemIdx) == true)
            {
                Destroy(gameObject);
            }
            //인벤토리 매니저에게 내가 습득되는 확인
        }
    }
}
