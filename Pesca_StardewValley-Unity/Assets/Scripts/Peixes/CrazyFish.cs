using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CrazyFish : MonoBehaviour
{
    public float defaultCrazyness;
    public float crazyness;
    Rigidbody2D rb;

    public void Setup(float DefaultCrazyness, float crazyness, Rigidbody2D rb)
    {
        this.defaultCrazyness = DefaultCrazyness;
        this.crazyness = crazyness;
        this.rb = rb;
    }

    public void move()
    {
        Vector2 force = new Vector2(0f, Random.Range(-crazyness, crazyness));
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
        if (crazyness < 30f)
        {
            crazyness += 2f;
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
            crazyness -= 2f;
            yield return new WaitForSeconds(0.5f);
            StartCoroutine("lessCrazyness");
        }
        yield return new WaitForSeconds(0);
    }

}
