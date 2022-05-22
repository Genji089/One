using UnityEngine;
using System.Collections;


public class bulletShotgun : bulletBase
{
    public float hitBackDistance;
    public float hitBackSpeed;
    public float hitBackTime;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        bulletType = "shotgun";
    }

    protected override void AddHitExtraData(ArrayList arrayList)
    {
        arrayList.Add(hitBackTime);
        arrayList.Add(hitBackSpeed);
    }
}