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

        //List<cItemData> listItem = new List<cItemData>();//����Ʈ�� �������� ���������� ���� �ִ� ����

        //cItemData testData = new cItemData();//jsonȭ
        //testData.idx = "00000001";
        //testData.sprite = GetComponent<SpriteRenderer>().sprite.name;

        //cItemData testData2 = new cItemData();//jsonȭ
        //testData.idx = "00000001";
        //testData.sprite = GetComponent<SpriteRenderer>().sprite.name;

        //listItem.Add(testData);
        //listItem.Add(testData2);

        //string value = JsonConvert.SerializeObject(testData);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))//��������� �÷��̾���
        {
            if(inventoryManager.GetItem(itemIdx) == true)
            {
                Destroy(gameObject);
            }
            //�κ��丮 �Ŵ������� ���� ����Ǵ� Ȯ��
        }
    }
}
