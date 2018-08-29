using System;
using UnityEngine;

namespace Deflector {
    public class GameMode : ScriptableObject {

        public Action         onGameWon = () => { };
        public Action         onGameLost = () => { };

        public virtual void Deactivate() {}
        public virtual void Activate() {}
        public virtual void Validate() {}

        public virtual string GetDescription(SystemLanguage lang) {
            return "N/A";
        }

        protected void GameLost() {
            onGameLost?.Invoke();
        }

        protected void GameWon() {
            onGameWon?.Invoke();
        }
    }
}
