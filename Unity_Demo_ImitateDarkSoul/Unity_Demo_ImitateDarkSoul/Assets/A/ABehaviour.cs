using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Souls;

[Serializable]
public class ABehaviour : PlayableBehaviour
{
    public AbstractActorManager AM;

    public override void OnPlayableCreate (Playable playable)
    {
        
    }
}
