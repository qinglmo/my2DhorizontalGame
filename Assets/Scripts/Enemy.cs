using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 50;
    private bool isDead = false;
    public float knockbackForce = 5f; // 击退力度
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
        Debug.Log(name + "受到伤害，剩余血量：" + health);

        if (health <= 0)
        {
            Die();
        }
        if (rb != null)
        {
            // 根据攻击方向施加力
            rb.AddForce(attackDirection * knockbackForce, ForceMode2D.Impulse);
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log(name + "死亡");
        Destroy(gameObject);
    }
}
