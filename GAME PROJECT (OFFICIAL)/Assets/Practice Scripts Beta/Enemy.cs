using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public int damage;

    public void TakeDamage(int amount)
    {
          health -= amount;
    }
}
