using Souls.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Souls
{
    public class RuntimeConsole : MonoBehaviour
    {
        public const string PlayerCommand = "player";
        public const string EnemyCommand = "enemy";
        public const string BossCommand = "boss";

        public InputField IF;
        bool _active = false;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                if (_active)
                {
                    IF.text = "";
                    IF.gameObject.SetActive(false);
                }
                else
                {
                    IF.gameObject.SetActive(true);
                    IF.ActivateInputField();
                }
            }
        }

        public void OnIFNameEndEdit()
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                if (IF.text == "")
                {
                    IF.ActivateInputField();
                }
                else
                {
                    GetCommnand(IF.text);
                }
                IF.gameObject.SetActive(false);
            }
        }

        public void GetCommnand(string cmd)
        {
            var cmds = cmd.Split(' ');
            if ( cmds.Length < 0)
                LogError("不存在该指令");
            Debug.Log(cmds.Length);
            Action error = () => LogError("不存在该指令");
            switch (cmds.Length)
            {
                case 3:
                    if (cmds[0] == PlayerCommand)
                    {
                        var pc = FindObjectOfType<PlayerController>();
                        switch (cmds[1])
                        {

                            case "run":
                                TryDo(() => { float value = float.Parse(cmds[2]); pc.RunMultiplier = value; }, error);
                                break;
                            case "attack":
                                TryDo(() => { float value = float.Parse(cmds[2]); pc.Info.Damage = value; }, error);
                                break;
                            case "add_hp":
                                TryDo(() => { float value = float.Parse(cmds[2]); pc.GetComponent<ActorManager>().SM.AddHP(value); }, error);
                                break;
                        }
                    }
                    break;
                case 4:
                    if (cmds[0] == BossCommand)
                    {
                        BossActorManager am = FindObjectOfType<BossActorManager>();
                        switch (cmds[1])
                        {
                            case "freq":
                                TryDo(() =>
                                {
                                    var value1 = float.Parse(cmds[2]);
                                    var value2 = float.Parse(cmds[3]);
                                    am.GetComponent<BossFSM>().BossCol.Freq = new Vector2(value1, value2);
                                }, error);
                                break;
                        }
                    }
                    break;
            }
        }

        public void LogError(string str)
        {

        }

        public void TryDo(Action tryDo, Action catchDo)
        {
            try
            {
                tryDo?.Invoke();
            }
            catch
            {
                catchDo?.Invoke();
            }
        }

    }
}
