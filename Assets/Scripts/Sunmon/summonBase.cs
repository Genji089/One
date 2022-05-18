using UnityEngine;
using System.Collections;


public class summonBase : MonoBehaviour
{
    public float delayFollowTime;
    public float backFollowSpeed;
    public float showQueueLenth;

    private SummonState state = SummonState.follow;
    private Vector3 posOffset;
    private Queue posQueue = new Queue();
    private float delayFollowWaitTimeCount = 0;

    protected Vector3 playerPosition;

    public Vector3 PosOffset
    {
        get
        {
            return posOffset;
        }
        set
        {
            posOffset = value;
        }
    }

    // Use this for initialization
    protected virtual void Start()
    {
        
    }

    public void UpdateLogic(Vector3 position)
    {
        playerPosition = position;
        UpdateBeforeStateLogic();

        if (state == SummonState.idle)
        {

        }
        else if (state == SummonState.action)
        {
            Action();
        }
        else if (state == SummonState.follow)
        {
            Follow(playerPosition);
        }
    }

    protected virtual void UpdateBeforeStateLogic()
    {

    }

    protected virtual void Action()
    {

    }

    protected virtual void Follow(Vector3 playerPosition)
    {
        posQueue.Enqueue(new Vector3(playerPosition.x + posOffset.x, playerPosition.y + posOffset.y, playerPosition.z));
        showQueueLenth = posQueue.Count;

        if (delayFollowWaitTimeCount >= delayFollowTime)
        {
            transform.position = (Vector3)posQueue.Dequeue();
        }
        else
        {
            delayFollowWaitTimeCount += Time.deltaTime;
        }
    }

    protected void SwitchState(SummonState newState)
    {
        if (state == newState)
        {
            return;
        }

        state = newState;
        // 进入新状态事件
        if (newState == SummonState.idle)
        {
            
        }
        else if (newState == SummonState.action)
        {
            
        }
        else if (newState == SummonState.follow)
        {

        }

        // 退出状态事件
        if (state == SummonState.follow)
        {
            delayFollowWaitTimeCount = 0;
            posQueue.Clear();
        }
    }

    protected enum SummonState
    {
        idle,
        action,
        follow,
    }
}