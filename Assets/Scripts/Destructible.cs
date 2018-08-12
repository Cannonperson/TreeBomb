using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour {

    public int hp;

    public virtual void Damage(int dmg)
    {
        hp -= dmg;
    }
}
