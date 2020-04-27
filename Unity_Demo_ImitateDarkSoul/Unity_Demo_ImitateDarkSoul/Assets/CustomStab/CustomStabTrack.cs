using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Souls;

[TrackColor(1f, 0.5408292f, 0f)]
[TrackClipType(typeof(CustomStabClip))]
[TrackBindingType(typeof(ActorManager))]
public class CustomStabTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<CustomStabMixerBehaviour>.Create (graph, inputCount);
    }

}
