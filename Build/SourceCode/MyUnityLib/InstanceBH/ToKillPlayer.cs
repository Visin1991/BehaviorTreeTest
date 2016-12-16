using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeiBHTLibrary;

public class ToKillPlayer : Sequence {

    TreeBase treeBase;

    int testIndex;

    public ToKillPlayer(TreeBase _treeBase) {
        treeBase = _treeBase;

        Add<Behavior>().Update = IsFindPlayer;

        Selector makeStrategy = Add<Selector>();
        {
            PrioritySequence fight = makeStrategy.Add<PrioritySequence>();
            {
                fight.Add<Condition>().CanPass = IsHealthLow;
                fight.Add<Behavior>().Update = KillingPlayer;
            }

            makeStrategy.Add<Behavior>().Update = Hide;
        }
    }

    Status IsFindPlayer() {
        if (treeBase.isFindPlayer)
        {
            return Status.BhSuccess;
        }
        else {
            treeBase.aiStatu = TreeBase.AiStatu.Partoal;
            return Status.BhFailure;
        }
    }
    
    bool IsHealthLow() {
        if (treeBase.ds.hp > 50)
            return true;
        else
            return false;
    }

    Status KillingPlayer() {
        if (treeBase.numberFrame % 30 == 0)
        {
            treeBase.aiStatu = TreeBase.AiStatu.Fight;
            treeBase.Fire();
        }
        return Status.BhRunning;
    }

    Status Hide() {
        if (treeBase.ds.hp <= 50)
        {
            treeBase.aiStatu = TreeBase.AiStatu.Hide;
            return Status.BhRunning;
        }
        else
            return Status.BhFailure;

    }
}
