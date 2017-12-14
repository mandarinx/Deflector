using GameEvents;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Modes/Endure")]
public class GameModeEndure : GameMode {

    [SerializeField]
    private int maxProjectiles;
    private int despawnedProjectiles;
    [SerializeField]
    private GameObjectEvent onProjectileDespawned;
    
    public override string title => $"Endure {maxProjectiles} projectiles";

    private void OnEnable() {
        Debug.Log("endure enable");
        onProjectileDespawned?.RegisterCallback(() => {
            ++despawnedProjectiles;
        });
    }

    protected override bool ValidateWinCondition() {
        return despawnedProjectiles >= maxProjectiles;
    }
}
