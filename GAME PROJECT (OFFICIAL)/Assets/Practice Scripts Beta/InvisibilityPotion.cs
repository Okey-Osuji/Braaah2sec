using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InvisibilityPotion : Potion
{
    public void BecomeInvisible()
    {
         Debug.Log("Invisibility potion drank! Player is Invisible!");
    }

    public void Start()
    {
          Drink();
          BecomeInvisible();
    }
}
