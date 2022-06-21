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
        Vector3 spawnPoint = transform.position + this.getSpawnPoint();
        Instantiate(this.toSpawn, spawnPoint, Quaternion.identity);
    }

    private Vector3 getSpawnPoint()
    {
        var spawnX = Random.Range(this.bounds.min.x, this.bounds.max.x);
        var spawnY = Random.Range(this.bounds.min.y, this.bounds.max.y);
        return new Vector3(spawnX, spawnY);
    }
}
