using UnityEngine;

public class Animal : MonoBehaviour
{

    private int arms;
    private int legs;
    private string species;
   
    public Animal()
    {
        Debug.Log("Animal Created");
    }

    public Animal(int newArms, int newLegs, string newSpecies)
    {
        arms = newArms;
        legs = newLegs;
        species = newSpecies;
    }
    
}
