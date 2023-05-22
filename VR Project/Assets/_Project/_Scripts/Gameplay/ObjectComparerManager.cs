using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ObjectComparerManager : MonoBehaviour
{
    [SerializeField] private List<ObjectComparer> _comparers;

    [SerializeField] private UnityEvent OnConditionMet;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var comparer in _comparers)
        {
            comparer.Bind(this);
        }
    }
    
    public void CheckCondition()
    {
        if (_comparers.Count == 0) return;
        
        if (_comparers.Any(comparer => !comparer.isTrue))
        {
            return;
        }
        
        OnConditionMet.Invoke();
    }
}
