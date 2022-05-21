using UnityEngine;
using System.Collections;


public class bulletBase : MonoBehaviour
{
    public float moveSpeed;
    public float aliveTime;
    public bool isTriggerDestory;
    public string bulletType;
    public float damage = 0;

    private float aliveCount;

    // Use this for initialization
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        AliveCounter();
        MoveMent();
    }

    public virtual void MoveMent()
    {
        float moveDistance = moveSpeed * Time.deltaTime;
        float radian = transform.localEulerAngles.z * Mathf.Deg2Rad;
        var Offset = new Vector3(Mathf.Cos(radian) * moveDistance, Mathf.Sin(radian) * moveDistance, 0);
        transform.position += Offset;
    }

    public virtual void AliveCounter()
    {
        aliveCount += Time.deltaTime;
        if (aliveCount >= aliveTime)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTriggerDestory)
        { 
            Destroy(gameObject);
        }
    }

    public ArrayList GetHitData()
    {
        ArrayList arrayList = new ArrayList
        {
            damage,
        };
        AddHitExtraData(arrayList);
        return arrayList;
    }

    protected virtual void AddHitExtraData(ArrayList arrayList)
    {
        
    }
}