using System.Collections;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    [SerializeField]
    private float timeToLive;

    IEnumerator Start()
    {
        Debug.Assert(this.timeToLive > 0, "TTL should greater than 0");
        yield return new WaitForSeconds(this.timeToLive);
        Destroy(gameObject);
    }
}
