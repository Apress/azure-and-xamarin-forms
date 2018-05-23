namespace BookPollClientApp.Models
{
    public class PollResponse
    {
        public string Id { get; set; }
        public string PollQuestionId { get; set; }
        public string Name { get; set; }
        public int ResponseIndex { get; set; }
    }
}
