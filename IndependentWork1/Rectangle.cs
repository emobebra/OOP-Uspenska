public class Rectangle
{
    private double _width;
    private double _height;

    public double Width
    {
        get { return _width; }
        set { _width = value; }
    }

    public double Height
    {
        get { return _height; }
    }

    public Rectangle(double width, double height)
    {
        _width = width;
        _height = height;
    }

    public double GetArea()
    {
        return _width * _height;
    }

    public bool IsSquare()
    {
        return _width == _height;
    }
}