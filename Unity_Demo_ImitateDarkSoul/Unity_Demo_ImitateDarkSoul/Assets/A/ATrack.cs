using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Souls;

[TrackColor(0.855f, 0.8623f, 0.87f)]
[TrackClipType(typeof(AClip))]
public class ATrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<AMixerBehaviour>.Create (graph, inputCount);
    }
}
