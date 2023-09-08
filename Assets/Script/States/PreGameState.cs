using Managers;
using UnityEngine;

namespace States
{
    public class PreGameState : GameState
    {
        #region implemented abstract members of _StatesBase             

        public static PreGameState Instance { get; private set; }
        protected override void SetName()
        {
            Name = nameof(PreGameState);
        }

        protected override void Awake()
        {
            base.Awake();
            SetName();         

        }

        public override void OnActivate()
        {
            Debug.Log("<color=green>PreGame State</color> OnActive");
            LevelManager.Instance.SetLevel();           
        }

        public override void OnDeactivate()
        {
            Debug.Log("<color=red>PreGame State</color> OnDeactivate");
        }

        public override void OnUpdateState()
        {
        }

        #endregion          
    }
}