using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class ConditionManager : MonoBehaviour
{
    [SerializeField] private List<Conditional> conditions;

    [SerializeField] private UnityEvent OnConditionMet;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var comparer in conditions)
        {
            comparer.Bind(this);
        }
    }
    
    public void CheckCondition()
    {
        if (conditions.Count == 0) return;
        if (conditions.Any(condition => !condition.isTrue))
        {
            return;
        }
        
        OnConditionMet.Invoke();
    }
    

    [Button]
    public void ForceCondition()
    {
        OnConditionMet.Invoke();
    }
}
