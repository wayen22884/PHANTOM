using UnityEngine;
using UnityEngine.Events;

public class DoAnimationEventProxy : MonoBehaviour, ICharacterAnimationSubscriber<PlayerCharacter>
{
    [HideInInspector]
    public UnityEvent<DoAnimationEventArgs> DoAnimation;

    public void Subscribe(PlayerCharacter publisher)
    {
        publisher.DoAnimation += (transform, state, isFaceRight) =>
        {
            this.DoAnimation.Invoke(new DoAnimationEventArgs
            {
                transform = transform,
                state = state,
                isFaceRight = isFaceRight,
            });
        };
    }
}
