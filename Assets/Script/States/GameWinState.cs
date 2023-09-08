using Managers;
using System;
using UnityEngine;

namespace States
{
    public class GameWinState : GameState
    {
        public Action GameWinAction;
        public static GameWinState Instance { get; private set; }
        public GameWinUI GameWinUI;

        protected override void SetName()
        {
            Name = nameof(GameWinState);
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
            Debug.Log("<color=green>Game Win State</color> OnActive");
            GameWinUI.OnActivate();
        }

        public override void OnDeactivate()
        {
            Debug.Log("<color=red>Game Win State</color> OnDeactivate");
            GameWinUI.OnDeactivate();
        }

        public override void OnUpdateState()
        {
        }

        #endregion

        public void NextLevel()
        {
            GameManager.Instance.SetState(nameof(PreGameState));
            Time.timeScale = 1;
        }
    }
}