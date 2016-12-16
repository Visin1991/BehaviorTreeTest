using UnityEngine;
using System.Collections;

public class Destructable : MonoBehaviour
{
    public float maxHp = 100;
    public float hp = 2;
    public GameObject replacement;

    public void takeDamage(float damage)
    {
        hp = hp - damage;
        if (hp <= 0)
        {
            Instantiate(replacement, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
