using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponNormal : weaponBase
{
    #region Inspector
    public Transform muzzle;
    public Transform fire;
    public float fireDuration;

    #endregion

    private float attackCount;
    private bool isFire = false;
    private float fireDurationCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        attackCount = attackCd;
        fire.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isFire)
        {
            if (fireDurationCount >= fireDuration)
            {
                fire.gameObject.SetActive(false);
                fireDurationCount = 0;
            }
            else
            {
                fireDurationCount += Time.deltaTime;
            }
        }
    }

    public override void Attack()
    {
        base.Attack();
        if (attackCount >= attackCd)
        {
            // do attack
            if (attackProduct)
            {
                AttackAction();
            }
            attackCount = 0;
        }
        attackCount += Time.deltaTime;
    }

    protected virtual void AttackAction()
    {
        Instantiate(attackProduct, muzzle.position, Quaternion.Euler(saveWorldRotation));
        isFire = true;
        fire.gameObject.SetActive(true);
        if (attackAudio)
        {
            audioMgr.instance.PlaySound(attackAudio);
        }
    }

    public override void StopAttack()
    {
        base.StopAttack();
        attackCount = attackCd;
    }
}
