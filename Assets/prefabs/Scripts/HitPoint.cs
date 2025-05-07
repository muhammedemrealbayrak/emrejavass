using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoint : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
       // if (collision.name == "Leaf_Ranger") { Debug.Log("bimbom"); }
        if (collision.name == "MetalChar") { collision.GetComponent<Metal_Char_Combat>().normalAttackDamage += 10; }
        if (collision.name == "Cristal_Char") { collision.GetComponent<CristalCharControler>().normalAttackDamage += 10; }
       // if (collision.name == "Water_Priestess") { collision.GetComponent<Water_Priestess_Combat>().normalAttackDamage += 10; }
        Destroy(gameObject);
    }
}
