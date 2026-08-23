using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Goblin : Enemy
{
    public void GoblinHealth()
    {
           Debug.Log("The Goblin has taken damage! Health remaining :" + health);
    }
}
