using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 50;
    private bool isDead = false;
    public float knockbackForce = 5f; // ��������
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        
    }


    public void TakeDamage(int damage, Vector2 attackDirection)
    {
        if (isDead) return;

        health -= damage;
        Debug.Log(name + "�ܵ��˺���ʣ��Ѫ����" + health);

        if (health <= 0)
        {
            Die();
        }
        if (rb != null)
        {
            // ���ݹ�������ʩ����
            rb.AddForce(attackDirection * knockbackForce, ForceMode2D.Impulse);
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log(name + "����");
        Destroy(gameObject);
    }
}
