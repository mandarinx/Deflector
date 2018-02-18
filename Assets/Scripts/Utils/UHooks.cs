using UnityEngine;
using System.Collections.Generic;

namespace LunchGame01 {
    public interface IOnGUI {
        void UOnGUI();
    }

    public interface IOnUpdate {
        void UOnUpdate();
    }

    public class UHooks : MonoBehaviour {

        // OnGUI

        private List<IOnGUI> onGUIs;
        private UOnGUI       onGUI;

        public int           numOnGUIs => onGUIs.Count;

        public IOnGUI GetOnGUI(int n) {
            return onGUIs[n];
        }

        // Update

        private List<IOnUpdate> onUpdates;
        private UOnUpdate       onUpdate;

        public int              numOnUpdates => onUpdates.Count;

        public IOnUpdate GetOnUpdate(int n) {
            return onUpdates[n];
        }

        private void Awake() {
            onUpdates = new List<IOnUpdate>();
            onGUIs = new List<IOnGUI>();
        }

        public void AddOnUpdate(IOnUpdate hook) {
            if (onUpdate == null) {
                onUpdate = gameObject.AddComponent<UOnUpdate>();
                onUpdate.SetUHooks(this);
            }

            if (onUpdates.Contains(hook)) {
                return;
            }

            onUpdates.Add(hook);
        }

        public void RemoveOnUpdate(IOnUpdate hook) {
            onUpdates.Remove(hook);
        }

        public void AddOnGUI(IOnGUI hook) {
            if (onGUI == null) {
                onGUI = gameObject.AddComponent<UOnGUI>();
                onGUI.SetUHooks(this);
            }

            if (onGUIs.Contains(hook)) {
                return;
            }

            onGUIs.Add(hook);
        }
    }

    public class UOnGUI : MonoBehaviour {

        private UHooks uhooks;

        public void SetUHooks(UHooks hooks) {
            uhooks = hooks;
        }

        private void OnGUI() {
            for (int i = 0; i < uhooks.numOnGUIs; ++i) {
                uhooks.GetOnGUI(i).UOnGUI();
            }
        }
    }

    public class UOnUpdate : MonoBehaviour {

        private UHooks uhooks;

        public void SetUHooks(UHooks hooks) {
            uhooks = hooks;
        }

        private void Update() {
            for (int i = 0; i < uhooks.numOnUpdates; ++i) {
                uhooks.GetOnUpdate(i).UOnUpdate();
            }
        }
    }
}
