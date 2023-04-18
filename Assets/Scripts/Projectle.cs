using UnityEngine;

/// <summary>
/// ¹¥»÷ÎäÆ÷
/// </summary>
public class Projectle : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }
    public void Attack(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }

    private void Update()
    {
        if (transform.position.magnitude > 30)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        EnemyManager enemy = collider.gameObject.GetComponent<EnemyManager>();
        if (enemy != null)
        {
            enemy.Fix();
        }
        Destroy(gameObject);
    }
}
