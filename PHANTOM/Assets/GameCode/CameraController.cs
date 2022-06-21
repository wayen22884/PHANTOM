using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cameraRightLimit;
    public Transform cameraLeftLimit;
    // Update is called once per frame
    void Update()
    {
        var tempPosition = transform.position;
        if (AllSourcePool.PlayerCharacter!=null && AllSourcePool.PlayerCharacter.Transform!=null)
        {
            tempPosition.x=AllSourcePool.PlayerCharacter.Transform.position.x;
            tempPosition.x = Mathf.Clamp(tempPosition.x, cameraLeftLimit.position.x, cameraRightLimit.position.x);
        }
        transform.position = tempPosition;
    }
}
