using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Souls;

[Serializable]
public class StabTimelineAnimClip : PlayableAsset, ITimelineClipAsset
{
    public StabTimelineAnimBehaviour template = new StabTimelineAnimBehaviour ();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.None; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<StabTimelineAnimBehaviour>.Create (graph, template);
        StabTimelineAnimBehaviour clone = playable.GetBehaviour ();
        return playable;
    }
}
