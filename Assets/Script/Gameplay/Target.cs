using UnityEngine;

public class Target : MonoBehaviour
{
    private ControllerType controllerType;
    public ControllerType ControllerType => controllerType;

    public void SetTarget(ControllerType controllerType)
    {
        this.controllerType = controllerType;
    }
}
