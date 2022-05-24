using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        var tempPosition = transform.position;
        if (AllSourcePool.PlayerCharacter!=null)
        {
            tempPosition.x=AllSourcePool.PlayerCharacter.Transform.position.x;
        }
        transform.position = tempPosition;
    }
}
