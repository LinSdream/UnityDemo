using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Souls
{
    /// <summary>
    /// 导演管理器，控制由Timeliness来控制的动画
    /// </summary>
    [RequireComponent(typeof(PlayableDirector))]
    public class DirectorManager : AbstractActorManager
    {
        public PlayableDirector Director;

        [Header("Timeline Asstes")]
        public TimelineAsset FrontStab;

        [Header("Assets Settings")]
        public ActorManager Attacker;
        public ActorManager Victim;

        private void Start()
        {
            Director = GetComponent<PlayableDirector>();
            Director.playOnAwake = false;
            //Director.playableAsset = FrontStab;
        }

        private void Update()
        {
            //if (Input.GetKey(KeyCode.H))
            //{
            //    Debug.Log("!!!!!!!!!!");
            //    Director.Play();
            //}

        }

        /// <summary>
        /// 弹反的动画播放
        /// </summary>
        /// <param name="timelineName">timeliness名</param>
        /// <param name="attacker">攻击者</param>
        /// <param name="virctiom">受击者</param>
        public void Play(string timelineName, ActorManager attacker, ActorManager virctiom)
        {
            if (timelineName == "FrontStab")
            {
                Director.playableAsset = Instantiate(FrontStab);

                foreach (var cell in Director.playableAsset.outputs)
                {
                    switch (cell.streamName)
                    {
                        case "VictimScript":
                            Director.SetGenericBinding(cell.sourceObject, virctiom);
                            break;
                        case "AttackerScript":
                            Director.SetGenericBinding(cell.sourceObject, attacker);
                            break;
                        case "Animation Attacker":
                            Director.SetGenericBinding(cell.sourceObject, attacker.Controller.Anim);
                            break;
                        case "Animation Victim":
                            Director.SetGenericBinding(cell.sourceObject, virctiom.Controller.Anim);
                            break;
                    }
                }
                Director.Play();
            }
        }
    }

}