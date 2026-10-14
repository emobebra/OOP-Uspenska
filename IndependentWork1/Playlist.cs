public class Playlist
{
    private string _name;
    private int _trackCount;
    private double _totalMinutes;

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public int TrackCount
    {
        get { return _trackCount; }
    }

    public Playlist(string name, int trackCount, double totalMinutes)
    {
        _name = name;
        _trackCount = trackCount;
        _totalMinutes = totalMinutes;
    }

    public void AddTrack(double minutes)
    {
        _trackCount++;
        _totalMinutes += minutes;
    }

    public double GetAverageTrackLength()
    {
        return _totalMinutes / _trackCount;
    }
}