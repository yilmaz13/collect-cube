using UnityEngine;

namespace States
{
    public class MenuState : GameState
    {
        #region implemented abstract members of GameState

        public static MenuState Instance { get; private set; }
        public MenuUI MenuUI;

        protected override void SetName()
        {
            Name = nameof(MenuState);
        }

        protected override void Awake()
        {
            base.Awake();
            SetName();
            Instance = this;
        }

        public override void OnActivate()
        {           
            Debug.Log("<color=green>Menu State</color> OnActive");
            MenuUI.OnActivate();
        }

        public override void OnDeactivate()
        {
            Debug.Log("<color=red>Menu State</color> OnDeactivate");
        }

        public override void OnUpdateState()
        {
        }

        #endregion
    
    }
}