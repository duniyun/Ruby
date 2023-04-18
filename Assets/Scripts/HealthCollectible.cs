using UnityEngine;

/// <summary>
/// Ôö¼ÓÉúÃü
/// </summary>
public class HealthCollectible : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log(collider.name);
        PlayerManager player = collider.GetComponent<PlayerManager>();
        if (player != null)
        {
            if (player.Health < player.maxHealth)
            {
                player.ChangHealth(1);
                Invoke("Fun1", 3);
                gameObject.SetActive(false);
            }
        }
    }
    private void Fun1()
    {
        gameObject.SetActive(true);
    }
}
