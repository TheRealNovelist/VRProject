using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class LightsOutLock : Lock
{
    [TableMatrix(HorizontalTitle = "Lights", SquareCells = true)]
    public readonly bool[,] lights = new bool[3, 3];

    public void ToggleLight(Vector2Int pos)
    {
        if (isTrue) return;

        for (int dx = -1; dx <= 1; dx++)
        for (int dy = -1; dy <= 1; dy++)
        {
            Vector2Int intPos = new(dx, dy);

            if (intPos != Vector2Int.down && 
                intPos != Vector2Int.up && 
                intPos != Vector2Int.left &&
                intPos != Vector2Int.right &&
                intPos != Vector2Int.zero) continue;
            
            Vector2Int adjPos = new(pos.x + dx, pos.y + dy);

            if (IsValid(adjPos))
            {
                lights[adjPos.x, adjPos.y] = !lights[adjPos.x, adjPos.y];
            }
        }
        
        CheckAllLight();
    }

    public void CheckAllLight()
    {
        foreach (bool lightVar in lights)
        {
            if (lightVar == false)
            {
                return;
            }
        }
        
        Invoke(true);
    }

    public bool IsValid(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < lights.GetLength(0) && pos.y >= 0 && pos.y < lights.GetLength(1);
    }
}
