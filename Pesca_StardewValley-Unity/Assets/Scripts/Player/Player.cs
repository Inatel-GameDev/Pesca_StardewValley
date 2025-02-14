using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public GameObject hookPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hookPrefab.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            hookPrefab.SetActive(true);
        }
    }
}
