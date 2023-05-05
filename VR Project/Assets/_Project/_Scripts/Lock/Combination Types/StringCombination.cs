using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Combination", menuName = "Lock/Combination")]
public class StringCombination : ScriptableObject
{
    [SerializeField] private string combination;
    public string GetCombination() => combination;
}
