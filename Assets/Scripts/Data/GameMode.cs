using UnityEngine;

public class GameMode : ScriptableObject {

    private bool won = false;
    
    public virtual string title => "";

    private void OnEnable() {
        won = false;
    }

    public bool Validate() {
        if (won) {
            return true;
        }
        if (!ValidateWinCondition()) {
            return false;
        }
        won = true;
        return true;
    }

    protected virtual bool ValidateWinCondition() {
        return false;
    }
}
