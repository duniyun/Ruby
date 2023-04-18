using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 玩家管理器
/// </summary>
public class PlayerManager : MonoBehaviour
{
    /// <summary>
    /// 移动速度
    /// </summary>
    public int moveSpeed = 50;
    /// <summary>
    /// 最大生命值
    /// </summary>
    public int maxHealth = 5;
    /// <summary>
    /// 当前生命值
    /// </summary>
    private int currentHealth;
    /// <summary>
    /// 生命公开属性
    /// </summary>
    public int Health { get { return currentHealth; } }
    /// <summary>
    /// 移动方向
    /// </summary>
    private Vector2 moveDirection;
    /// <summary>
    /// 受伤等待时间
    /// </summary>
    public float timeInvincible = 2;
    /// <summary>
    /// 是否开起计时
    /// </summary>
    public bool isInvincible;
    /// <summary>
    /// 计时器时间
    /// </summary>
    public float invincibleTimer;
    /// <summary>
    /// 攻击武器预制体
    /// </summary>
    public GameObject attackPrefab;
    /// <summary>
    /// 动画器
    /// </summary>
    private Animator animator;
    /// <summary>
    /// 2d角色控制器
    /// </summary>
    private Rigidbody2D rigidbody2d;

    /// <summary>
    /// 对话系统
    /// </summary>
    private DialogueSystem dlg;
    /// <summary>
    /// 血量值
    /// </summary>
    public Image spLife;
    /// <summary>
    /// 对话框
    /// </summary>
    public GameObject Dlalog;
    /// <summary>
    /// 是否可以攻击
    /// </summary>
    public bool IsAttack=false;
    public int numEnemy = 0;
    public Image imgOver;

    void Start()
    { 
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        dlg = Dlalog.GetComponent<DialogueSystem>();
        Init();
    }

    void Update()
    {
        IsOverGame();
        RoleMove();
        CountDown();

        if (Input.GetKeyDown(KeyCode.Space)&&IsAttack)
        {
            Attack();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            RaycastHit2D hit2D;
            var ray = Physics2D.Raycast(rigidbody2d.position+Vector2.up*1f,moveDirection,1.5f);
            if (ray.collider == null) return;
            Debug.Log(ray.collider.name);
            if (ray.collider.name=="Boss")
            {
                if (numEnemy>=4)
                {
                    dlg.Open();
                    dlg.Dialoge(1, "非常感谢，你帮助我修复机器人！哎，又恢复平静了！");
                    Invoke("fun5", 2);
                    return;
                }
                if(IsAttack && numEnemy<4)
                {
                    dlg.Open();
                    dlg.Dialoge(1, "只要攻击命中机器人就能修复了!（空格发射武器），GO,GO,GO，快快行动吧！");
                    Invoke("fun5", 2);
                    return;
                }
                dlg.Open();
                dlg.Dialoge(1, "Ruby，我最经遇到了一些问题，你能帮帮我吗？？");
                Invoke("fun1", 2);
                Invoke("fun2", 4);
                Invoke("fun3", 6);
                Invoke("fun4", 8);
                Invoke("fun5", 10);
            }
        }
    }

    private void fun1()
    {
        dlg.Dialoge(0, "Boos，我怎么帮助你哪？");
    }
    private void fun2()
    {
        dlg.Dialoge(1, "我给你攻击武器零件，你帮我修复4个的机器人吗？");
    }
    private void fun3()
    {
        dlg.Dialoge(0, "好的，没问题！");
    }
    private void fun4()
    {
        dlg.Dialoge(1, "非常感谢！");
        IsAttack = true;
    }
    private void fun5()
    {
        dlg.Close();
    }
    /// <summary>
    /// 初始化数据
    /// </summary>
    public void Init()
    {
        currentHealth = maxHealth;
        IsAttack=false;
    }
    /// <summary>
    /// 判断是否结束游戏
    /// </summary>
    private void IsOverGame()
    {
        if (currentHealth <= 0)
        {
            imgOver.gameObject.SetActive(true);
            Invoke("GameOver", 1.5f);
        }
    }
    private void GameOver()
    {
        SceneManager.LoadScene(0);
    }
    /// <summary>
    /// 角色移动
    /// </summary>
    private void RoleMove()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        //Mathf.Approximately近似相等
        if (!Mathf.Approximately(h, 0) || !Mathf.Approximately(v, 0))
        {
            moveDirection.Set(h, v);
            moveDirection.Normalize();
            //Debug.Log(moveDirection.x+"/"+ moveDirection.y);
        }
        animator.SetFloat("Look X", moveDirection.x);
        animator.SetFloat("Look Y", moveDirection.y);
        animator.SetFloat("Speed", new Vector2(v, h).magnitude);
        Vector2 position = transform.position;
        rigidbody2d.MovePosition(position + moveSpeed * new Vector2(h, v) * Time.deltaTime);
    }

    /// <summary>
    /// 倒计时
    /// </summary>
    private void CountDown()
    {
        if (isInvincible)
        {
            invincibleTimer = invincibleTimer - Time.deltaTime;
            if (invincibleTimer <= 0)
            {
                isInvincible = false;
            }
        }
    }
    /// <summary>
    /// 受伤方法
    /// </summary>
    public void ChangHealth(int amount)
    {
        //持续伤害区域
        if (amount < 0)
        {
            if (isInvincible)
            {
                return;
            }
            isInvincible = true;
            invincibleTimer = timeInvincible;
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        spLife.fillAmount = (float)currentHealth/ (float)maxHealth;
        IsOverGame();
        Debug.Log(currentHealth + "/" + maxHealth);
    }
    /// <summary>
    /// 攻击方法
    /// </summary>
    private void Attack()
    {
        GameObject prefab = Instantiate(attackPrefab, transform.position, Quaternion.identity);
        Projectle projectle = prefab.GetComponent<Projectle>();
        projectle.Attack(moveDirection, 300);
        animator.SetTrigger("Attack");
    }

}

