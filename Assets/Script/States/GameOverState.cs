using UnityEngine;

namespace States
{
    public class GameOverState : GameState
    {
        public GameOverUI GameOverUI;
        public static GameOverState Instance { get; private set; }

        protected override void SetName()
        {
            Name = nameof(GameOverState);
        }

        protected override void Awake()
        {
            base.Awake();
            SetName();
            Instance = this;
        }

        #region implemented abstract members of StatesBase

        public override void OnActivate()
        {
            Debug.Log("<color=green>Game Over State</color> OnActive");
            GameOverUI.OnActivate();
        }

        public override void OnDeactivate()
        {
            Debug.Log("<color=red>Game Over State</color> OnDeactivate");
            GameOverUI.OnDeactivate();
        }

        public override void OnUpdateState()
        {
        }

        #endregion
    }
}