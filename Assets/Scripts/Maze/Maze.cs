using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Maze<T>
{
    public readonly int ColumnCount;
    public readonly int RowCount;
    protected List<T> _Cells;
    public readonly int CellsSize;
    public Maze(int columnCount, int rowCount, int cellsSize)
    {
        ColumnCount = columnCount;
        RowCount = rowCount;
        CellsSize = cellsSize;
    }
    public virtual T GetCell(int x, int z)
    {

        if (x < ColumnCount && z < RowCount)
        {
            return _Cells[x + z * ColumnCount];
        }
        else
        {
            Debug.Log("Index out of range: "
                + (x < ColumnCount ?
                z.ToString() + ">=" + RowCount.ToString() :
                x.ToString() + ">=" + ColumnCount.ToString()));
            return default(T);
        }
    }

    public virtual Vector2 GetCellWorldPosition(int x, int y)
    {
        return new Vector2(x + CellsSize*0.5f, y + CellsSize*0.5f);
    }

    public virtual Vector2Int GetXY(Vector2 WorldPosition)
    {
        var x = Mathf.FloorToInt(WorldPosition.x / CellsSize);
        var y = Mathf.FloorToInt(WorldPosition.y / CellsSize);

        return new Vector2Int(x, y);

    }
}
