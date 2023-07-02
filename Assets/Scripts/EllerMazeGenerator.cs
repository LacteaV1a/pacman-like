using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EllerMazeGenerator : MonoBehaviour
{
    [SerializeField] private int _cols;
    [SerializeField] private int _rows;
    [SerializeField] private int _seed;
    private List<int> _sideLine = new List<int>();
    private bool[,] _verticalWalls;
    private bool[,] _horizontallWalls;

    private int _counter = 1;
    private System.Random _random;
    private void Awake()
    {
        _random = new System.Random(_seed);
        _verticalWalls = new bool[_rows, _cols];
        _horizontallWalls = new bool[_rows, _cols];
        Generate();
    }

    [ContextMenu("Generate")]
    public void Generate()
    {
        FillEmptyValue();
        for (int i = 0; i < _rows - 1; i++)
        {
            AssignUniqueSet();
            AddingVerticalWalls(i);
            AddingHorizotalWalls(i);
            CheckedHorizontalWalls(i);

            PreparatingNewLine(i);
        }
        AddingEndLine();

            string line = "";
        for (int i = 0; i < _cols; i++)
        {
            for (int j = 0; j < _rows; j++)
                line += _verticalWalls[i, j] ? 1 : 0;
            line += "\n";
        }
            Debug.Log(line);

    }

    private void FillEmptyValue()
    {
        for (int i = 0; i < _cols; i++)
        {
            _sideLine.Add(0);
        }
    }

    private void AssignUniqueSet()
    {
        for (int i = 0; i < _cols; i++)
        {
            if(_sideLine[i] == 0)
            {
                _sideLine[i] = _counter;
                _counter++;
            }
        }
    }

    private void AddingVerticalWalls(int row)
    {
        for (int i = 0; i < _cols - 1; i++)
        {
            bool choise = _random.NextDouble() > 0.5f;

            if(choise || _sideLine[i] == _sideLine[i + 1])
            {
                _verticalWalls[row, i] = true;
            }
            else
            {
                MergeSet(i, _sideLine[i]);
            }
        }

        _verticalWalls[row, _cols - 1] = true;
    }

    private void AddingHorizotalWalls(int row)
    {
        for (int i = 0; i < _cols; i++)
        {
            bool choise = _random.NextDouble() > 0.5f;

            if(CalculateUniqueSet(_sideLine[i]) != 1 && choise)
            {
                _horizontallWalls[row, i] = true;
            }
        }
    }

    private int CalculateUniqueSet(int element)
    {
        int countUniqSet = 0;
        for (int i = 0; i < _cols; i++)
        {
            if(_sideLine[i] == element)
            {
                countUniqSet++;
            }
        }
        return countUniqSet;
    }

    private void CheckedHorizontalWalls(int row)
    {
        for (int i = 0; i < _cols; i++)
        {
            if(CalculateHorizontalWalls(_sideLine[i], row) == 0)
            {
                _horizontallWalls[row, i] = false;
            }
        }
    }

    private int CalculateHorizontalWalls(int element, int row)
    {
        int countHorizontalWalls = 0;
        for (int i = 0; i < _cols; i++)
        {
            if(_sideLine[i] == element && _horizontallWalls[row, i] == false)
            {
                countHorizontalWalls++;
            }
        }
        return countHorizontalWalls;
    }

    private void MergeSet(int index, int element)
    {
        int mutabelSet = _sideLine[index + 1];
        for (int j = 0; j < _cols; j++)
        {
            if(_sideLine[j] == mutabelSet)
            {
                _sideLine[j] = element;
            }
        }
    }

    private void PreparatingNewLine(int row)
    {
        for (int i = 0; i < _cols; i++)
        {
            if(_horizontallWalls[row, i])
            {
                _sideLine[i] = 0;
            }
        }
    }

    private void AddingEndLine()
    {
        AssignUniqueSet();
        AddingVerticalWalls(_rows - 1);
        CheckedEndLine();
    }

    private void CheckedEndLine()
    {
        for (int i = 0; i < _cols - 1; i++)
        {
            if(_sideLine[i] != _sideLine[i + 1])
            {
                _verticalWalls[_rows - 1, i] = false;
                MergeSet(i, _sideLine[i]);
            }

            _horizontallWalls[_rows - 1, i] = true;
        }
        _horizontallWalls[_rows - 1, _cols - 1] = true;
    }
}
