using UnityEngine;

public class PoolObject : MonoBehaviour
{
    #region Variables

    [SerializeField] private bool poolAfterDelay;
    [SerializeField] private float delay = 0;
    [SerializeField] private string objectName;

    protected Transform parent;

    public Transform Parent
    {
        get => parent;
        set => parent = value;
    }

    public string PrefabName
    {
        get => objectName;
        set => objectName = value;
    }
    #endregion

    #region Public Methods

    public virtual void GoToPool(float delay)
    {
        //TODO: Use UniTask
        Invoke(nameof(GoToPool), delay);
    }

    public virtual void GoToPool()
    {
        transform.SetParent(parent);
        gameObject.SetActive(false);

        IPoolable[] components = GetComponents<IPoolable>();
        foreach (IPoolable poolable in components)
            poolable.OnReturnPool();
    }

    public virtual void PoolSpawn()
    {
        IPoolable[] components = GetComponents<IPoolable>();
        foreach (IPoolable poolable in components)
            poolable.OnPoolSpawn();

        if (poolAfterDelay)
            Invoke(nameof(GoToPool), delay);   
    }

    public void ManuelPoolAfterDelay(float delay)
    {
        CancelInvoke();
        Invoke(nameof(GoToPool), delay);
    }

    #endregion
   
    [ContextMenu("GotoPool")]
    public void GotoPoolTest()
    {
        GoToPool();
    }
}