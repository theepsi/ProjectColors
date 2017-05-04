using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForSecondsRealTime : CustomYieldInstruction {

    private float _waitTime;

    public override bool keepWaiting
    {
        get { return Time.realtimeSinceStartup < _waitTime; }
    }

    public WaitForSecondsRealTime(float time)
    {
        _waitTime = Time.realtimeSinceStartup + time;
    }
}
