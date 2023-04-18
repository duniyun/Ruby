using UnityEngine;

/// <summary>
/// 受伤检测
/// </summary>
public class Damageable : MonoBehaviour
{
    //触发检测
    private void OnTriggerStay2D(Collider2D collider)
    {
       
        PlayerManager player = collider.GetComponent<PlayerManager>();
        if (player != null)
        {
            player.ChangHealth(-1);
        }
    }

}
