using System.Collections.Generic;
using System.Threading.Tasks;
using BookPollClientApp.Models;

namespace BookPollClientApp.Interfaces
{
    public interface IPollQuestionService
    {
        Task AddOrUpdatePollResponseAsync(PollResponse response);
        Task DeletePollResponseAsync(PollResponse response);
        Task<IEnumerable<PollQuestion>> GetQuestionsAsync();
        Task<IEnumerable<PollResponse>> GetResponsesForPollAsync(string questionId);
        Task<PollResponse> GetResponseForPollAsync(string questionId, string name);
    }
}