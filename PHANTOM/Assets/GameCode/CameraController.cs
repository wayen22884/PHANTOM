using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public Transform cameraRightLimit;
    public Transform cameraLeftLimit;

    void Update()
    {
        var tempPosition = transform.position;
        if (AllSourcePool.PlayerCharacter != null && AllSourcePool.PlayerCharacter.Transform != null)
        {
            tempPosition.x = AllSourcePool.PlayerCharacter.Transform.position.x;
            tempPosition.x = Mathf.Clamp(tempPosition.x, cameraLeftLimit.position.x, cameraRightLimit.position.x);
        }
        transform.position = tempPosition;
    }

    public void Shake(float duration, float strength = 1)
    {
        transform.DOShakePosition(duration, strength);
    }
}
