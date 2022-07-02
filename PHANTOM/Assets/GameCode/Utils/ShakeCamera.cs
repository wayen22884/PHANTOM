using UnityEngine;

public class ShakeCamera : StateMachineBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private float strength;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var controller = Camera.main.GetComponent<CameraController>();
        Debug.Assert(controller != null, "Camera controller should be attached on main camera");
        controller.Shake(this.duration, this.strength);
    }
}
