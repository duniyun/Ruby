using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// ��ҹ�����
/// </summary>
public class PlayerManager : MonoBehaviour
{
    /// <summary>
    /// �ƶ��ٶ�
    /// </summary>
    public int moveSpeed = 50;
    /// <summary>
    /// �������ֵ
    /// </summary>
    public int maxHealth = 5;
    /// <summary>
    /// ��ǰ����ֵ
    /// </summary>
    private int currentHealth;
    /// <summary>
    /// ������������
    /// </summary>
    public int Health { get { return currentHealth; } }
    /// <summary>
    /// �ƶ�����
    /// </summary>
    private Vector2 moveDirection;
    /// <summary>
    /// ���˵ȴ�ʱ��
    /// </summary>
    public float timeInvincible = 2;
    /// <summary>
    /// �Ƿ����ʱ
    /// </summary>
    public bool isInvincible;
    /// <summary>
    /// ��ʱ��ʱ��
    /// </summary>
    public float invincibleTimer;
    /// <summary>
    /// ��������Ԥ����
    /// </summary>
    public GameObject attackPrefab;
    /// <summary>
    /// ������
    /// </summary>
    private Animator animator;
    /// <summary>
    /// 2d��ɫ������
    /// </summary>
    private Rigidbody2D rigidbody2d;

    /// <summary>
    /// �Ի�ϵͳ
    /// </summary>
    private DialogueSystem dlg;
    /// <summary>
    /// Ѫ��ֵ
    /// </summary>
    public Image spLife;
    /// <summary>
    /// �Ի���
    /// </summary>
    public GameObject Dlalog;
    /// <summary>
    /// �Ƿ���Թ���
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
                    dlg.Dialoge(1, "�ǳ���л����������޸������ˣ������ָֻ�ƽ���ˣ�");
                    Invoke("fun5", 2);
                    return;
                }
                if(IsAttack && numEnemy<4)
                {
                    dlg.Open();
                    dlg.Dialoge(1, "ֻҪ�������л����˾����޸���!���ո�����������GO,GO,GO������ж��ɣ�");
                    Invoke("fun5", 2);
                    return;
                }
                dlg.Open();
                dlg.Dialoge(1, "Ruby�����������һЩ���⣬���ܰ�����𣿣�");
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
        dlg.Dialoge(0, "Boos������ô�������ģ�");
    }
    private void fun2()
    {
        dlg.Dialoge(1, "�Ҹ��㹥�����������������޸�4���Ļ�������");
    }
    private void fun3()
    {
        dlg.Dialoge(0, "�õģ�û���⣡");
    }
    private void fun4()
    {
        dlg.Dialoge(1, "�ǳ���л��");
        IsAttack = true;
    }
    private void fun5()
    {
        dlg.Close();
    }
    /// <summary>
    /// ��ʼ������
    /// </summary>
    public void Init()
    {
        currentHealth = maxHealth;
        IsAttack=false;
    }
    /// <summary>
    /// �ж��Ƿ������Ϸ
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
    /// ��ɫ�ƶ�
    /// </summary>
    private void RoleMove()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        //Mathf.Approximately�������
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
    /// ����ʱ
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
    /// ���˷���
    /// </summary>
    public void ChangHealth(int amount)
    {
        //�����˺�����
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
    /// ��������
    /// </summary>
    private void Attack()
    {
        GameObject prefab = Instantiate(attackPrefab, transform.position, Quaternion.identity);
        Projectle projectle = prefab.GetComponent<Projectle>();
        projectle.Attack(moveDirection, 300);
        animator.SetTrigger("Attack");
    }

}

