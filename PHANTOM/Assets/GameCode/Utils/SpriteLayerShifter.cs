using UnityEngine;

public class SpriteLayerShifter : MonoBehaviour
{
    void Start()
    {
        var offset = Random.Range(-10000, 10000);
        foreach (var renderer in GetComponentsInChildren<SpriteRenderer>())
        {
            renderer.sortingOrder += offset;
        }
        Destroy(this);
    }
}
