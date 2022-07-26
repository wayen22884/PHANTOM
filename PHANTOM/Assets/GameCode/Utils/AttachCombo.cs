using UnityEngine;

public class AttachCombo : MonoBehaviour, ICharacterAnimationSubscriber<EnemyCharacter>, ICharacterAnimationSubscriber<PlayerCharacter>
{
    public void Subscribe(EnemyCharacter publisher)
    {
        publisher.combo = GameResource.Combo;
        Debug.Log("Enemy combo set");
    }

    public void Subscribe(PlayerCharacter publisher)
    {
        publisher.combo = GameResource.Combo;
        Debug.Log("Player combo set");
    }
}
