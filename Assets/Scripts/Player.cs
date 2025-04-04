using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator animator;
    private int direction = 1;

    public float attackRange = 1f; // 攻击范围
    public LayerMask enemyLayer;   // 敌人所在的层级
    public int damage = 10;        // 基础伤害值
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("a"))
        {
            transform.Translate(-1 * Time.deltaTime, 0, 0, Space.World);
            direction = -1;
            animator.SetBool("Move Bool", true); // 
        }
        else if (Input.GetKey("d"))
        {
            transform.Translate(1 * Time.deltaTime, 0, 0, Space.World);
            direction = 1;
            animator.SetBool("Move Bool", true); // 
        }
        else
        {
            animator.SetBool("Move Bool", false); // 
        }
        Vector3 newScale = transform.localScale;
        newScale.x = Mathf.Abs(newScale.x) * Mathf.Sign(direction);
        transform.localScale = newScale;

        if (Input.GetKeyDown(KeyCode.J)) // 假设J键是攻击键
        {
            AudioManager.Instance.Play("0");
            animator.SetTrigger("AttackTrigger");
            // 创建攻击区域（一个矩形范围）
            Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(
                transform.position + transform.right * attackRange, // 攻击中心点
                new Vector2(attackRange, 1f), // 攻击范围大小
                0, // 旋转角度
                enemyLayer
            );

            // 遍历所有命中的敌人
            foreach (Collider2D enemy in hitEnemies)
            {
                
                Enemy enemyComponent = enemy.GetComponent<Enemy>();
                if (enemyComponent == null)
                {
                    Debug.LogWarning($"对象 {enemy.name} 缺少 Enemy 组件");
                    continue;
                }
                Vector2 attackDir = (enemy.transform.position - transform.position).normalized;
                enemy.GetComponent<Enemy>().TakeDamage(damage, attackDir);

            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            transform.position + transform.right * attackRange,
            new Vector2(attackRange, 1f)
        );
    }
}
