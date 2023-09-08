using System;
using System.Collections.Generic;
using Managers;
using States;
using GameCore;
using UnityEngine;
using Utilities;

namespace Managers
{
    public class LevelManager : Singleton<LevelManager>
    {
        #region Variables      

        [SerializeField] private int currentIndex = 0;
        public Level currentLevel;
        public List<CubeControlller> Cubes = new List<CubeControlller>();
        public List<CubeControlller> CubesCollected = new List<CubeControlller>();
        public PlayerController playerController;
        public EnemyController enemyController;

        public Target targetPlayer;
        public Target targetEnemy;

        private int blocksIndex = 0;

        public float gameMaxTime = 30;
        public float currentTime;
        public int playerScore;
        public int enemyScore;

        #endregion

        #region Public Methods

        protected override void Awake()
        {
            base.Awake();
            if (PlayerPrefs.HasKey("currentIndex"))
            {
                currentIndex = PlayerPrefs.GetInt("currentIndex");
            }
            else
            {
                currentIndex = 0;
                PlayerPrefs.SetInt("currentIndex", 0);
            }

            var path = Application.dataPath + "/LevelEditor/Resources/Levels/" + 0 + ".json";
            if (!string.IsNullOrEmpty(path))
            {
                currentLevel = Extensions.LoadJsonFile<Level>(path);
            }
        }

        public void CheckLevel()
        {
            if (Cubes.Count == 0 && currentLevel.levelType == LevelType.One)
            {
                GameManager.Instance.GameWin();
            }
            if (Cubes.Count == 0 && currentLevel.levelType == LevelType.Time)
            {
                SpawntObj();
            }
        }

        public Vector3 RandomCubeVector()
        {
            int randomIndex = UnityEngine.Random.Range(0, Cubes.Count);
            return Cubes[randomIndex].GetPosition();
        }

        public CubeControlller RandomCube()
        {
            int randomIndex = UnityEngine.Random.Range(0, Cubes.Count);
            return Cubes[randomIndex];
        }
        public void Update()
        {
            if (GameManager.Instance.IsGameStarted && currentLevel.levelType == LevelType.Multiplayer)
            //  if (GameManager.Instance.IsGameStarted && currentLevel.levelType is LevelType.Time or LevelType.Multiplayer)
            {
                currentTime -= Time.deltaTime;
                GamePlayState.Instance.UpdateTime((int)Math.Round(currentTime));
                if (currentTime <= 0)
                {
                    GameManager.Instance.GameWin();
                }
            }
        }

        public void Timer()
        {

        }
        public void SubscribeCubeListener(CubeControlller cube)
        {
            Cubes.Add(cube);
        }

        public void UnSubscribeCubeListener(CubeControlller cube)
        {
            Cubes.Remove(cube);
        }

        public void SubscribeCubesCollectedListener(CubeControlller cube)
        {
            CubesCollected.Add(cube);

            if (cube.GetType() == ControllerType.player)
            {
                playerScore += cube.Score;
                GamePlayState.Instance.UpdatePlayerScore(playerScore, cube.GetType());
            }
            else if (cube.GetType() == ControllerType.enemy)
            {
                enemyScore += cube.Score;
                GamePlayState.Instance.UpdateEnemyScore(enemyScore, cube.GetType());
            }
        }

        public void UnSubscribeCubesCollectedListener(CubeControlller cube)
        {
            CubesCollected.Remove(cube);
        }

        public void SpawnPlayerController()
        {
            if (!playerController)
            {
                playerController = SpawnManager.Instance.Spawn(GamePlayState.Instance.PlayerCollectorPoint, "PlayerCollector").GetComponent<PlayerController>();
                targetPlayer = SpawnManager.Instance.Spawn(GamePlayState.Instance.PlayerTargetPoint, "Target").GetComponent<Target>();
                targetPlayer.SetTarget(ControllerType.player);
                playerController.SetController(GamePlayState.Instance.GetJoystick(), targetPlayer);
            }
        }

