using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace States
{
    public abstract class GameState : MonoBehaviour
    {
        public bool startDisabled = false;

        public void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.UnSubscribeStateListener(this);
            }
        }

        protected string Name { get; set; }

        public List<GameState> ActiveStates = new List<GameState>();

        protected virtual void Awake()
        {
            GameManager.Instance.SubscribeStateListener(this);
            if (startDisabled) gameObject.SetActive(false);
        }

        protected abstract void SetName();
        public abstract void OnActivate();
        public abstract void OnDeactivate();
        public abstract void OnUpdateState();

        public override string ToString()
        {
            return this.GetType().ToString();
        }
    }
}