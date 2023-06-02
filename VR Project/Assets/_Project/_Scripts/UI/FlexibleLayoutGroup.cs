using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UI;

public class FlexibleLayoutGroup : LayoutGroup
{
    public enum FitType
    {
        Uniform,
        Width,
        Height,
        FixedRows,
        FixedColumns
    }
    
    public FitType fitType;
    public int rows;
    public int columns;
    
    public Vector2 cellSize;
    public Vector2 spacing;
    
    public bool fitX, fitY;
    
    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();

        if (fitType is FitType.Width or FitType.Height or FitType.Uniform)
        {
            fitX = true;
            fitY = true;
            
            float sqrRt = Mathf.Sqrt(transform.childCount);
            rows = Mathf.CeilToInt(sqrRt);
            columns = Mathf.CeilToInt(sqrRt);
        }

        switch (fitType)
        {
            case FitType.Width or FitType.FixedColumns:
                rows = Mathf.CeilToInt(transform.childCount / (float)columns);
                break;
            case FitType.Height or FitType.FixedRows:
                columns = Mathf.CeilToInt(transform.childCount / (float)rows);
                break;
        }

        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        float cellWidth = (parentWidth - padding.left - padding.right - spacing.x * (columns - 1)) / columns;
        float cellHeight = (parentHeight - padding.top - padding.bottom - spacing.y * (rows - 1)) / rows;

        cellSize.x = fitX ? cellWidth : cellSize.x;
        cellSize.y = fitY ? cellHeight : cellSize.y;

        for (int i = 0; i < rectChildren.Count; i++)
        {
            int rowCount = i / columns;
            int columnCount = i % columns;

            RectTransform item = rectChildren[i];

            float xPos = cellSize.x * columnCount + spacing.x * columnCount + padding.left;
            float yPos = cellSize.y * rowCount + spacing.y * rowCount + padding.top;
            
            SetChildAlongAxis(item, 0, xPos, cellSize.x);
            SetChildAlongAxis(item, 1, yPos, cellSize.y);
            
        }
    }

    public override void CalculateLayoutInputVertical()
    {
        
    }

    public override void SetLayoutHorizontal()
    {
        
    }

    public override void SetLayoutVertical()
    {
        
    }
}
