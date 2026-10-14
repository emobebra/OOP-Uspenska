public class Program
{
    public static void Main(string[] args)
    {
        Book book = new Book("Ти", 494);
        book.ReadPages(242);
        Console.WriteLine($"Книга '{book.Title}': прочитано {book.GetProgress():F1}% із {book.Pages} сторінок");

        Playlist playlist = new Playlist("Улюблена музика", 100, 250);
        playlist.AddTrack(3);
        Console.WriteLine($"Плейлист '{playlist.Name}': {playlist.TrackCount} треків, середня тривалість {playlist.GetAverageTrackLength():F2} хв");

        Rectangle rectangle = new Rectangle(12, 5);
        Console.WriteLine($"Площа прямокутника: {rectangle.GetArea()}");
        Console.WriteLine($"Це квадрат? {rectangle.IsSquare()}");
    }
}