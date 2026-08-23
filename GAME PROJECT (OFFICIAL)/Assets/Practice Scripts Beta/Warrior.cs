using UnityEngine;

public class Warrior : Character
{
    public string Weapon;

    public Warrior(string name, string weapon) : base(name)
    {
        Weapon = weapon;
    }
}
