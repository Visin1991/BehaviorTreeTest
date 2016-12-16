using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeiBHTLibrary;

public class SenseAndEmotion : Behavior {

    TreeBase treeBase;

    public SenseAndEmotion(TreeBase _treeBase) {
        treeBase = _treeBase;
        Update = ProcessInfo;
    }

    Status ProcessInfo() {
        return Status.BhRunning;
    }

}
