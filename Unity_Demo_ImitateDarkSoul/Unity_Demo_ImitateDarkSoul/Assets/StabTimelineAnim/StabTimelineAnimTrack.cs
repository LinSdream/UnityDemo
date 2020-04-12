using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Souls;

[TrackColor(0f, 0.5f, 1f)]
[TrackClipType(typeof(StabTimelineAnimClip))]
[TrackBindingType(typeof(AnimatorEvents))]
public class StabTimelineAnimTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<StabTimelineAnimMixerBehaviour>.Create (graph, inputCount);
    }
}
