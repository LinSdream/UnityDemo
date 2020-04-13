using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Souls;

[Serializable]
public class CustomStabClip : PlayableAsset, ITimelineClipAsset
{
    public CustomStabBehaviour template = new CustomStabBehaviour ();
    public ExposedReference<ActorManager> ActorMgr;

    public ClipCaps clipCaps
    {
        get { return ClipCaps.None; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<CustomStabBehaviour>.Create (graph, template);
        CustomStabBehaviour clone = playable.GetBehaviour ();
        
        clone.ActorMgr = ActorMgr.Resolve(graph.GetResolver());
        return playable;
    }
}
