using UnityEngine;
using System.Collections;


public class weaponBase : MonoBehaviour
{
    #region Inspector
    public Vector3 saveWorldRotation;
    public float attackCd;
    public Transform attackProduct;

    #endregion

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void Attack()
    {

    }

    public virtual void StopAttack()
    {

    }
}