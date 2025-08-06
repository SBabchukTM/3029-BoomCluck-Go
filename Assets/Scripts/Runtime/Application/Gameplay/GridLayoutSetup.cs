using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

[CreateAssetMenu(fileName = "GridLayoutSetup", menuName = "Config/GridLayoutSetup")]
public class GridLayoutSetup : BaseSettings
{
    public GridSetup SixElementsGrid;
    public GridSetup EightElementsGrid;
    public GridSetup TenElementsGrid;
    public GridSetup TwelveElementsGrid;
    public GridSetup FourteenElementsGrid;
    public GridSetup SixteenElementsGrid;
    public GridSetup EighteenElementsGrid;
    public GridSetup TwentyElementsGrid;
}

[Serializable]
public class GridSetup
{
    public int CellSize;
    public int Spacing;
    public bool ConstRows = false;
    public int Rows = 0;
    public bool ConstColumns = false;
    public int Columns = 0;
}
