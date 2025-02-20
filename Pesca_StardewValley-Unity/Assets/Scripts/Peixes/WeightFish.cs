using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WeightFish : MonoBehaviour
{
    public float defaultCrazyness;
    public float crazyness;
    public float maxCrazyness;
    public float crazynessIncrease;
    public float weight;
    Rigidbody2D rb;

    public void Setup(GenericalFish fish, Rigidbody2D rb)
    {
        // O valor de crazynees original que o peixe vai rodar em volta
        this.defaultCrazyness = fish.crazyness;
        // Valor flutuante de loucura do peixe
        this.crazyness = fish.crazyness;
        this.rb = rb;
        this.weight = fish.weight;
        this.crazynessIncrease = fish.crazynessIncrease;
        this.maxCrazyness = fish.maxCrazyness;
    }

    public void move()
    {
        Vector2 force;
        // leve
        Debug.Log("Weight: " + weight);
        if (weight < 15f)
        {
            if (rb.position.y <= 0)
            {
                force = new Vector2(0f, Random.Range(0, (weight * 10f) * crazyness));
            }
            else
            {
                force = new Vector2(0f, Random.Range(-crazyness, 0));
            }
            }
        else
        {
            if (rb.position.y <= 0)
            {
                force = new Vector2(0f, Random.Range(0,crazyness));
                Debug.Log("ForceDown: " + force);
            }
            else
            {
                force = new Vector2(0f, Random.Range(-crazyness * ((weight - 14) * 10f), 0));
                Debug.Log("ForceUp: " + force);
            }
        }
    }

    public void startTriggerFish()
    {
        StartCoroutine(moreCrazyness());
    }

    public void stopTriggerFish()
    {
        StartCoroutine(lessCrazyness());
        Debug.Log("Crazyness: " + crazyness);
        if (weight < 15f)
        {
            Vector2 force;
            force = new Vector2(0f, Random.Range(-crazyness, (weight*5f)*crazyness));
            rb.AddForce(force);
        }
        else
        {
            Vector2 force;
            force = new Vector2(0f, Random.Range(-crazyness * ((weight-14)*5f), crazyness));
            rb.AddForce(force);
        }
    }

    IEnumerator moreCrazyness()
    {
        StopCoroutine("lessCrazyness");
        if (crazyness < maxCrazyness)
        {
            crazyness += crazynessIncrease;
            yield return new WaitForSeconds(1f);
            StartCoroutine("moreCrazyness");
        }
        yield return new WaitForSeconds(0);
    }
    IEnumerator lessCrazyness()
    {
        StopCoroutine("moreCrazyness");
        if (crazyness > defaultCrazyness)
        {
            crazyness -= crazynessIncrease;
            yield return new WaitForSeconds(0.5f);
            StartCoroutine("lessCrazyness");
        }
        yield return new WaitForSeconds(0);
    }
}
