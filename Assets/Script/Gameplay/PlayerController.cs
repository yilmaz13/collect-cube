using UnityEngine;

public class PlayerController : ArcController
{
    #region Variables

    public Joystick joystick;

    #endregion

    #region Unity Methods
    private void Start()
    {
        var data = Resources.Load<CollecterData>("ScriptableObjects/CollecterData/PlayerData");
        antiMultiplier = data.antiMultiplier;
        speed = data.speed;
    }   

    private void Update()
    {
        Move();
    }

    #endregion

    #region Public Methods
    public void SetController(Joystick joystick, Target target)
    {
        this.joystick = joystick;
        this.TargetManager = target;
        collisonDetect.TargetManager = target;
    }

    protected override void Move()
    {
        Vector2 direction = joystick.direction;
        moveDirection = new Vector3(direction.x, 0, direction.y);
        Quaternion targetRotation = moveDirection != Vector3.zero ? Quaternion.LookRotation(moveDirection) : transform.rotation;
        transform.rotation = targetRotation;
        moveDirection = moveDirection * speed * antiMultiplier;
        moveDirection.Normalize();
        transform.position += moveDirection * Time.deltaTime * 5f;
    }
    #endregion
}
