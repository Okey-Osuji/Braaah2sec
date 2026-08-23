using System.Collections.Generic;
using System;
using UnityEngine;

public class Action : MonoBehaviour
{
    
   
    void Start()
    {
        Sword sword = gameObject.AddComponent<Sword>();
        Arrow arrow = gameObject.AddComponent<Arrow>();

        List<Weapon> weapons = new List<Weapon>{ sword, arrow };


        foreach (Weapon weapon in weapons) 
        {
            weapon.Attack();
        }
    }



    

}
