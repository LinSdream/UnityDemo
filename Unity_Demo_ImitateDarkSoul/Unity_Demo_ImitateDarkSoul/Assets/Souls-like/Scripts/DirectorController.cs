using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Souls
{
    public class DirectorController : MonoBehaviour
    {
        public PlayableDirector Director;

        public Animator Attacker;
        public Animator Victim;


        public void Play()
        {

            foreach(var cell in Director.playableAsset.outputs)
            {
                if (cell.streamName == "Animation Attacker")
                    Director.SetGenericBinding(cell.sourceObject, Attacker);
                else if (cell.streamName == "Animation Victim")
                    Director.SetGenericBinding(cell.sourceObject, Victim);
            }

            Director.time = 0;
            Director.Stop();
            Director.Evaluate();
            Director.Play();
        }
    }

}