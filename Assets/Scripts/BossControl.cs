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

    // basic pattern. Stone dropping from top.
    public IEnumerator BossOne(bool attack = true, float delay1 = 0.5f, float delay2 = 2.5f)
    {
        while (attack == true)
        {
            yield return StartCoroutine(BossAttack(attack, delay1, delay2));
        }
    }

    // bird coming from left. Slow spawned birds.
    public IEnumerator BossTwo(bool attack = true, float delay1 = 0.5f, float delay2 = 1.5f, float velocity = 7)
    {
        while (attack == true)
        {
            yield return StartCoroutine(BossAttackFromLeft(attack, delay1, delay2,velocity));
        }
    }

  // bird coming from right. Faster spawned birds.
    public IEnumerator BossThree(bool attack = true, float delay1 = 0.5f, float delay2 = 1f, float velocity = 10f)
    {
        while (attack == true)
        {
            yield return StartCoroutine(BossAttackFromRight(attack, delay1, delay2, velocity));
        }
    }

    // stone drop then bird spawn.
    public IEnumerator BossFour(bool attack = true, float delay1 = 0.5f, float delay2 = 1.3f, float velocity = 7)
    {
        while (attack == true)
        {
            yield return StartCoroutine(BossAttack(attack, delay1, delay2));
            if (Random.Range(0, 2) == 0)
            {
                yield return StartCoroutine(BossAttackFromLeft(attack, delay1, delay2, velocity));
            }
            else
            {
                yield return StartCoroutine(BossAttackFromRight(attack, delay1, delay2, velocity));
            }
        }
    }

    // random between any 3 pattern. 
    public IEnumerator BossFive(bool attack = true, float delay1 = 0.5f, float delay2 = 0.8f, float velocity = 10f)
    {
        while (attack == true)
        {
            int temp = Random.Range(0, 3);
            if (temp == 0)
            {
                yield return StartCoroutine(BossAttackFromLeft(attack, delay1, delay2, velocity));
            }
            else if(temp==1)
            {
                yield return StartCoroutine(BossAttackFromRight(attack, delay1, delay2, velocity));
            }
            else
            {
                yield return StartCoroutine(BossAttack(attack, delay1, delay2));
            }
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

    IEnumerator BossAttackFromLeft(bool attack, float delay1, float delay2, float velocity)
    {
        bossObjects.Add(Instantiate(bossBirdToRight, new Vector2(-5f, Random.Range(0, 3f)), Quaternion.identity));
        bossObjects[counter].GetComponent<Rigidbody2D>().isKinematic = true;
        bossObjects[counter].GetComponent<LineRenderer>().SetPosition(0, new Vector3(0, 0.25f, 0));
        bossObjects[counter].GetComponent<LineRenderer>().SetPosition(1, new Vector3(25, 0.25f, 0));
        yield return new WaitForSeconds(delay1);
        bossObjects[counter].GetComponent<Rigidbody2D>().velocity = new Vector2(velocity, 0);
        bossObjects[counter].GetComponent<CircleCollider2D>().isTrigger = false;
        bossObjects[counter].GetComponent<LineRenderer>().enabled = false;
        counter++;
        yield return new WaitForSeconds(delay2);
    }

    IEnumerator BossAttackFromRight(bool attack, float delay1, float delay2, float velocity)
    {
        bossObjects.Add(Instantiate(bossBirdToLeft, new Vector2(5f, Random.Range(0, 3f)), Quaternion.identity));
        bossObjects[counter].GetComponent<Rigidbody2D>().isKinematic = true;
        bossObjects[counter].GetComponent<LineRenderer>().SetPosition(0, new Vector3(0, 0.25f, 0));
        bossObjects[counter].GetComponent<LineRenderer>().SetPosition(1, new Vector3(-25, 0.25f, 0));
        yield return new WaitForSeconds(delay1);
        bossObjects[counter].GetComponent<Rigidbody2D>().velocity = new Vector2(-velocity, 0);
        bossObjects[counter].GetComponent<CircleCollider2D>().isTrigger = false;
        bossObjects[counter].GetComponent<LineRenderer>().enabled = false;
        counter++;
        yield return new WaitForSeconds(delay2);
    }
}
