using System.Collections.Generic;
using PoolSystem;
using UI;
using UnityEngine;

namespace States
{
    public class GamePlayState : GameState
    {

        #region Public Transfoms
        public static GamePlayState Instance { get; private set; }

        public GamePlayUI GamePlayUI;

        public Transform SpawnParent;
        public Transform UpSpawn;
        public Transform target;

        public Transform PlayerCollectorPoint;
        public Transform EnemyCollectorPoint;

        public Transform PlayerTargetPoint;
        public Transform EnemyTargetPoint;
        public Transform SpawnPoint;

        public List<CubeControlller> Cubes = new List<CubeControlller>();

        #endregion

        #region implemented abstract members of _StatesBase

        protected override void SetName()
        {
            Name = nameof(GamePlayState);
        }

        protected override void Awake()
        {
            base.Awake();
            SetName();
            Instance = this;

        }

        public override void OnActivate()
        {
            Debug.Log("<color=green>Gameplay State</color> OnActive");
            ObjectPooler.Instance.CheckPool();
        }

        public override void OnDeactivate()
        {
            Debug.Log("<color=red>Gameplay State</color> OnDeactivate");
        }

        public override void OnUpdateState()
        {

        }

        #endregion

        #region   UpdateUI
        public Joystick GetJoystick()
        {
            return GamePlayUI.Joystick;
        }

        public void ResetUI()
        {
            GamePlayUI.timeText.text = "";
            GamePlayUI.scorePlayerText.text = "";
            GamePlayUI.scoreEnemyText.text = "";
            GamePlayUI.levelText.text = "";
        }
        public void UpdateTime(int time)
        {
            GamePlayUI.timeText.text = time.ToString();
        }

        public void UpdatePlayerScore(int score, ControllerType type)
        {
            GamePlayUI.scorePlayerText.text = score.ToString();
        }
        public void UpdateEnemyScore(int score, ControllerType type)
        {
            GamePlayUI.scoreEnemyText.text = score.ToString();
        }

        public void UpdateLevel(int level)
        {
            GamePlayUI.levelText.text = level.ToString();
        }
        #endregion
    }
}