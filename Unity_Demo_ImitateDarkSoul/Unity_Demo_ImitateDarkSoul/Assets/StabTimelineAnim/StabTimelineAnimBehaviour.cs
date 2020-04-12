using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Souls;

[Serializable]
public class StabTimelineAnimBehaviour : PlayableBehaviour
{
    //PlayableDirector _director;
    //Quaternion q;
    //Quaternion qq;

    ////public override void OnBehaviourPlay(Playable playable, FrameData info)
    ////{
    ////    var d = playable.GetGraph().GetResolver() as PlayableDirector;
    ////    foreach (var cell in d.playableAsset.outputs)
    ////    {
    ////        if (cell.streamName == "AttackerScript" || cell.streamName == "VictimScript")
    ////        {
    ////            var am = (AnimatorEvents)_director.GetGenericBinding(cell.sourceObject);
    ////            if (am == null)
    ////                Debug.LogError("!!!!1!!ERROR");
    ////            q = am.Am.Controller.Model.transform.rotation;
    ////            Debug.Log(q.eulerAngles);
    ////            am.LockUnLockActorController(true);
    ////        }
    ////    }
    ////}

    //public override void OnPlayableCreate(Playable playable)
    //{
    //    var d = playable.GetGraph().GetResolver() as PlayableDirector;
    //    foreach (var cell in d.playableAsset.outputs)
    //    {
    //        if (cell.streamName == "AttackerScript")
    //        {
    //            var am = (AnimatorEvents)d.GetGenericBinding(cell.sourceObject);
    //            if (am != null)
    //            {

    //            q = am.Am.Controller.Model.transform.rotation;
    //            Debug.Log(q.eulerAngles);
    //            }
    //        }
    //        else if(cell.streamName == "VictimScript")
    //        {
    //            var am = (AnimatorEvents)d.GetGenericBinding(cell.sourceObject);
    //            if (am != null)
    //            { 
    //            qq = am.Am.Controller.Model.transform.rotation;
    //            Debug.Log(q.eulerAngles);
    //            }
    //        }
    //    }
    //}

    //public override void OnGraphStart(Playable playable)
    //{
    //    _director = playable.GetGraph().GetResolver() as PlayableDirector;

    //    foreach (var cell in _director.playableAsset.outputs)
    //    {
    //        if (cell.streamName == "AttackerScript")
    //        {
    //            var am = (AnimatorEvents)_director.GetGenericBinding(cell.sourceObject);
    //            if (am == null)
    //                Debug.LogError("!!!!1!!ERROR");
    //            am.Am.Controller.Model.transform.rotation = q;
    //            Debug.Log(q.eulerAngles);
    //            am.LockUnLockActorController(true);
    //        }
    //        else if (cell.streamName == "VictimScript")
    //        {
    //            var am = (AnimatorEvents)_director.GetGenericBinding(cell.sourceObject);
    //            if (am == null)
    //                Debug.LogError("!!!!1!!ERROR");
    //            am.Am.Controller.Model.transform.rotation = qq;
    //            Debug.Log(q.eulerAngles);
    //            am.LockUnLockActorController(true);
    //        }
    //    }
    //}

    //public override void OnBehaviourPlay(Playable playable, FrameData info)
    //{
    //    foreach (var cell in _director.playableAsset.outputs)
    //    {
    //        if (cell.streamName == "AttackerScript")
    //        {
    //            var am = (AnimatorEvents)_director.GetGenericBinding(cell.sourceObject);
    //            if (am == null)
    //                Debug.LogError("!!!!1!!ERROR");
    //            am.Am.Controller.Model.transform.rotation = q;
    //            Debug.Log(q.eulerAngles);
    //        }
    //        else if (cell.streamName == "VictimScript")
    //        {
    //            var am = (AnimatorEvents)_director.GetGenericBinding(cell.sourceObject);
    //            if (am == null)
    //                Debug.LogError("!!!!1!!ERROR");
    //            am.Am.Controller.Model.transform.rotation = qq;
    //            Debug.Log(q.eulerAngles);
    //        }
    //    }
    //}


    //public override void PrepareFrame(Playable playable, FrameData info)
    //{
    //    foreach (var cell in _director.playableAsset.outputs)
    //    {
    //        if (cell.streamName == "AttackerScript")
    //        {
    //            var am = (AnimatorEvents)_director.GetGenericBinding(cell.sourceObject);
    //            if (am == null)
    //                Debug.LogError("!!!!1!!ERROR");
    //            am.Am.Controller.Model.transform.rotation = q;
    //            Debug.Log(q.eulerAngles);
    //        }
    //        else if (cell.streamName == "VictimScript")
    //        {
    //            var am = (AnimatorEvents)_director.GetGenericBinding(cell.sourceObject);
    //            if (am == null)
    //                Debug.LogError("!!!!1!!ERROR");
    //            am.Am.Controller.Model.transform.rotation = qq;
    //            Debug.Log(q.eulerAngles);
    //        }
    //    }
    //}

    //public override void OnGraphStop(Playable playable)
    //{
    //    if (_director == null)
    //        return;
    //    foreach (var cell in _director.playableAsset.outputs)
    //    {
    //        if (cell.streamName == "AttackerScript" )
    //        {
    //            var am = _director.GetGenericBinding(cell.sourceObject) as AnimatorEvents;
    //            am.Am.Controller.Model.transform.rotation = q;
    //            am.LockUnLockActorController(false);
    //        }
    //        else if (cell.streamName == "VictimScript")
    //        {
    //            var am = _director.GetGenericBinding(cell.sourceObject) as AnimatorEvents;
    //            am.Am.Controller.Model.transform.rotation = qq;
    //            am.LockUnLockActorController(false);
    //        }
    //    }
    //}
}
