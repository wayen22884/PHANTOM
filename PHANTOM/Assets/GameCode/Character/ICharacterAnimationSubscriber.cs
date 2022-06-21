public interface ICharacterAnimationSubscriber<T> where T : ICharacter
{
    void Subscribe(T publisher);
}
