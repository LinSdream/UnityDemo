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

        /// <summary>
        /// 弹反的动画播放
        /// </summary>
        /// <param name="timelineName">timeliness名</param>
        /// <param name="attacker">攻击者</param>
        /// <param name="virctiom">受击者</param>
        public void Play(string timelineName, ActorManager attacker, ActorManager virctiom)
        {
            if (Director.playableAsset != null)
                return;

            if (timelineName == "FrontStab")
            {
                Director.playableAsset = Instantiate(FrontStab);

                TimelineAsset timeline = Director.playableAsset as TimelineAsset;
                foreach (var track in timeline.GetOutputTracks())
                {
                    switch (track.name)
                    {
                        case "VictimScript": 
                            Director.SetGenericBinding(track, virctiom); 
                            foreach(var cell in track.GetClips())
                            {
                                CustomStabClip clip = cell.asset as CustomStabClip;
                                CustomStabBehaviour behaviour = clip.template;
                                Director.SetReferenceValue(clip.ActorMgr.exposedName, virctiom);
                            }
                            break;
                        case "AttackerScript": 
                            Director.SetGenericBinding(track, attacker); 
                            foreach(var cell in track.GetClips())
                            {
                                CustomStabClip clip = cell.asset as CustomStabClip;
                                CustomStabBehaviour behaviour = clip.template;
                                Director.SetReferenceValue(clip.ActorMgr.exposedName, attacker);
                            }
                            break;
                        case "Animation Attacker": Director.SetGenericBinding(track, attacker.Controller.Anim); break;
                        case "Animation Victim": Director.SetGenericBinding(track, virctiom.Controller.Anim); break;
                    }
                }

                Director.Evaluate();
                Director.Play();
            }
        }
    }

}