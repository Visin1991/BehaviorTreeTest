using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using WeiBHTLibrary;

public class LookAround : Behavior{

    TreeBase treeBase;

    UInt64 numberFrame = 0;
    List<string> allTags;
    List<Collider> colliders;

    public LookAround(TreeBase _treeBase) {
        treeBase = _treeBase;
        Update = ProcessViewInfo;
    }

    Status ProcessViewInfo()
    {
        if (numberFrame % 30 == 0) //we Look around each time 30 frame.
        {
            allTags = new List<string>();
            colliders = treeBase.fieldOfView.GetAllColliderInsideFieldOfView();
            int playerNumber =0;
            foreach (Collider c in colliders)
            {
               allTags.Add(c.transform.root.tag);
                if (c.transform.root.tag == "Player")
                {
                    playerNumber++;
                    treeBase.player = c.gameObject;
                }
            }
       
            if (playerNumber > 0)
            {
                treeBase.isFindPlayer = true;
            }
            else {
                treeBase.isFindPlayer = false;
            }

            treeBase.viewTags = allTags;
        }

        numberFrame++;
        return Status.BhRunning;
    }
}
