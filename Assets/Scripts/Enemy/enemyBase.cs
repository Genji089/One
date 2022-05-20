using UnityEngine;
using System.Collections;


public class enemyBase : MonoBehaviour
{
    #region Inspector
    [Header("Audio")]
    public AudioClip dieAudio;

    [Header("Default Attributes")]
    public float HP;
    public float ATK;
    public float moveSpeed;
    #endregion

    public delegate void EnemyDieDelegate(Transform enemy);
    public EnemyDieDelegate OnEnemyDie;

    protected enemyState state = enemyState.chase;
    private bool bAlive;
    private float curHP;
    private float curATK;
    private float curMoveSpeed;
    private Animator animator;
    private BoxCollider2D boxCollider2D;

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
        if (state == enemyState.chase)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerPos, curMoveSpeed * Time.deltaTime);
        }
        else if (state == enemyState.beAttacked)
        {

        }
        else if (state == enemyState.dying)
        {

        }
    }

    public void Init(Vector3 position, Vector3 rotation)
    {
        state = enemyState.chase;
        bAlive = true;
        gameObject.SetActive(true);
        curHP = HP;
        curATK = ATK;
        curMoveSpeed = moveSpeed;
        transform.position = position;
        transform.localEulerAngles = rotation;

        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator.SetBool("isDie", false);
        boxCollider2D.enabled = true;
    }

    public bool IsAlive()
    {
        return bAlive;
    }

    private void SwitchState(enemyState newState)
    {
        if (state == newState)
        {
            return;
        }

        state = newState;

        if (newState == enemyState.dying)
        {
            OnEnterStateDying();
        }
    }

    void OnEnterStateDying()
    {
        animator.SetBool("isDie", true);
        boxCollider2D.enabled = false;
        audioMgr.instance.PlaySound(dieAudio);
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
            SwitchState(enemyState.dying);
        }
    }

    void OnDieAnimationEnd()
    {
        bAlive = false;
        gameObject.SetActive(false);
        OnEnemyDie?.Invoke(transform);
    }

    protected enum enemyState
    {
        chase,
        beAttacked,
        dying,
    }
}