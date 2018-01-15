using UnityEngine;

public class HitPointsController : MonoBehaviour {

    [SerializeField]
    private GameObjectPool hitPointsPool;
    private Grid           grid;

    public void SpawnHitPoint(Vector3 position, int numHitPoints) {
        HitPoint hitPoint = hitPointsPool
            .Spawn(transform, position)
            .GetComponent<HitPoint>();
            
        hitPoint.PositionAt(grid.GetCellCenterWorld(grid.WorldToCell(position)));
        hitPoint.Show(numHitPoints);
    }

    public void OnLevelLoaded(Level level) {
        grid = level.levelInstance.GetComponent<Grid>();
    }
}
