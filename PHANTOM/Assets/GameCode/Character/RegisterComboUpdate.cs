using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterComboUpdate : MonoBehaviour, ICharacterAnimationSubscriber<EnemyCharacter>
{
    public void Subscribe(EnemyCharacter publisher)
    {
        publisher.E_Attr.CallCombo += _ =>
        {
            publisher.combo?.Add(1);
        };
        publisher.E_Attr.CallCombo += x =>
        {
            if (x == 3)
            {
                var controller = Camera.main.GetComponent<CameraController>();
                Debug.Assert(controller != null, "Camera controller should be attached on main camera");
                // controller.Shake(this.duration, this.strength);
                controller.Shake(0.5f, 0.5f);
            }
        };
    }
}
