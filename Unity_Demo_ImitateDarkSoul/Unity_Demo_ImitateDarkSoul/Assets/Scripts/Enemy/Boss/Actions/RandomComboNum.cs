﻿using LS.Common;
using LS.Test.AI;
using LS.Test.AI.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls.AI
{
    [CreateAssetMenu(menuName = "Souls/EnemyAI/Actions/Boss/RandomComboNum")]
    public class RandomComboNum : Action
    {

        public BossAttackWeighted ShortWeighted;
        public BossAttackWeighted MidWeighted;
        public BossAttackWeighted LongWeighted;

        public override void Act(FSMBase controller)
        {
            var fsm = controller as BossFSM;
            //按照行为频率来调整攻击ComboNum;
            string weighted;
            switch (fsm.DistanceType)
            {
                case BossFSM.Distance.Short:

                    weighted = SharedMethods.GetWeightedRandomRes(ShortWeighted.Weighted).WeightedName;
                    for(int i=0;i < ShortWeighted.Weighted.Length;i++)
                    {
                        if(weighted==ShortWeighted.Weighted[i].WeightedName)
                        {
                            fsm.BossCombo = ShortWeighted.Weighted[i].ComboNumber;
                            return;
                        }
                    }
                    fsm.BossCombo = 1;
                    break;
                case BossFSM.Distance.Mid:

                     weighted = SharedMethods.GetWeightedRandomRes(MidWeighted.Weighted).WeightedName;
                    for (int i = 0; i < ShortWeighted.Weighted.Length; i++)
                    {
                        if (weighted == ShortWeighted.Weighted[i].WeightedName)
                        {
                            fsm.BossCombo = ShortWeighted.Weighted[i].ComboNumber;
                            return;
                        }
                    }
                    fsm.BossCombo = 1;
                    break;
                case BossFSM.Distance.Long:

                     weighted = SharedMethods.GetWeightedRandomRes(LongWeighted.Weighted).WeightedName;
                    for (int i = 0; i < ShortWeighted.Weighted.Length; i++)
                    {
                        if (weighted == ShortWeighted.Weighted[i].WeightedName)
                        {
                            fsm.BossCombo = ShortWeighted.Weighted[i].ComboNumber;
                            return;
                        }
                    }
                    fsm.BossCombo = 1;
                    break;
                case BossFSM.Distance.VLong:
                case BossFSM.Distance.None:
                    fsm.BossCombo = 0;
                    break;
            }

            if (fsm.BehaviourTimer >= fsm.CurrentBehaviourFrequency)//这里的计时器已经有State来计算了，所以不需要再次计算
            {
                //switch (SharedMethods.GetWeightedRandomRes(Weighted.Weighted).WeightedName)
                //{
                //    case "Attack1": fsm.BossCombo = 1; break;
                //    case "Attack2": fsm.BossCombo = 2; break;
                //    case "Attack3": fsm.BossCombo = 7; break;
                //    case "Attack4": fsm.BossCombo = 3; break;
                //    case "Attack5": fsm.BossCombo = 6; break;
                //    case "Speical1":
                //        if (fsm.DistanceType != BossFSM.Distance.Short)
                //            fsm.BossCombo = 1;
                //        else
                //            fsm.BossCombo = 4;//近距离开炮
                //        break;
                //    default: fsm.BossCombo = 0; break;
                //};
                return;
            }
        }
    }
}