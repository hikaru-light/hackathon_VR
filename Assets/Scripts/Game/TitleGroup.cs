using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

public class TitleGroup : SubGroup
{
    [SerializeField]
    VRTeleport vrTeleport;

    [SerializeField]
    UnityEvent showCallback;

    public override void FadeCallback()
    {
        showCallback.Invoke();
        this.vrTeleport.IsTeleportLock = false;
        Timer.StartCount();
    }
}
