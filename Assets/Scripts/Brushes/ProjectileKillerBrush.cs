using UnityEngine;
using UnityEngine.Tilemaps;
using GameEvents;
using LunchGame01;

[CreateAssetMenu(menuName = "Brushes/Projectile Killer",
                 fileName = "ProjectileKillerBrush.asset")]
[CustomGridBrush(hideAssetInstances:  false,
                 hideDefaultInstance: true,
                 defaultBrush:        false,
                 defaultName:         "Projectile Killer")]
public class ProjectileKillerBrush : GridBrushBase {

    [SerializeField]
    private string 		    layerName;
    [SerializeField]
    private Tile            tile;
    [SerializeField]
    private GameObjectEvent onKilledEvent;
    [SerializeField]
    private LayerMask       killerMask;

    public override void Paint(GridLayout grid, GameObject layer, Vector3Int position) {
        Tilemap tm = BrushUtility
           .GetLayer(layerName)
           .GetComponent<Tilemap>();

        Killer killer = tm.gameObject.GetComponent<Killer>() ?? layer.AddComponent<Killer>();
        killer.SetOnKilledEvent(onKilledEvent);
        killer.SetLayerMask(killerMask);

        tm.SetTile(position, tile);
    }

    public override void Erase(GridLayout grid, GameObject layer, Vector3Int position) {
        BrushUtility
           .GetLayer(layerName)
           .GetComponent<Tilemap>()
           .SetTile(position, null);
    }
}
