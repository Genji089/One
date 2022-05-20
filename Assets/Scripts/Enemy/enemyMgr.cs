using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMgr : MonoBehaviour
{
    public Transform player;

    [Header("Enemy Generate Ctrl")]
    public float maxAmount;
    public float generateCD;
    public float generateRadius;

    [System.Serializable]
    public class EnemyMap
    {
        public string enemyName;
        public Transform enemy;
    }
    public EnemyMap[] enemyMap;

    private Dictionary<string, List<enemyBase>> enemyPoolDict = new Dictionary<string, List<enemyBase>>();
    private Dictionary<string, Transform> enemyObjectDict = new Dictionary<string, Transform>();
    private int enemyAmount;
    private float generateTimeCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach (EnemyMap e in enemyMap)
        {
            enemyObjectDict[e.enemyName] = e.enemy;
        }
    }

    // Update is called once per frame
    void Update()
    {
        var playerPos = player.position;
        //生成逻辑
        if (enemyAmount < maxAmount)
        {
            if (generateTimeCount > generateCD)
            {
                float x = Random.Range(playerPos.x - generateRadius, playerPos.x + generateRadius);
                float temp = Mathf.Sqrt(Mathf.Pow(generateRadius, 2) - Mathf.Pow(x - playerPos.x, 2));
                temp *= Random.Range(0, 1f) <= 0.5f ? -1 : 1;
                float y = temp + playerPos.y;

                //float x = Random.Range(playerPos.x - generateRadius / 2, playerPos.x + generateRadius / 2);
                //float y = x * Mathf.Sin(Random.Range(0, 360) * Mathf.Deg2Rad);
                AddEnemy("slime", new Vector3(x, y, 0), Vector3.zero);
                enemyAmount++;
                generateTimeCount = 0;
            }
            else
            {
                generateTimeCount += Time.deltaTime;
            }
        }

        //遍历运行enemy逻辑
        foreach (List<enemyBase> enemyList in enemyPoolDict.Values)
        {
            foreach (enemyBase enemy in enemyList)
            {
                if (enemy.IsAlive())
                {
                    enemy.UpdateLogic(playerPos);
                }
            }
        }
    }

    void AddEnemy(string enemyName, Vector3 position, Vector3 rotation)
    {
        enemyBase enemy;
        if (!enemyPoolDict.ContainsKey(enemyName))
        {
            enemyPoolDict[enemyName] = new List<enemyBase>();
            enemy = CreateEnemy(enemyName);
            enemyPoolDict[enemyName].Add(enemy);
        }
        else
        {
            enemy = CheckFreeEnemy(enemyName);
            if (!enemy)
            {
                enemy = CreateEnemy(enemyName);
                enemyPoolDict[enemyName].Add(enemy);
            }
        }

        enemy.Init(position, rotation);
    }

    enemyBase CheckFreeEnemy(string enemyName)
    {
        foreach (enemyBase enemy in enemyPoolDict[enemyName])
        {
            if (!enemy.IsAlive())
            {
                return enemy;
            }
        }
        return null;
    }

    enemyBase CreateEnemy(string enemyName)
    {
        Transform enemyObject = Instantiate(enemyObjectDict[enemyName]);
        enemyObject.parent = transform;
        enemyBase enemy = enemyObject.GetComponent<enemyBase>();
        enemy.OnEnemyDie = OnEnemyDie;
        return enemy;
    }

    void OnEnemyDie(Transform enemy)
    {
        enemyAmount--;
        //StartCoroutine(SlowDownFrame());
    }

    IEnumerator SlowDownFrame()
    {
        Time.timeScale = 0.7f;
        yield return new WaitForSeconds(0.1f);
        Time.timeScale = 1;
    }
}
