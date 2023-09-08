using UnityEngine;
public enum ControllerType
{
    player,
    enemy
}
public abstract class ArcController : MonoBehaviour
{
    #region Variables

    public float speed;
    public int antiMultiplier = 0;

    protected Vector3 moveDirection;
    protected Vector2 direction;
    protected ControllerType controllerType;

    public PlayerCollisonDetect collisonDetect;
    public Target TargetManager;

    #endregion

    #region Unity Methods
    protected void Update()
    {
        Move();
    }

    #endregion

    #region Abstract Methods
    protected abstract void Move();

    #endregion


}
