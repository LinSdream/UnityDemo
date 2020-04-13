using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Souls;

[Serializable]
public class AClip : PlayableAsset, ITimelineClipAsset
{
    public ABehaviour template = new ABehaviour ();
    public ExposedReference<AbstractActorManager> AM;

    public ClipCaps clipCaps
    {
        get { return ClipCaps.None; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<ABehaviour>.Create (graph, template);
        ABehaviour clone = playable.GetBehaviour ();
        clone.AM = AM.Resolve (graph.GetResolver ());
        return playable;
    }
}
