using System.Collections;
using System.Collections.Generic;
using States;
using Managers;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        private bool pause;
        public GameState _currentGameState;
        public GameState GameState => _currentGameState;
        public bool Pause => pause;
        public List<GameState> activeStates = new List<GameState>();

        private void OnDestroy()
        {
            activeStates.Clear();
        }

        public void SubscribeStateListener(GameState stateListener)
        {
            activeStates.Add(stateListener);
            stateListener.ActiveStates = activeStates;
        }

        public void UnSubscribeStateListener(GameState stateListener)
        {
            activeStates.Remove(stateListener);
        }
               
        public void GameOver()
        {
            LevelManager.Instance.ClearLevel();          
            var state = GetState(nameof(GameOverState));
            CallStateEnters(state);
        }

        [ContextMenu("GameWin")]
        public void GameWin()
        {
            LevelManager.Instance.ClearLevel();
            LevelManager.Instance.SetNextLevel();

            var state = GetState(nameof(GameWinState));
            CallStateEnters(state);
        }

        private void Start()
        {
            Application.targetFrameRate = 60;
            var state = GetState(nameof(MenuState));
            CallFirstStateEnters(state);
        }

        public void SetState(string stateName)
        {
            var state = GetState(stateName);
            CallStateEnters(state);
        }
      
        private void CallStateEnters(GameState newState)
        {
            if (GameState != null)
            {
                GameState.OnDeactivate();
            }

            if (newState != null)
            {
                newState.OnActivate();
            }

            _currentGameState = newState;
        }

        private void CallStateExits(GameState oldState)
        {
            oldState.OnDeactivate();
        }

        private void CallFirstStateEnters(GameState firstState)
        {
            StartCoroutine(CallFirstStateEntersCoroutine(firstState));
        }

        private IEnumerator CallFirstStateEntersCoroutine(GameState firstState)
        {
            yield return new WaitForSeconds(0.5f);
            firstState.OnActivate();
            _currentGameState = firstState;
        }

        public GameState GetState(string stateName)
        {
            for (int i = 0; i < activeStates.Count; i++)
            {
                var state = activeStates[i];
                if (state.name == stateName)
                {
                    return state;
                }
            }

            return null;
        }

        public bool IsGameStarted => _currentGameState is GamePlayState;      

        public void RestartGame()
        { 
            SetState(nameof(PreGameState));           
        }     

        public void StartGame()
        {
            var scene = SceneManager.LoadSceneAsync("GameScene");
            LevelManager.Instance.ClearLevel();
            scene.completed += operation => { GameManager.Instance.SetState1(nameof(PreGameState)); };
        }   

        public async UniTask SetState1(string stateName)
        {
            await UniTask.Delay(500);
            var state = GetState(stateName);
            CallStateEnters(state);
        }
    }
}