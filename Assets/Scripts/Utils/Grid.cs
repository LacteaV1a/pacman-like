public class Grid<T>
{
    private int _width;
    private int _height;
    private T[,] _gridArray;

    public Grid(int width, int height)
    {
        _width = width;
        _height = height;
        _gridArray = new T[width, height];
    }

    public void SetValue(int x, int y, T value)
    {
        if(x >= 0 && y >= 0 && x < _width && y < _height)
        {
            _gridArray[x, y] = value;
        }
    }

    public bool TryGetEmptyCell(out int x, out int y)
    {
        x = 0;
        y = 0;
        var empty = false;
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                if (_gridArray[i, j].Equals(default(T)))
                {
                    empty = true;
                    x = i;
                    y = j;
                    break;
                }
            }
        }
        return empty;
    }

    public T GetValue(int x, int y)
    {
        return _gridArray[x, y];
    }

    public void Clear()
    {
        _gridArray = new T[_width, _height];
    }
}
