using UnityEngine;
using System.Collections.Generic;

public class LocationManager
{
    private uint _rowsCount;
    private uint _columnsCount;

    private float _centersHeight = 0.5F;
    private float _gapBetweenParts = 0.01F;
    private Vector3[,] _centers;

    public LocationManager(uint rowsCount, uint columnsCount)
    {
        _rowsCount = rowsCount;
        _columnsCount = columnsCount;

        CreateCenters();
    }

    public Vector3 GetLocationFor(uint row, uint column)
    {
        return _centers[row, column];
    }

    public List<Vector3> PrepareInitialPositions()
    {
        uint initialColumnsCount = _columnsCount * 5;
        uint initialRowsCount = _rowsCount / 2;

        Vector2 topLeft = new Vector2(((int)_columnsCount - initialColumnsCount) / 2, _rowsCount + 2);
        Vector2 bottomRight = new Vector2((_columnsCount + initialColumnsCount) / 2, _rowsCount + initialRowsCount + 2);

        List<Vector3> positions = new List<Vector3>();

        for (float z = topLeft.y; z <= bottomRight.y; z++)
            for (float x = topLeft.x; x <= bottomRight.x; x++)
            {
                positions.Add(new Vector3(x, _centersHeight + Random.value * 3, z));
            }

        return positions;
    }


    public bool CanStick(PuzzlePart part, uint row, uint column)
    {
        GameObject goPart = part.gameObject;
        Vector2 partPos = new Vector2(goPart.transform.position.x, goPart.transform.position.z);
        Vector2 centerPos = new Vector2(GetLocationFor(row, column).x, GetLocationFor(row, column).z);

        return (Vector2.Distance(partPos, centerPos) < 0.5F * goPart.transform.localScale.x);
    }

    public Vector3? GetStickLocation(PuzzlePart part)
    {
        GameObject goPart = part.gameObject;
        for (uint row = 0; row < _rowsCount; row++)
            for (uint column = 0; column < _columnsCount; column++)
            {
                if (CanStick(part, row, column))
                {
                    return GetLocationFor(row, column);
                }

            }
        return null;
    }

    public static Vector3 RandomRotation()
    {
        return new Vector3(0, 0, Random.Range(0, 4) * 90);
    }

    public static Vector3 NormalizedRotation(Vector3 eulerAngles)
    {
        return new Vector3(0, 0, Mathf.Round(eulerAngles.z / 90) * 90);
    }

    private void CreateCenters()
    {
        _centers = new Vector3[_rowsCount, _columnsCount];

        for (uint row = 0; row < _rowsCount; row++)
            for (uint column = 0; column < _columnsCount; column++)
            {
                _centers[row, column] = new Vector3((1F + _gapBetweenParts) * column, _centersHeight, (1F + _gapBetweenParts) * row);
            }
    }
}
