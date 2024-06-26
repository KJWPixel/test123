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
        //itemData = (TextAsset)Resources.Load("ItemData");//쓰기쉽다, 에러가 나면 해당줄 아래로 기능하지 않음 
        //itemData = Resources.Load<TextAsset>("ItemData");//잘못되면 null로 리턴함

        //TextAsset itemData = Resources.Load("ItemData") as TextAsset;////잘못되면 null로 리턴함
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

        //cWeapon data = new cWeapon(); //중요
        //itemDatas.Add(data);
    }

    public string GetSpriteNameFromIdx(string _idx)//큐플 무명 클래스
    {
        if(itemDatas == null) return string.Empty;

        return itemDatas.Find(x => x.idx == _idx).sprite;//람다식, Find  c# > c++로 변환하는 과정: il2cpp
    }
}

