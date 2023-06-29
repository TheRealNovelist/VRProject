using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SearchComponentMode
{
    Any,
    IncludeChild,
    IncludeParent
}

public static class Extension
{
    public static void RotateTowards(this Transform transform, Transform target, float turnSpeed = 1f, bool freezeX = false, bool freezeY = false, bool freezeZ = false)
    {
        Quaternion tmpRotation = transform.localRotation;
        Vector3 targetPointTurret = (target.transform.position - transform.position).normalized;
        Quaternion targetRotationTurret = Quaternion.LookRotation(targetPointTurret, transform.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotationTurret, turnSpeed);

        float localX = freezeX ? tmpRotation.eulerAngles.x : transform.localRotation.eulerAngles.x;
        float localY = freezeY ? tmpRotation.eulerAngles.y : transform.localRotation.eulerAngles.y;
        float localZ = freezeZ ? tmpRotation.eulerAngles.z : transform.localRotation.eulerAngles.z;
        
        transform.localRotation = Quaternion.Euler(localX, localY, localZ);
    }
    
    public static bool TryGetComponent<T>(this GameObject gameObject, SearchComponentMode mode, out T component)
    {
        var tempComp = gameObject.GetComponent<T>();
        if (tempComp != null)
        {
            component = tempComp;
            return true;
        }
        
        if (mode is SearchComponentMode.Any or SearchComponentMode.IncludeParent)
        {
            tempComp = gameObject.GetComponentInParent<T>();
            if (tempComp != null)
            {
                component = tempComp;
                return true;
            }
        }

        if (mode is SearchComponentMode.Any or SearchComponentMode.IncludeChild)
        {
            tempComp = gameObject.GetComponentInChildren<T>();
            if (tempComp != null)
            {
                component = tempComp;
                return true;
            }
        }

        component = default;
        return false;
    }

    public static Vector3 GetClosestPoint(this Transform transform, Vector3 position)
    {
        var root = transform.root;

        List<Collider> colliders = root.GetComponents<Collider>().ToList();

        colliders = colliders.Union(root.GetComponentsInChildren<Collider>().ToList()) as List<Collider>;

        float closestDist = Mathf.Infinity;
        Vector3 closestPoint = transform.position;

        if (colliders == null) return transform.position;
        foreach (var collider in colliders)
        {
            var point = collider.ClosestPoint(position);
            var dist = Vector3.Distance(position, point);
            if (dist < closestDist)
            {
                closestPoint = point;
                closestDist = dist;
            }
        }

        return closestPoint;
    }   
}