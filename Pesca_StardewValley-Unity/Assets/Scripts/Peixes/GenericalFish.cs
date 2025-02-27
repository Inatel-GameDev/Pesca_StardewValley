using UnityEngine;
using UnityEngine.UI;

public class GenericalFish
{
    public string name;
    public float crazyness;
    public string classWeight;
    public float weight;
    public float maxCrazyness;
    public float crazynessIncrease;
    public string classFish;
    public float price;
    public GenericalFish(string name, float crazyness, string classWeight, float weight, float maxCrazyness, float crazynessIncrease, string classFish, float price)
    {
        this.name = name;
        this.crazyness = crazyness;
        this.classWeight = classWeight;
        this.weight = weight;
        this.maxCrazyness = maxCrazyness;
        this.crazynessIncrease = crazynessIncrease;
        this.classFish = classFish;
        this.price = price;
    }

}
