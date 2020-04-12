using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Souls;

public class StabTimelineAnimMixerBehaviour : PlayableBehaviour
{
    // NOTE: This function is called at runtime and edit time.  Keep that in mind when setting the values of properties.
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        AnimatorEvents trackBinding = playerData as AnimatorEvents;

        if (!trackBinding)
            return;

        int inputCount = playable.GetInputCount ();

        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<StabTimelineAnimBehaviour> inputPlayable = (ScriptPlayable<StabTimelineAnimBehaviour>)playable.GetInput(i);
            StabTimelineAnimBehaviour input = inputPlayable.GetBehaviour ();
            
            // Use the above variables to process each frame of this playable.
            
        }
    }
}
