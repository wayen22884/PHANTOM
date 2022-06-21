using UnityEngine;

public class AnimateState : StateMachineBehaviour
{
    [SerializeField]
    private string animateObjectName;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (Transform obj in animator.gameObject.transform)
        {
            // HACK: Hardcoded is bad
            if (obj.name.StartsWith("__HACK__"))
                continue;
            obj.gameObject.SetActive(obj.name.Equals(this.animateObjectName));
        }
    }
}
