using UnityEngine;

public class Cat : MonoBehaviour
{
    private string name;
    private int age;

    public void Meow()
    {
         Debug.Log(name + "The cat is meowing");
    }
}
