namespace BookPollClientApp.Models
{
    public class PollQuestion
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Answers { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
