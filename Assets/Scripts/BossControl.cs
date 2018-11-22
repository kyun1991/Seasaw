using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossControl : MonoBehaviour
{

    public GameObject bossStone;
    public GameObject bossBirdToLeft;
    public GameObject bossBirdToRight;

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
            yield return StartCoroutine(BossAttackFromLeft(attack, delay1, delay2));
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
        bossObjects.Add(Instantiate(bossStone, new Vector2(Random.Range(-2.5f, 2.5f), 7f), Quaternion.identity));
        bossObjects[counter].GetComponent<Rigidbody2D>().isKinematic = true;
        bossObjects[counter].GetComponent<LineRenderer>().SetPosition(1, new Vector3(0, -25, 0));
        yield return new WaitForSeconds(delay1);
        bossObjects[counter].GetComponent<Rigidbody2D>().isKinematic = false;
        bossObjects[counter].GetComponent<BoxCollider2D>().isTrigger = false;
        bossObjects[counter].GetComponent<LineRenderer>().enabled = false;
        counter++;
        yield return new WaitForSeconds(delay2);
    }

    IEnumerator BossAttackFromLeft(bool attack, float delay1, float delay2)
    {
        bossObjects.Add(Instantiate(bossBirdToRight, new Vector2(-5f, Random.Range(-0.5f, 2.5f)), Quaternion.identity));
        bossObjects[counter].GetComponent<Rigidbody2D>().isKinematic = true;
        bossObjects[counter].GetComponent<LineRenderer>().SetPosition(0, new Vector3(0, 0.25f, 0));
        bossObjects[counter].GetComponent<LineRenderer>().SetPosition(1, new Vector3(25, 0.25f, 0));
        yield return new WaitForSeconds(delay1);
        bossObjects[counter].GetComponent<Rigidbody2D>().velocity = new Vector2(7, 0);
        bossObjects[counter].GetComponent<CircleCollider2D>().isTrigger = false;
        bossObjects[counter].GetComponent<LineRenderer>().enabled = false;
        counter++;
        yield return new WaitForSeconds(delay2);
    }

    IEnumerator BossAttackFromRight(bool attack, float delay1, float delay2)
    {
        bossObjects.Add(Instantiate(bossBirdToRight, new Vector2(5f, Random.Range(0.5f, 2.5f)), Quaternion.identity));
        bossObjects[counter].GetComponent<Rigidbody2D>().isKinematic = true;
        bossObjects[counter].GetComponent<LineRenderer>().SetPosition(0, new Vector3(0, 0.25f, 0));
        bossObjects[counter].GetComponent<LineRenderer>().SetPosition(1, new Vector3(-25, 0.25f, 0));
        yield return new WaitForSeconds(delay1);
        bossObjects[counter].GetComponent<Rigidbody2D>().velocity = new Vector2(7, 0);
        bossObjects[counter].GetComponent<CircleCollider2D>().isTrigger = false;
        bossObjects[counter].GetComponent<LineRenderer>().enabled = false;
        counter++;
        yield return new WaitForSeconds(delay2);
    }
}
