using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.UI;

public class GridLayoutSetupController
{
    private readonly ISettingProvider _settingProvider;

    public GridLayoutSetupController(ISettingProvider settingProvider)
    {
        _settingProvider = settingProvider;
    }    
    public void AdjustGridLayout(GridLayoutGroup gridLayoutGroup, int elements)
    {
        var gridSetup = _settingProvider.Get<GridLayoutSetup>();

        switch (elements)
        {
            case 6:
                AdjustGridLayout(gridLayoutGroup, gridSetup.SixElementsGrid);
                break;
            case 8:
                AdjustGridLayout(gridLayoutGroup, gridSetup.EightElementsGrid);
                break;
            case 10:
                AdjustGridLayout(gridLayoutGroup, gridSetup.TenElementsGrid);
                break;
            case 12:
                AdjustGridLayout(gridLayoutGroup, gridSetup.TwelveElementsGrid);
                break;
            case 14:
                AdjustGridLayout(gridLayoutGroup, gridSetup.FourteenElementsGrid);
                break;
            case 16:
                AdjustGridLayout(gridLayoutGroup, gridSetup.SixteenElementsGrid);
                break;
            case 18:
                AdjustGridLayout(gridLayoutGroup, gridSetup.EighteenElementsGrid);
                break;
            case 20:
                AdjustGridLayout(gridLayoutGroup, gridSetup.TwelveElementsGrid);
                break;
            default:
                Debug.LogError($"No setup for {elements} elements");
                break;
        }
    }

    private void AdjustGridLayout(GridLayoutGroup gridLayoutGroup, GridSetup gridSetup)
    {
        gridLayoutGroup.cellSize = new Vector2(gridSetup.CellSize, gridSetup.CellSize);
        gridLayoutGroup.spacing = new Vector2(gridSetup.Spacing, gridSetup.Spacing);

        if (gridSetup.ConstRows)
        {
            gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            gridLayoutGroup.constraintCount = gridSetup.Rows;
        }
        else if (gridSetup.ConstColumns)
        {
            gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayoutGroup.constraintCount = gridSetup.Columns;
        }
    }
}
