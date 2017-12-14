using GameEvents;
using UnityEngine;

public class UIPanelToggle : StateMachineBehaviour {
	
	[SerializeField]
	private string      panelName;
	[SerializeField]
	private StringEvent panelOnEvent;
	[SerializeField]
	private StringEvent panelOffEvent;
	
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		panelOnEvent.Invoke(panelName);
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
		panelOffEvent.Invoke(panelName);
	}
}
