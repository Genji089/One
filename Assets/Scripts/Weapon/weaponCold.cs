using UnityEngine;
using System.Collections;


public class weaponCold : weaponBase
{
    private BoxCollider2D attackTrigger;
    // Use this for initialization
    void Start()
    {
        attackTrigger = GetComponent<BoxCollider2D>();
        attackTrigger.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnAttackKeyFrame()
    {
        attackTrigger.enabled = true;
    }

    void OnAttackEnd()
    {
        attackTrigger.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}