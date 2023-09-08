
public class ManagerPrent : Singleton<ManagerPrent>
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
}