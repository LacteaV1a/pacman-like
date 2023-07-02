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

    public T GetValue(int x, int y)
    {
        return _gridArray[x, y];
    }

    public void Clear()
    {
        _gridArray = new T[_width, _height];
    }
}
