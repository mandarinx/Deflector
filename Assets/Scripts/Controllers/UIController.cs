using UnityEngine;
using System.Collections.Generic;
using GameEvents;
using UnityEngine.Assertions;

namespace Deflector {

    public class UIController : MonoBehaviour, IOnUpdate {

        [SerializeField]
        private UIPanelRef                        defaultPanel;
        [SerializeField]
        private UIPanelRef                        nonePanel;
        [SerializeField]
        private UHooks                            uHooks;
        [SerializeField]
        private Transform                         panelRoot;
        [SerializeField]
        private GameEvent                         onMenuOptionSelected;
        [SerializeField]
        private GameEvent                         onMenuOptionClicked;

        private int[]                             panelIds;
        private readonly Dictionary<int, UIPanel> panels = new Dictionary<int, UIPanel>();
        private int                               currentPanelInstanceId;
        private readonly List<MenuOption>         currentOptions = new List<MenuOption>();
        private readonly List<UIPanelRef>         currentPanelRefs = new List<UIPanelRef>();
        private int                               currentOption;

        private void Start() {
            Assert.IsNotNull(defaultPanel, "UIController is missing a default UIPanel");
            Assert.IsNotNull(nonePanel, "UIController is missing a blank UIPanel");
            Assert.IsNotNull(uHooks, "UIController is missing a reference to UHooks");
            Assert.IsNotNull(panelRoot, "UIController is missing a reference to a transform that contains all panels");
            RegisterPanels();
            CloseAllPanels();
            OpenPanel(defaultPanel);
        }

        public void RegisterPanels() {
            UIPanel[] uiPanels = panelRoot.GetComponentsInChildren<UIPanel>();
            panelIds = new int[uiPanels.Length];
            for (int i = 0; i < uiPanels.Length; ++i) {
                panelIds[i] = uiPanels[i].PanelRef.GetInstanceID();
                panels.Add(panelIds[i], uiPanels[i]);
            }
        }

        private void CloseAllPanels() {
            for (int i = 0; i < panelIds.Length; ++i) {
                panels[panelIds[i]].Close();
            }
        }

        private void ClosePanel(int instanceId) {
            UIPanel panel;
            if (instanceId == 0 || !panels.TryGetValue(instanceId, out panel)) {
                return;
            }
            panel.Close();
        }

        public void OpenPanel(UIPanelRef panelRef) {
            int instanceId = panelRef.GetInstanceID();
            if (instanceId == currentPanelInstanceId) {
                return;
            }

            UIPanel nextPanel;
            if (!panels.TryGetValue(instanceId, out nextPanel)) {
                return;
            }

            uHooks.RemoveOnUpdate(this);
            ClosePanel(currentPanelInstanceId);

            currentOptions.Clear();
            currentOptions.AddRange(nextPanel.MenuOptions);
            currentPanelRefs.Clear();
            currentPanelRefs.AddRange(nextPanel.PanelRefs);
            currentOption = 0;
            DeselectAllMenuOptions();
            HighlightMenuOption(currentOption);

            if (currentOptions.Count > 0) {
                uHooks.AddOnUpdate(this);
            }

            nextPanel.Open();
            currentPanelInstanceId = instanceId;
        }

        private bool HighlightMenuOption(int n) {
            if (currentOptions.Count == 0) {
                return false;
            }
            n = Mathf.Clamp(n, 0, currentOptions.Count);
            currentOptions[n].Select();
            return true;
        }

        private void SelectMenuOption(int n) {
            if (!HighlightMenuOption(n)) {
                return;
            }
            if (onMenuOptionSelected != null) {
                onMenuOptionSelected.Invoke();
            }
        }

        private void DeselectMenuOption(int n) {
            if (currentOptions.Count == 0) {
                return;
            }
            n = Mathf.Clamp(n, 0, currentOptions.Count);
            currentOptions[n].Deselect();
        }

        private void DeselectAllMenuOptions() {
            for (int i = 0; i < currentOptions.Count; ++i) {
                currentOptions[i].Deselect();
            }
        }

        public void UOnUpdate() {
            if (Input.GetKeyUp(KeyCode.UpArrow)) {
                DeselectMenuOption(currentOption);
                --currentOption;
                if (currentOption < 0) {
                    currentOption = currentOptions.Count - 1;
                }
                currentOption = currentOption % currentOptions.Count;
                SelectMenuOption(currentOption);
            }

            if (Input.GetKeyUp(KeyCode.DownArrow)) {
                DeselectMenuOption(currentOption);
                ++currentOption;
                currentOption = currentOption % currentOptions.Count;
                SelectMenuOption(currentOption);
            }

            if (Input.GetKeyUp(KeyCode.X)) {
                MenuOption opt = currentOptions[currentOption];
                if (!opt.gameObject.activeSelf) {
                    return;
                }
                opt.Click();
                if (onMenuOptionClicked != null) {
                    onMenuOptionClicked.Invoke();
                }
                UIPanelRef next = currentPanelRefs[currentOption];
                if (next == nonePanel) {
                    ClosePanel(currentPanelInstanceId);
                    return;
                }
                OpenPanel(next);
            }
        }
    }
}
