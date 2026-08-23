using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Orc : Enemy
{
    public void OrcAttack()
    {
        Debug.Log("The Orc is attacking. Damage dealth :" + damage);
    }
}
