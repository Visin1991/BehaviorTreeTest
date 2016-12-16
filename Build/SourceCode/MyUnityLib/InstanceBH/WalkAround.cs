using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using WeiBHTLibrary;

public class WalkAround : Selector {

    UInt64 numberFrame = 0;
    TreeBase treeBase;

    bool getNewSearchPos = true;

    int positionIndex = 0;

    public WalkAround(TreeBase _treeBase) {
        treeBase = _treeBase;
        Add<Behavior>().Update = Patrol;
        Add<Behavior>().Update = Search;
        Add<Behavior>().Update = Fight;
        Add<Behavior>().Update = Hide;
        
    }

    Status Patrol()
    {
        if (treeBase.aiStatu == TreeBase.AiStatu.Partoal)
        {
            if (treeBase.IsArriveDestiniation())
            {
                treeBase.SetNewDestination(treeBase.partrolPos[positionIndex%treeBase.partrolPos.Length].position);
                positionIndex++;
            }
            return Status.BhRunning;
        }
        else
        {
            return Status.BhFailure;
        }
    }

    Status Search() {
        if (treeBase.aiStatu == TreeBase.AiStatu.Search)
        {
            if (IsFinishSearch())
            {
                return Status.BhSuccess;
            }
            else
            {
                return Status.BhRunning;
            }
        }
        return Status.BhFailure;
   }

    Status Fight()
    {
        if (treeBase.aiStatu == TreeBase.AiStatu.Fight)
        {
            if (treeBase.numberFrame % 30 == 0) {
                treeBase.navMeshAngent.SetDestination(treeBase.player.transform.position);
            }
            return Status.BhRunning;
        }
        else
            return Status.BhFailure;
    }

    Status Hide() {
        if (treeBase.aiStatu == TreeBase.AiStatu.Hide)
        {
            treeBase.navMeshAngent.SetDestination(treeBase.hidePos.position);
            return Status.BhRunning;
        }
        else {
            return Status.BhFailure;
        }
    }

    bool IsFinishSearch() {
        if (treeBase.IsArriveDestiniation())
        {
            return true;
        }
        else {
            return false;
        }
    }

}
