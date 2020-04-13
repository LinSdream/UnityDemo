using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Souls;

[Serializable]
public class CustomStabBehaviour : PlayableBehaviour
{
    public ActorManager ActorMgr;
    PlayableDirector _director;


    public override void OnGraphStart(Playable playable)
    {
        _director = playable.GetGraph().GetResolver() as PlayableDirector;
    }

    public override void OnGraphStop(Playable playable)
    {
        if (_director != null)
            _director.playableAsset = null;
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if (ActorMgr == null)
            Debug.LogWarning("CustomStabTrack PrepareFrame ActorManager is null");
        ActorMgr.LockUnLockActorController(false);
    }

    public override void PrepareFrame(Playable playable, FrameData info)
    {
        if (ActorMgr == null)
            Debug.LogWarning("CustomStabTrack PrepareFrame ActorManager is null");
        ActorMgr.LockUnLockActorController(true);
    }

}
