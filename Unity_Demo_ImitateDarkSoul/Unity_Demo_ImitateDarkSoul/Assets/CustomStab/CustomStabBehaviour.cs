using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Souls;

[Serializable]
public class CustomStabBehaviour : PlayableBehaviour
{

    PlayableDirector _director;

    public override void OnGraphStart(Playable playable)
    {
        _director = playable.GetGraph().GetResolver() as PlayableDirector;
        Debug.Log("!");
        foreach(var cell in _director.playableAsset.outputs)
        {
            if(cell.streamName== "AttackerScript" || cell.streamName == "VictimScript")
            {
                var am = (ActorManager)_director.GetGenericBinding(cell.sourceObject);
                if (am == null)
                    Debug.LogError("!!!!1!!ERROR");
                am.LockUnLockActorController(true);
            }
        }
    }

    public override void OnGraphStop(Playable playable)
    {
        if (_director == null)
            return;
        foreach (var cell in _director.playableAsset.outputs)
        {
            if (cell.streamName == "AttackerScript" || cell.streamName == "VictimScript")
            {
                var am = _director.GetGenericBinding(cell.sourceObject) as ActorManager;
                am.LockUnLockActorController(false);
            }
        }
    }
}