        public void SpawnEnemyController()
        {
            if (!enemyController)
            {
                enemyController = SpawnManager.Instance.Spawn(GamePlayState.Instance.EnemyCollectorPoint, "EnemyCollector").GetComponent<EnemyController>();
                targetEnemy = SpawnManager.Instance.Spawn(GamePlayState.Instance.EnemyTargetPoint, "Target").GetComponent<Target>();
                targetEnemy.SetTarget(ControllerType.enemy);
                enemyController.SetSpawnPoint(GamePlayState.Instance.EnemyCollectorPoint, targetEnemy);

            }
        }

        public void SpawnBlocks(int index)
        {
            float baseValue = currentLevel.width / 2 * 0.25f + 0.125f + GamePlayState.Instance.SpawnPoint.position.x;
            var currentblockList = currentLevel.blockList[index];
            for (var j = 0; j < currentLevel.height; j++)
            {
                for (var i = 0; i < currentLevel.width; i++)
                {
                    var idx = i + j * currentLevel.width;
                    var cube = SpawnManager.Instance.Spawn(new Vector3(i * 0.25f - baseValue, 0.25f * index + 0.125f, j * 0.25f), "CubeBase");
                    CubeControlller cubeControlller = cube.GetComponent<CubeControlller>();
                    cubeControlller.SetCube(GamePlayState.Instance.target, true, currentblockList.blocks[idx].color, currentLevel.oneCubeScore);
                    Cubes.Add(cubeControlller);
                }
            }
        }

        [ContextMenu("Spawn TestObj")]
        public void SpawntObj()
        {
            if (currentLevel.levelType == LevelType.Time)
            {
                SpawnPlayerController();
                SpawnBlocks(blocksIndex);

                blocksIndex++;
                if (currentLevel.blockList.Count - 1 < blocksIndex)
                {
                    blocksIndex = currentLevel.blockList.Count - 1;
                }
            }
            else if (currentLevel.levelType == LevelType.One)
            {
                SpawnPlayerController();

                for (int index = 0; index < currentLevel.blockList.Count; index++)
                {
                    SpawnBlocks(index);
                }
            }
            else if (currentLevel.levelType == LevelType.Multiplayer)
            {
                SpawnPlayerController();
                SpawnEnemyController();

                for (int index = 0; index < currentLevel.blockList.Count; index++)
                {
                    SpawnBlocks(index);
                }

            }
        }

        public void ClearLevel()
        {
            for (int i = 0; i < Cubes.Count; i++)
            {
                var cube = Cubes[i].GetComponent<PoolObject>();
                cube.GoToPool();

            }
            for (int i = 0; i < CubesCollected.Count; i++)
            {
                var cube = CubesCollected[i].GetComponent<PoolObject>();
                cube.GoToPool();
            }

            CubesCollected.Clear();
            Cubes.Clear();

            if (playerController != null)
            {
                playerController.GetComponent<PoolObject>().GoToPool();
                playerController = null;
            }
            if (enemyController != null)
            {
                enemyController.GetComponent<PoolObject>().GoToPool();
                enemyController = null;
            }

            playerScore = 0;
            enemyScore = 0;
        }

        public void SetLevel()
        {
            blocksIndex = 0;

            GamePlayState.Instance.ResetUI();

            GamePlayState.Instance.UpdateLevel(currentLevel.id);
            if (currentLevel.levelType == LevelType.Time || currentLevel.levelType == LevelType.Multiplayer)
            {
                currentTime = currentLevel.time;
            }

            SpawntObj();

            GameManager.Instance.SetState1(nameof(GamePlayState));
        }


        public void SetNextLevel()
        {
            currentIndex++;
            PlayerPrefs.SetInt("currentIndex", currentIndex);
            if (currentIndex > 2)
                currentIndex = 0;

            var path = Application.dataPath + "/LevelEditor/Resources/Levels/" + currentIndex + ".json";
            if (!string.IsNullOrEmpty(path))
            {
                currentLevel = Extensions.LoadJsonFile<Level>(path);
            }
        }

        #endregion
    }
}