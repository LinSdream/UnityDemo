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
        //对对象绑定ID，用于后面的赋值
        ActorMgr.exposedName = GetInstanceID().ToString();
        clone.ActorMgr = ActorMgr.Resolve(graph.GetResolver());
        return playable;
    }
}
