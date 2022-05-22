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

    private bool isFire = false;
    private float fireDurationCount = 0;
    private bool isAttackReady = true;

    // Start is called before the first frame update
    void Start()
    {
        fire.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        isAttackReady = true;
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
        if (isAttackReady)
        {
            // do attack
            if (attackProduct)
            {
                AttackAction();
            }
            StartCoroutine(WaitAttackCD());
        }
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

    IEnumerator WaitAttackCD()
    {
        isAttackReady = false;
        yield return new WaitForSeconds(attackCd);
        isAttackReady = true;
    }
}
