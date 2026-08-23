using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealthPotion : Potion
{

    public void RestoreHealth()
    {
    Debug.Log("Health potion drank! Health Restored!");
    }


    public void Start()
    {
          Drink();
          RestoreHealth();
    }
}
