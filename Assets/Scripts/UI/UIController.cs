using UnityEngine;

public class UIController : MonoBehaviour {

    [SerializeField]
    private UHooks   hooks;
    private Animator fsm;
    
    private void Start() {
        for (int i = 0; i < transform.childCount; ++i) {
            UIPanel panel = transform.GetChild(i).GetComponent<UIPanel>();
            panel.Init(this, hooks);
            panel.Close();
        }

        fsm = GetComponent<Animator>();
        NextState();
    }

    public void NextState() {
        fsm.SetTrigger("Next");
    }

    public void OnPlayerDied() {
        NextState();
    }
}
