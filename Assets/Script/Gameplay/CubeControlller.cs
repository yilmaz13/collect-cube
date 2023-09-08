using UnityEngine;
using DG.Tweening;
using GameCore;
using Managers;

public class CubeControlller : MonoBehaviour, IPoolable
{

    public Transform Target;
    public Target TargetManager;
    public bool active;
    public Material[] material;
    public PrefabType Color;
    public int Score;

    #region Unity Methods
    void Update()
    {
        if (TargetManager == null)
        {
            return;
        }
        if (Vector3.Distance(TargetManager.gameObject.transform.position, transform.position) < 1.75f && active)
        {
            CollectedCube();
        }
    }
    #endregion


    public void SetCube(Transform target, bool isActive, PrefabType color, int score)
    {
        Target = target;
        active = isActive;
        Color = color;
        Score = score;
        GetComponent<MeshRenderer>().sharedMaterial = material[(int)color];
    }

    private void SetTarget(Target target)
    {
        TargetManager = target;
    }

    private void RemoveTarget()
    {
        TargetManager = null;
    }

    public void CollectedCube()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<BoxCollider>().enabled = false;
        LevelManager.Instance.UnSubscribeCubeListener(this);
        LevelManager.Instance.SubscribeCubesCollectedListener(this);
        active = false;
        FlyTarget();
        LevelManager.Instance.CheckLevel();
    }
    public void FlyTarget()
    {
        transform.DOMove(TargetManager.gameObject.transform.position, 0.5f).SetEase(Ease.Flash);
        GetComponent<MeshRenderer>().sharedMaterial = material[8];
    }

    public ControllerType GetType()
    {
        return TargetManager.ControllerType;
    }
    public Vector3 GetPosition()
    {
        return transform.position;
    }
    public void OnReturnPool()
    {
        GetComponent<MeshRenderer>().sharedMaterial = material[7];
        transform.rotation = Quaternion.identity;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<BoxCollider>().enabled = true;
    }

    public void OnPoolSpawn()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerCollisonDetect>(out PlayerCollisonDetect collisonDetect))
        {
            if (collisonDetect.TargetManager != null)
            {
                TargetManager = collisonDetect.TargetManager;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerCollisonDetect>(out PlayerCollisonDetect collisonDetect))
        {
            if (collisonDetect.TargetManager != null)
            {
                TargetManager = null;
            }
        }
    }
}
