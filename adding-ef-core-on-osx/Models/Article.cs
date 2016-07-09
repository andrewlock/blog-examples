namespace AddingEFCoreOnOSX.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Body { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}