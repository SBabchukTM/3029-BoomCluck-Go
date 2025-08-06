using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class GridCellSizeController : MonoBehaviour
{
    void Start()
    {
        AdjustCellSize();
    }

    void OnRectTransformDimensionsChange()
    {
        AdjustCellSize();
    }
    
    void AdjustCellSize()
    {
        var gridLayout = GetComponent<GridLayoutGroup>();
        if(gridLayout.constraint == GridLayoutGroup.Constraint.Flexible)
            return;
        
        int constraintCount = gridLayout.constraintCount;
        var rectTransform = GetComponent<RectTransform>();
        
        float parentWidth = rectTransform.rect.width;
        float spacingTotal = gridLayout.spacing.x * (constraintCount - 1);
        float paddingTotal = gridLayout.padding.left + gridLayout.padding.right;
        
        float aspectRatio = gridLayout.cellSize.y / gridLayout.cellSize.x;
        float cellWidth = (parentWidth - spacingTotal - paddingTotal) / constraintCount;
        float cellHeight = cellWidth * aspectRatio;
        
        gridLayout.cellSize = new Vector2(cellWidth, cellHeight);
    }
}
