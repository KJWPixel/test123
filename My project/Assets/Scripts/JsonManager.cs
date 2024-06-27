using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.TextCore.Text;

public class JsonManager : MonoBehaviour
{
    public static JsonManager Instance;

    //TextAsset itemData;
    List<cItemData> itemDatas;

    private void Awake()
    {
        //itemData = (TextAsset)Resources.Load("ItemData");//���⽱��, ������ ���� �ش��� �Ʒ��� ������� ���� 
        //itemData = Resources.Load<TextAsset>("ItemData");//�߸��Ǹ� null�� ������

        //TextAsset itemData = Resources.Load("ItemData") as TextAsset;////�߸��Ǹ� null�� ������
        //itemDatas = JsonConvert.DeserializeObject<List<cItemData>>(itemData.ToString());

        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        initJsonDatas();
    }

    private void initJsonDatas()
    {
        TextAsset itemData = Resources.Load("ItemData") as TextAsset;
        itemDatas = JsonConvert.DeserializeObject<List<cItemData>>(itemData.ToString());
    }

    public string GetNameFromIdx(string _idx)
    {
        if(itemDatas == null) return string.Empty;

        return itemDatas.Find(x => x.idx == _idx).sprite;
    }
}

