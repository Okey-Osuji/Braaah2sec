using UnityEngine;

public class Zombie : MonoBehaviour
{
    private string named;
    private int arms, legs;

    public Zombie(string newName, int newArms, int newLegs)
    {
       named = newName;
       arms = newArms;
       legs = newLegs;
    }

    public void setNumLegs(int newLegs)
    {
        legs = newLegs;
    }

    public int getArms()
    {
        return arms;
    }

    public int getLegs()
    {
        return legs;
    }
}
