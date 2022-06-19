using UnityEngine;

// TODO: Use a more general form (e.g. publish/receive an event) to handle this logic
public class SpawnInArea : StateMachineBehaviour
{
    [SerializeField]
    private GameObject toSpawn;
    [SerializeField]
    private Bounds bounds;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.spawn(animator.transform);
    }

    private void spawn(Transform transform)
    {
        Vector3 spawnPoint = this.getSpawnPoint(transform);
        Instantiate(this.toSpawn, spawnPoint, Quaternion.identity);
    }

    private Vector3 getSpawnPoint(Transform transform)
    {
        var isFaceToRight = transform.right.x > 0;
        var spawnX = Random.Range(this.bounds.min.x, this.bounds.max.x) * (isFaceToRight ? 1 : -1);
        var spawnY = Random.Range(this.bounds.min.y, this.bounds.max.y);
        var spawnPoint = transform.position + new Vector3(spawnX, spawnY);
        return spawnPoint;
    }
}
