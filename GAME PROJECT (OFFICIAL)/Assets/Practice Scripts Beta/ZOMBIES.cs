using UnityEngine;

public class ZOMBIES : MonoBehaviour
{
    
    void Start()
    {
        Zombie myZombie = new Zombie("Chuck",1,2);

        myZombie.setNumLegs(0);

        Debug.Log(myZombie.getArms());
        Debug.Log(myZombie.getLegs());

    }

    



    void Update()
    {
        
    }
}
