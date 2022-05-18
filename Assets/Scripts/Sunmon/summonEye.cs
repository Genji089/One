using UnityEngine;
using System.Collections;


public class summonEye : summonAttackType
{
    public Transform attackProduct;
    public Transform muzzle;
    public float attackCd;
    public float maxDistanceToPlayer;

    private float attackCdCount = 0;

    protected override void UpdateBeforeStateLogic()
    {
        if (Vector3.Distance(transform.position, playerPosition) > maxDistanceToPlayer)
        {
            SwitchState(SummonState.follow);
            return;
        }
        base.UpdateBeforeStateLogic();
    }

    protected override void Attack()
    {
        base.Attack();
        Transform target = enemyDiscoveredList[0];

        // 旋转，面向目标
        var angleVector = target.position - transform.position;
        var rotateZ = Mathf.Atan2(angleVector.y, angleVector.x) * Mathf.Rad2Deg;
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, rotateZ);

        if (attackCdCount <= 0)
        {
            // attack
            Instantiate(attackProduct, muzzle.position, transform.rotation);
            attackCdCount = attackCd;
        }
        else
        {
            attackCdCount -= Time.deltaTime;
        }
    }
}