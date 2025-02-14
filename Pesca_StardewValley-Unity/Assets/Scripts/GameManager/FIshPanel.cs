using UnityEngine;
using UnityEngine.UI;

public class FIshPanel : MonoBehaviour
{
    [SerializeField] public Inventory inventory;
    [SerializeField] public Text numPeixes;
    public int fishCount = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fishCount = inventory.fishCount;
        numPeixes.text = inventory.fishCount.ToString();
    }
}
