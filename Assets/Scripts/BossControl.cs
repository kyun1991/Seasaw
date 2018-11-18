using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossControl : MonoBehaviour
{

    public GameObject bossStar;

    private List<GameObject> bossObjects = new List<GameObject>();

    private int counter = 0;

    // below function is dropping pattern for boss 1. This is called from Start function in GameControl script.
    public IEnumerator BossOne(bool attack = true, float delay1 = 0.5f, float delay2 = 3f)
    {
        while (attack == true)
        {
            yield return StartCoroutine(BossAttack(attack, delay1, delay2));
        }
    }

    // below function is dropping pattern for boss 2. This is called from Start function in GameControl script.
    public IEnumerator BossTwo(bool attack = true, float delay1 = 0.5f, float delay2 = 3f)
    {
        while (attack == true)
        {
            yield return StartCoroutine(BossAttack(attack, delay1, delay2));
        }
    }

    // below function is dropping pattern for boss 3. This is called from Start function in GameControl script.
    public IEnumerator BossThree(bool attack = true, float delay1 = 0.5f, float delay2 = 3f)
    {
        while (attack == true)
        {
            yield return StartCoroutine(BossAttack(attack, delay1, delay2));
        }
    }

    // below function is dropping pattern for boss 4. This is called from Start function in GameControl script.
    public IEnumerator BossFour(bool attack = true, float delay1 = 0.5f, float delay2 = 3f)
    {
        while (attack == true)
        {
            yield return StartCoroutine(BossAttack(attack, delay1, delay2));
        }
    }

    // below function is dropping pattern for boss 5. This is called from Start function in GameControl script.
    public IEnumerator BossFive(bool attack = true, float delay1 = 0.5f, float delay2 = 3f)
    {
        while (attack == true)
        {
            yield return StartCoroutine(BossAttack(attack, delay1, delay2));
        }
    }

    // initialise and drops boss objects after certain delay.
    IEnumerator BossAttack(bool attack, float delay1, float delay2)
    {
        bossObjects.Add(Instantiate(bossStar, new Vector2(Random.Range(-2.5f, 2.5f), 5.5f), Quaternion.identity));
        bossObjects[counter].GetComponent<Rigidbody2D>().isKinematic = true;
        yield return new WaitForSeconds(delay1);
        bossObjects[counter].GetComponent<Rigidbody2D>().isKinematic = false;
        bossObjects[counter].GetComponent<BoxCollider2D>().isTrigger = false;
        bossObjects[counter].GetComponent<LineRenderer>().enabled = false;
        counter++;
        yield return new WaitForSeconds(delay2);
    }

}
