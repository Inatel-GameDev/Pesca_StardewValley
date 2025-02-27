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
        //Debug.Log("Force: " + crazyness * 4f);
        // leve
        //Debug.Log("Weight: " + weight);
        if (weight < 15f)
        {
            if (rb.position.y < 0)
            {
                force = new Vector2(0f, Random.Range(-crazyness, (1+(weight/ 6f)) * crazyness)* 1.2f);
            }
            else
            {
                force = new Vector2(0f, Random.Range(-crazyness, crazyness+1)* 1.2f);
            }
            }
        else
        {
            if (rb.position.y <= 0)
            {
                force = new Vector2(0f, Random.Range(-crazyness-1, crazyness)*1.2f);
                //Debug.Log("ForceDown: " + force);
            }
            else
            {
                //Debug.Log("good: " + -crazyness * ((weight - 14) * 2));
                force = new Vector2(0f, Random.Range(-crazyness * (1+((weight - 14)/6f)), crazyness)*1.2f);
               // Debug.Log("ForceUp: " + force);
            }
        }
        rb.AddForce(force);
    }

    public void startTriggerFish()
    {
        StartCoroutine(moreCrazyness());
    }

    public void stopTriggerFish()
    {
        StartCoroutine(lessCrazyness());
        //Debug.Log("Crazyness: " + crazyness);
        if (weight < 15f)
        {
            Vector2 force;
            force = new Vector2(0f, Random.Range(-crazyness, (weight)*crazyness));
            rb.AddForce(force);
        }
        else
        {
            Vector2 force;
            force = new Vector2(0f, Random.Range(-crazyness * ((weight-14)), crazyness));
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
