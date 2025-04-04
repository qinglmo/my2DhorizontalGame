using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator animator;
    private int direction = 1;

    public float attackRange = 1f; // ������Χ
    public LayerMask enemyLayer;   // �������ڵĲ㼶
    public int damage = 10;        // �����˺�ֵ
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

        if (Input.GetKeyDown(KeyCode.J)) // ����J���ǹ�����
        {
            AudioManager.Instance.Play("0");
            animator.SetTrigger("AttackTrigger");
            // ������������һ�����η�Χ��
            Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(
                transform.position + transform.right * attackRange, // �������ĵ�
                new Vector2(attackRange, 1f), // ������Χ��С
                0, // ��ת�Ƕ�
                enemyLayer
            );

            // �����������еĵ���
            foreach (Collider2D enemy in hitEnemies)
            {
                
                Enemy enemyComponent = enemy.GetComponent<Enemy>();
                if (enemyComponent == null)
                {
                    Debug.LogWarning($"���� {enemy.name} ȱ�� Enemy ���");
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
