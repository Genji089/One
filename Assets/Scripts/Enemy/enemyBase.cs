using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    private Vector3 playerPos;
    private string hitItemType;
    private ArrayList hitData;
    private Vector3 hitBackEndPos;

    protected List<string> threateningItems = new List<string>(new string[] {
        "Bullet",
    });

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

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

    public virtual void UpdateLogic(Vector3 playerPos)
    {
        this.playerPos = playerPos;

        if (state == enemyState.chase)
        {
            ChaseAction();
        }
        else if (state == enemyState.beAttacked)
        {
            BeAttackedAction();
        }
        else if (state == enemyState.dying)
        {

        }
    }

    public virtual void ChaseAction()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerPos, curMoveSpeed * Time.deltaTime);
    }

    public virtual void BeAttackedAction()
    {
        if (hitItemType == "shotgun")
        {
            transform.position = Vector3.MoveTowards(transform.position, hitBackEndPos, (float)hitData[2] * Time.deltaTime);
        }
        else
        {
            ChaseAction();
            SwitchState(enemyState.chase);
        }
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
        else if (newState == enemyState.beAttacked)
        {
            if (hitItemType == "shotgun")
            {
                float hitBackDistance = (float)hitData[1];
                hitBackEndPos = transform.position + (transform.position - playerPos).normalized * hitBackDistance;
                StartCoroutine(WaitHitBackEnd());
            }
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
        if (threateningItems.Contains(collision.tag))
        {
            string hitItemType = "";
            float damage = 1;
            if (collision.CompareTag("Bullet"))
            {
                hitItemType = collision.GetComponent<bulletBase>().bulletType;
                hitData = collision.GetComponent<bulletBase>().GetHitData();
                damage = (float)hitData[0];
            }
            OnBeAttacked(hitItemType, damage);
        }
    }

    void OnBeAttacked(string hitItemType, float damage)
    {
        curHP -= damage;
        if (curHP <= 0)
        {
            SwitchState(enemyState.dying);
        }
        else
        {
            this.hitItemType = hitItemType;
            SwitchState(enemyState.beAttacked);
        }

        //gameObject.GetComponent<Renderer>().material.SetFloat("_FillPhase", 1f);
    }

    void OnDieAnimationEnd()
    {
        bAlive = false;
        gameObject.SetActive(false);
        OnEnemyDie?.Invoke(transform);
    }

    IEnumerator WaitHitBackEnd()
    {
        yield return new WaitUntil(() =>
        {
            return transform.position == hitBackEndPos;
        });
        SwitchState(enemyState.chase);
    }

    protected enum enemyState
    {
        chase,
        beAttacked,
        dying,
    }
}