using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class summonMgr : MonoBehaviour
{
    public Transform player;
    public Vector3[] summonPosOffsetList;

    private List<summonBase> summonModelList = new List<summonBase>(); // 自定义类 array数组初始化默认值是什么？如果是null的话直接用array就可以了。
    private int maxSummonAmount;

    // Use this for initialization
    void Start()
    {
        maxSummonAmount = summonPosOffsetList.Length;
        for (int num = 0; num < maxSummonAmount; num++)
        {
            summonModelList.Add(null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        var playerPos = player.position;
        foreach (summonBase summonModel in summonModelList)
        {
            summonModel?.UpdateLogic(playerPos);
        }
    }

    public void InstantiateSummon(string name)
    {
        int emptyIndex = -1;
        for (int index = 0; index < maxSummonAmount; index++)
        {
            if (!summonModelList[index])
            {
                emptyIndex = index;
                break;
            }
        }
        if (emptyIndex == -1)
        {
            Debug.Log("reach the summon upper limit!");
            return;
        }

        Object rawObj = Resources.Load("Prefabs/Summons/" + name);
        GameObject summonGameObj = Instantiate(rawObj) as GameObject;
        Transform summonTransform = summonGameObj.transform;
        summonTransform.parent = transform;

        summonModelList[emptyIndex] = summonTransform.GetComponent<summonBase>();
        summonModelList[emptyIndex].PosOffset = summonPosOffsetList[emptyIndex];
        float x = player.position.x + summonPosOffsetList[emptyIndex].x;
        float y = player.position.y + summonPosOffsetList[emptyIndex].y;
        summonTransform.position = new Vector3(x, y, player.position.z);
    }
}