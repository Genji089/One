using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class summonAttackType : summonBase
{
    protected List<Transform> enemyDiscoveredList = new List<Transform>();

    protected override void Start()
    {
        base.Start();
    }

    protected override void UpdateBeforeStateLogic()
    {
        CheckEnemyLoop();
    }

    protected override void Action()
    {
        base.Action();
        Attack();
    }

    protected virtual void Attack()
    {

    }

    private void CheckEnemyLoop()
    {
        if (enemyDiscoveredList.Count != 0)
        {
            SwitchState(SummonState.action);
        }
        else
        {
            SwitchState(SummonState.follow);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemyDiscoveredList.Add(collision.transform);
            collision.transform.GetComponent<enemyBase>().OnEnemyDie += OnDiscoveredEnemyDie;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            bool success = enemyDiscoveredList.Remove(collision.transform);
            collision.transform.GetComponent<enemyBase>().OnEnemyDie -= OnDiscoveredEnemyDie;
            if (!success)
            {
                Debug.Log("Remove a transform not in enemyDiscoveredList.");
            }
        }
    }

    private void OnDiscoveredEnemyDie(Transform enemy)
    {
        bool success = enemyDiscoveredList.Remove(enemy);
        if (!success)
        {
            Debug.Log("Remove a transform not in enemyDiscoveredList when enemy die.");
        }
    }
}