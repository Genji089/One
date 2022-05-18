using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponNormal : weaponBase
{
    #region Inspector
    public Transform muzzle;

    #endregion

    private float attackCount;

    // Start is called before the first frame update
    void Start()
    {
        attackCount = attackCd;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Attack()
    {
        base.Attack();
        if (attackCount >= attackCd)
        {
            // do attack
            if (attackProduct)
            {
                Instantiate(attackProduct, muzzle.position, Quaternion.Euler(saveWorldRotation));
            }
            attackCount = 0;
        }
        attackCount += Time.deltaTime;
    }

    public override void StopAttack()
    {
        base.StopAttack();
        attackCount = attackCd;
    }
}
