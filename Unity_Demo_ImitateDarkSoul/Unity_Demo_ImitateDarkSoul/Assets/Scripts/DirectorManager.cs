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
        public TimelineAsset OpenBox;

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
        /// 动画播放
        /// </summary>
        /// <param name="timelineName">timeliness名</param>
        /// <param name="attacker">实施者</param>
        /// <param name="virctiom">被实施者</param>
        public void Play(string timelineName, ActorManager attacker, ActorManager virctiom)
        {
            if (Director.state==PlayState.Playing)
                return;
            switch(timelineName)
            {
                case "FrontStab":
                    Director.playableAsset = Instantiate(FrontStab);
                    SetPlayableDirector("Animation Attacker", "Animation Victim", "AttackerScript", "VictimScript", attacker, virctiom);
                    Director.Evaluate();
                    Director.Play();
                    break;
                case "Box":
                    Director.playableAsset = Instantiate(OpenBox);
                    SetPlayableDirector("Player Anim", "Box Anim", "Player Script", "Box Script", attacker, virctiom);
                    Director.Evaluate();
                    Director.Play();
                    break;
            }
        }

        void SetPlayableDirector(string attackerAnim,string virctiomAnim,string attackerScript,string virctiomScript,ActorManager attacker,ActorManager virctiom)
        {
            TimelineAsset timeline = Director.playableAsset as TimelineAsset;
            foreach (var track in timeline.GetOutputTracks())
            {

                if(track.name== virctiomScript)
                {
                    Director.SetGenericBinding(track, virctiom);
                    foreach (var cell in track.GetClips())
                    {
                        CustomStabClip clip = cell.asset as CustomStabClip;
                        CustomStabBehaviour behaviour = clip.template;
                        //对对象绑定ID，用于后面的赋值
                        clip.ActorMgr.exposedName = System.Guid.NewGuid().ToString();
                        Director.SetReferenceValue(clip.ActorMgr.exposedName, virctiom);
                    }
                }else if(track.name== attackerScript)
                {
                    Director.SetGenericBinding(track, attacker);
                    foreach (var cell in track.GetClips())
                    {
                        CustomStabClip clip = cell.asset as CustomStabClip;
                        CustomStabBehaviour behaviour = clip.template;
                        //对对象绑定ID，用于后面的赋值
                        clip.ActorMgr.exposedName = System.Guid.NewGuid().ToString();
                        Director.SetReferenceValue(clip.ActorMgr.exposedName, attacker);
                    }
                }else if(track.name== attackerAnim)
                {
                    Director.SetGenericBinding(track, attacker.Controller.Anim);
                }else if(track.name== virctiomAnim)
                {
                    Director.SetGenericBinding(track, virctiom.Controller.Anim);
                }
            }
        }
    }

}