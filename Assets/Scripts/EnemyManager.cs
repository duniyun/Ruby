using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 敌人AI
/// </summary>
public class EnemyManager : MonoBehaviour
{
    /// <summary>
    /// 移动速度
    /// </summary>
    public float moveSpeed = 1;

    //轴向控制
    public bool vertical;

    //方向控制
    private int direction = 1;

    //方向改变时间隔，常量
    public float changeTime = 3;

    //计时器
    private float timer;

    //定义动画组件
    private Animator animator;

    //机器人修复好坏条件
    public bool broken;

    /// <summary>
    /// 2d刚体
    /// </summary>
    private Rigidbody2D rigidbody2d;
    private PlayerManager player;


    void Start()
    {
        //获取刚体组件
        rigidbody2d = GetComponent<Rigidbody2D>();
        //获取动画组件
        animator = GetComponent<Animator>();
        //初始化播放放条件
        PlayMoveAninmation();
        //初始化机器人好坏
        broken = true;
        player = GameObject.FindObjectOfType<PlayerManager>();
    }

    void Update()
    {
        //修复机器人跳出移动
        if (!broken)
        {
            return;
        }

        //定时改变移动方向
        timer = timer - Time.deltaTime;
        if (timer <= 0)
        {
            direction = -direction;
            PlayMoveAninmation();
            timer = changeTime;
        }

        //改变移动轴向
        Vector2 position = rigidbody2d.position;
        if (vertical)
        {
            position.y = position.y + moveSpeed * Time.deltaTime * direction;
        }
        else
        {
            position.x = position.x + moveSpeed * Time.deltaTime * direction;
        }
        rigidbody2d.position = position;
    }
    //播放组件方法
    private void PlayMoveAninmation()
    {
        if (vertical)
        {
            animator.SetFloat("ForwardX", 0);
            animator.SetFloat("ForwardY", direction);
        }
        else
        {
            animator.SetFloat("ForwardX", direction);
            animator.SetFloat("ForwardY", 0);
        }
    }
    //修复机器人
    public void Fix()
    {
        if(broken)
        {
            player.numEnemy++;
        }
        broken = false;
        rigidbody2d.simulated = false;
        animator.SetTrigger("Fixed"); 
    }
    private void OnTriggerStay2D(Collider2D collider)
    {

        PlayerManager player = collider.GetComponent<PlayerManager>();
        if (player != null)
        {
            player.ChangHealth(-1);
        }
    }
}
