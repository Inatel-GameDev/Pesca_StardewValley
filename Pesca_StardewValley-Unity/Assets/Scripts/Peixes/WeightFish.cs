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

    public void Setup(float DefaultCrazyness, float crazyness, Rigidbody2D rb, float weight)
    {
        // O valor de crazynees original que o peixe vai rodar em volta
        this.defaultCrazyness = DefaultCrazyness;
        // Valor flutuante de loucura do peixe
        this.crazyness = crazyness;
        this.rb = rb;
        this.weight = weight;   
    }

    public void move()
    {
        Vector2 force;
        // Sobe loco
        if (weight < 15f)
        {
            if (rb.position.y <= 0)
            {
                force = new Vector2(0f, Random.Range(0, (weight/10) * crazyness));
            }
            else
            {
                force = new Vector2(0f, Random.Range(-crazyness, 0));
            }
            }
        else if (weight > 15f)
        {
            if (rb.position.y <= 0)
            {
                force = new Vector2(0f, Random.Range(0,crazyness));
            }
            else
            {
                force = new Vector2(0f, Random.Range(-crazyness * (30-weight / 10), 0));
            }
        }
        else
            force = new Vector2(0f, Random.Range(-crazyness, crazyness));
        rb.AddForce(force);
    }

    public void startTriggerFish()
    {
        StartCoroutine(moreCrazyness());
    }

    public void stopTriggerFish()
    {
        StartCoroutine(lessCrazyness());
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
