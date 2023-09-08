using System;
using UnityEngine;

public class PlayerCollisonDetect : MonoBehaviour
{
    public Action Detect;
    public Action Undetect;

    public Target TargetManager;

    private void OnCollisionEnter(Collision collision)
    {
        Detect.Invoke();
    }

    private void OnCollisionExit(Collision collision)
    {
        Undetect.Invoke();
    }
}
