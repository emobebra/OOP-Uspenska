public class Program
{
    public static void Main(string[] args)
    {
        Book book = new Book("ти", 494);
        book.ReadPages(242);
        Console.WriteLine($"книга '{book.Title}': прочитано {book.GetProgress():F1}% з {book.Pages} сторінок");

        Playlist playlist = new Playlist("улюблена музика", 100, 250);
        playlist.AddTrack(3);
        Console.WriteLine($"плейлист '{playlist.Name}': {playlist.TrackCount} треків, середня тривалість {playlist.GetAverageTrackLength():F2} хв");

        Rectangle rectangle = new Rectangle(12, 5);
        Console.WriteLine($"площа прямокутника: {rectangle.GetArea()}");
        Console.WriteLine($"це квадрат? {rectangle.IsSquare()}");
    }
}