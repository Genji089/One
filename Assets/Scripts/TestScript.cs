using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public ArrayList arrayList;
    // Start is called before the first frame update
    void Start()
    {
        arrayList = new ArrayList
        {
            1,
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            int i = 1;
            i = i + 2 << 1; //结果，优先加，再移位
            //Debug.Log(i);

            i = 9;
            Debug.Log(Mathf.Sqrt(2));
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Debug.Log(-2 % 2);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log(arrayList.Count);
            AddExtraData(arrayList);
            Debug.Log(arrayList.Count);
        }
    }

    void AddExtraData(ArrayList array)
    {
        array.Add(2);
    }
}
