public class Book
{
    private string _title;
    private int _pages;
    private int _pagesRead;

    public string Title
    {
        get { return _title; }
        set { _title = value; }
    }

    public int Pages
    {
        get { return _pages; }
    }

    public Book(string title, int pages)
    {
        _title = title;
        _pages = pages;
        _pagesRead = 0;
    }

    public void ReadPages(int count)
    {
        _pagesRead += count;
        if (_pagesRead > _pages)
        {
            _pagesRead = _pages;
        }
    }

    public double GetProgress()
    {
        return (double)_pagesRead / _pages * 100;
    }
}