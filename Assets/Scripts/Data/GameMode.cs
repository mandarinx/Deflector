using System;
using UnityEngine;

namespace Deflector {
    public class GameMode : ScriptableObject {

        public virtual string title => "";
        public Action         onGameWon = () => { };
        public Action         onGameLost = () => { };

        public virtual void Deactivate() {}
        public virtual void Activate() {}
        public virtual void Validate() {}

        protected void GameLost() {
            onGameLost?.Invoke();
        }

        protected void GameWon() {
            onGameWon?.Invoke();
        }
    }
}
