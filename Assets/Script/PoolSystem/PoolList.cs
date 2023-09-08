using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TemplatePool", menuName = "ScriptableObjects/TemplatePool", order = 1)]
public class PoolList : ScriptableObject
{
    public List<Pool> pools;
}