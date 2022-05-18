using UnityEngine;
using System.Collections;


public class enemyBase : MonoBehaviour
{
    #region Inspector
    [Header("Default Attributes")]
    public float HP;
    public float ATK;
    public float moveSpeed;
    #endregion

    public delegate void EnemyDieDelegate(Transform enemy);
    public EnemyDieDelegate OnEnemyDie;

    private bool bAlive;
    private float curHP;
    private float curATK;
    private float curMoveSpeed;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }   

    public virtual void UpdateLogic(Vector3 playerPos)
    {
        transform.position = Vector3.MoveTowards(transform.position, playerPos, curMoveSpeed * Time.deltaTime);
    }

    public void Init(Vector3 position, Vector3 rotation)
    {
        bAlive = true;
        gameObject.SetActive(true);
        curHP = HP;
        curATK = ATK;
        curMoveSpeed = moveSpeed;
        transform.position = position;
        transform.localEulerAngles = rotation;
    }

    public bool IsAlive()
    {
        return bAlive;
    }

    public void SetAlive(bool bAlive)
    {
        this.bAlive = bAlive;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
            OnBeAttacked();
        }
    }

    void OnBeAttacked()
    {
        curHP--;
        if (curHP <= 0)
        {
            bAlive = false;
            gameObject.SetActive(false);
            OnEnemyDie?.Invoke(transform);
        }
    }
}