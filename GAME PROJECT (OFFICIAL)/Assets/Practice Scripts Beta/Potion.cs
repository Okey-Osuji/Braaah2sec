using UnityEngine;

public class Potion : MonoBehaviour
{
    public string potionName;
    public int potency;
    public float volume;

    public void Drink()
    {
         Debug.Log(potionName + "potion with " + potency + " potency with " + volume + " amount is being drank");
    }
}
