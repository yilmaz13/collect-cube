using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Managers;

public class EnemyController : ArcController
{
    Transform spawnPoint;
    public bool moveable = true;

    public CubeControlller TargetCubeControlller;
    public List<CubeControlller> PossibleCubes = new List<CubeControlller>();


    public void Start()
    {
        var data = Resources.Load<CollecterData>("ScriptableObjects/CollecterData/EnemyData");
        antiMultiplier = data.antiMultiplier;
        speed = data.speed;
    }
    private void Update()
    {
        if (moveable)
        {
            Sence();
            Think();
            _ = MoveSystem();
        }
    }
    protected void Sence()
    {
        //find possible targets
        PossibleCubes.Clear();
        PossibleCubes.Add(LevelManager.Instance.RandomCube());
        PossibleCubes.Add(LevelManager.Instance.RandomCube());
        PossibleCubes.Add(LevelManager.Instance.RandomCube());
    }

    protected void Think()
    {
        //Choose target
        var index = Random.Range(0, PossibleCubes.Count);
        TargetCubeControlller = PossibleCubes[index];
    }

    protected override void Move()
    {
        _ = MoveSystem();
    }
    protected void Move(Vector3 vector, float time)
    {
        transform.DOMove(vector, time).SetEase(Ease.Flash); ;
    }

    [ContextMenu("Move")]
    protected async UniTask MoveSystem()
    {
        moveable = false;
        await UniTask.Delay(500);
        var targetVector = TargetCubeControlller.GetPosition();

        var distance = Vector3.Distance(targetVector, transform.position);

        transform.DOLookAt(targetVector, 0.4f);
        await UniTask.Delay(400);
        Move(targetVector, distance / antiMultiplier);
        await UniTask.Delay((int)distance * 1000 / antiMultiplier);
        transform.DOLookAt(spawnPoint.position, 0.4f);
        await UniTask.Delay(400);
        Move(spawnPoint.position, distance / antiMultiplier);
        await UniTask.Delay((int)distance * 1000 / antiMultiplier);
        moveable = true;
    }

    public void SetSpawnPoint(Transform spawnPoint, Target target)
    {
        this.spawnPoint = spawnPoint;
        this.TargetManager = target;
        collisonDetect.TargetManager = target;
        moveable = true;
    }
}
