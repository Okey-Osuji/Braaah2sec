using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string Name;
    public int Damage;

    public virtual void Attack()
    {
        Debug.Log("The Weapon is attacking");
    }
}