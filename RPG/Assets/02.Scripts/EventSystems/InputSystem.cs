using RPG.Singleton;
using RPG.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.EventSystems
{
    public class InputSystem : SingletonMonoBase<InputSystem>
    {
        public class Map
        {
            public Dictionary<string, Action<float>> axesActions = new Dictionary<string, Action<float>>();
            public Dictionary<KeyCode, Action> keyDownActions = new Dictionary<KeyCode, Action>();
            public Dictionary<KeyCode, Action> keyPressActions = new Dictionary<KeyCode, Action>();
            public Dictionary<KeyCode, Action> keyUpActions = new Dictionary<KeyCode, Action>();

            public void RegisterAxisAction(string axisName, Action<float> action)
            {
                if (axesActions.ContainsKey(axisName))
                    axesActions[axisName] += action;
                else
                    axesActions.Add(axisName, action);
            }

            public void RegisterKeyDownAction(KeyCode keyCode, Action action)
            {
                if (keyDownActions.ContainsKey(keyCode))
                    keyDownActions[keyCode] += action;
                else
                    keyDownActions.Add(keyCode, action);
            }

            public void RegisterKeyPressAction(KeyCode keyCode, Action action)
            {
                if (keyPressActions.ContainsKey(keyCode))
                    keyPressActions[keyCode] += action;
                else
                    keyPressActions.Add(keyCode, action);
            }

            public void RegisterKeyUpAction(KeyCode keyCode, Action action)
            {
                if (keyUpActions.ContainsKey(keyCode))
                    keyUpActions[keyCode] += action;
                else
                    keyUpActions.Add(keyCode, action);
            }

            public void DoActions()
            {
                foreach (var pair in axesActions)
                {
                    pair.Value.Invoke(Input.GetAxis(pair.Key));
                }

                foreach (var pair in keyDownActions)
                {
                    if (Input.GetKeyDown(pair.Key))
                        pair.Value.Invoke();
                }

                foreach (var pair in keyPressActions)
                {
                    if (Input.GetKey(pair.Key))
                        pair.Value.Invoke();
                }

                foreach (var pair in keyUpActions)
                {
                    if (Input.GetKeyUp(pair.Key))
                        pair.Value.Invoke();
                }
            }
        }
        public Dictionary<string, Map> maps = new Dictionary<string, Map>();
        public string current;

        protected override void Init()
        {
            base.Init();
            Map playerMap = new Map();
            playerMap.RegisterKeyDownAction(KeyCode.I, () =>
            {
                InventoryUI ui = UIManager.instance.Get<InventoryUI>();
                if (ui.gameObject.activeSelf)
                    ui.Hide();
                else
                    ui.Show();
            });

            playerMap.RegisterKeyDownAction(KeyCode.Tab, () =>
            {
                EquippedUI ui = UIManager.instance.Get<EquippedUI>();
                if (ui.gameObject.activeSelf)
                    ui.Hide();
                else
                    ui.Show();
            });

            maps.Add("Player", playerMap);
            current = "Player";
        }

        private void Update()
        {
            if (maps.ContainsKey(current))
                maps[current].DoActions();
        }
    }
}