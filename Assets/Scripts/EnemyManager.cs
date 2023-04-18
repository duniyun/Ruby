using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ����AI
/// </summary>
public class EnemyManager : MonoBehaviour
{
    /// <summary>
    /// �ƶ��ٶ�
    /// </summary>
    public float moveSpeed = 1;

    //�������
    public bool vertical;

    //�������
    private int direction = 1;

    //����ı�ʱ���������
    public float changeTime = 3;

    //��ʱ��
    private float timer;

    //���嶯�����
    private Animator animator;

    //�������޸��û�����
    public bool broken;

    /// <summary>
    /// 2d����
    /// </summary>
    private Rigidbody2D rigidbody2d;
    private PlayerManager player;


    void Start()
    {
        //��ȡ�������
        rigidbody2d = GetComponent<Rigidbody2D>();
        //��ȡ�������
        animator = GetComponent<Animator>();
        //��ʼ�����ŷ�����
        PlayMoveAninmation();
        //��ʼ�������˺û�
        broken = true;
        player = GameObject.FindObjectOfType<PlayerManager>();
    }

    void Update()
    {
        //�޸������������ƶ�
        if (!broken)
        {
            return;
        }

        //��ʱ�ı��ƶ�����
        timer = timer - Time.deltaTime;
        if (timer <= 0)
        {
            direction = -direction;
            PlayMoveAninmation();
            timer = changeTime;
        }

        //�ı��ƶ�����
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
    //�����������
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
    //�޸�������
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
