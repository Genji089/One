using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControl : MonoBehaviour
{
    #region Inspector
    [Header("Attributes")]
    public float moveSpeed;
    public Transform role;
    public Transform weapon;
    public Transform summonMgr; //暂时放在人物这。后面看游戏设计要怎么召唤出召唤物。

    #endregion
    private PlayerState state;
    private WeaponState weaponState;
    private int runDir = 0; //[w, s, a, d]
    private Vector3 NextPosition;
    private Animator animator;
    private Animator weaponAnimator;
    private weaponBase weaponModel;

    // 装备
    private List<Transform> weaponList = new List<Transform>();
    private int weaponIndex = -1;

    // 暂时
    private summonMgr summonMgrModel;

    // Start is called before the first frame update
    void Start()
    {
        animator = role.GetComponent<Animator>();

        summonMgrModel = summonMgr.GetComponent<summonMgr>();
        GenerateWeapon("GreenDevil");
        GenerateWeapon("Changmao");
        weaponIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        ResetData();
        InputHandle();

        if (state == PlayerState.idle)
        {

        }
        else if (state == PlayerState.run)
        {
            Run();
        }

        if (weapon)
        {
            if (weaponState == WeaponState.idle)
            {
                weaponModel.StopAttack();
            }
            else if (weaponState == WeaponState.attack)
            {
                weaponModel.Attack();
            }
        }

        role.position = NextPosition;
    }

    private void FixedUpdate()
    {
        
    }

    void InputHandle()
    {
        //鼠标
        ChangeAngle(Input.mousePosition);

        if (Input.GetMouseButton(0)) //是不是考虑将输入抽出来单独处理。用一对多。现在是由人物分发输入信息。
        {
            SwitchWeaponState(WeaponState.attack);
        }

        if (Input.GetMouseButtonUp(0))
        {
            SwitchWeaponState(WeaponState.idle);
        }

        //键盘
        if (Input.GetKey(KeyCode.W))
        {
            runDir |= 1 << 3;
        }
        if (Input.GetKey(KeyCode.S))
        {
            runDir |= 1 << 2;
        }
        if (Input.GetKey(KeyCode.A))
        {
            runDir |= 1 << 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            runDir |= 1;
        }

        if (runDir != 0)
        {
            SwitchState(PlayerState.run);
        }
        else
        {
            SwitchState(PlayerState.idle);
        }

        //test
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchWeapon(false);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            SwitchWeapon(true);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            summonMgrModel.InstantiateSummon("OneEye");
        }
    }

    void SwitchState(PlayerState newState)
    {   
        if (state == newState) 
        {
            return;
        }

        state = newState;
        if (newState == PlayerState.idle)
        {
            animator.SetBool("bRun", false);
        }
        else if (newState == PlayerState.run)
        {
            animator.SetBool("bRun", true);
        }
    }

    void SwitchWeaponState(WeaponState newState)
    {
        if (!weapon)
        {
            return;
        }

        if (weaponState == newState)
        {
            return;
        }

        weaponState = newState;
        if (newState == WeaponState.idle)
        {
            weaponAnimator.SetBool("bAttack", false);
        }
        else if (newState == WeaponState.attack)
        {
            weaponAnimator.SetBool("bAttack", true);
        }
    }

    void Run()
    {
        var moveVector = Vector3.zero;
        var moveDistance = Time.deltaTime * moveSpeed;
        if ((runDir & 1 << 3) != 0) //w
        {
            moveVector.y += moveDistance;
        }
        if ((runDir & 1 << 2) != 0) //s
        {
            moveVector.y -= moveDistance;
        }
        if ((runDir & 1 << 1) != 0) //a
        {
            moveVector.x -= moveDistance;
        }
        if ((runDir & 1) != 0) //d
        {
            moveVector.x += moveDistance;
        }

        if (moveVector.x != 0 && moveVector.y != 0)
        {
            moveVector.x /= Mathf.Sqrt(2);
            moveVector.y /= Mathf.Sqrt(2);
        }

        NextPosition += moveVector;
    }

    void SetDiretion(int dir)
    {
        float ReverseEulerAngleY = dir == 1 ? 0 : 180;
        role.localEulerAngles = new Vector3(role.localEulerAngles.x, ReverseEulerAngleY, role.localEulerAngles.z);
    }

    void ChangeAngle(Vector3 cursorPos)
    {
        if (!weapon)
        {
            return;
        }

        var objScreenPos = Camera.main.WorldToScreenPoint(weapon.position);
        var angleVector = cursorPos - objScreenPos;
        var radian = Mathf.Atan2(angleVector.y, angleVector.x) * Mathf.Rad2Deg;
        var localRadian = radian;
        if (radian <= 90 && radian >= -90)
        {
            SetDiretion(1);
        }
        else
        {
            SetDiretion(0);
            localRadian = 180 - radian;
        }

        weapon.localEulerAngles = new Vector3(weapon.localEulerAngles.x, weapon.localEulerAngles.y, localRadian);
        weaponModel.saveWorldRotation = new Vector3(weapon.localEulerAngles.x, weapon.localEulerAngles.y, radian);
    }

    void ResetData()
    {
        runDir = 0;
        NextPosition = role.position;
    }

    void SwitchWeapon(bool front)
    {
        if (front)
        {
            weaponIndex = (weaponIndex + 1) % weaponList.Count;
        }
        else
        {
            weaponIndex = (weaponIndex + weaponList.Count - 1) % weaponList.Count;
        }
        if (weapon)   //上一把武器隐藏
        {
            weapon.gameObject.SetActive(false);
        }
        weapon = weaponList[weaponIndex];
        weapon.gameObject.SetActive(true);
        weaponModel = weapon.GetComponent<weaponBase>();
        weaponAnimator = weapon.GetComponent<Animator>();
    }

    void GenerateWeapon(string name)
    {
        Object oWeapon = Resources.Load("Prefabs/Weapons/" + name);
        GameObject oWeaponGameObj = Instantiate(oWeapon) as GameObject;
        Transform oWeaponTransform = oWeaponGameObj.transform;
        oWeaponTransform.parent = role;
        oWeaponTransform.localPosition = new Vector3(0, -0.04f, 0);
        oWeaponTransform.localScale = new Vector3(1, 1, 0);
        oWeaponGameObj.SetActive(false);
        weaponList.Add(oWeaponTransform);
        animator.SetBool("bEquip", true);
    }

    public enum PlayerState
    {
        idle,
        run,
    }

    public enum WeaponState
    {
        idle,
        attack,
    }
}


public class AnimationClipOverrides:List<KeyValuePair<AnimationClip, AnimationClip>>
{
    public AnimationClipOverrides(int capacity) : base(capacity) { }
    public AnimationClip this[string name]
    {
        get
        {
            return Find(x => x.Key.name.Equals(name)).Value;
        }
        set
        {
            int index = this.FindIndex(x => x.Key.name.Equals(name));
            if (index != -1)
            {
                this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
            }
            else
            {
                Debug.Log("AnimationClipOverrides set error");
            }
        }
    }
}