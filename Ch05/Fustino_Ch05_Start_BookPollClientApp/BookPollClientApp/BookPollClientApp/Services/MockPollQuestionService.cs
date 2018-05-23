using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookPollClientApp.Interfaces;
using BookPollClientApp.Models;

namespace BookPollClientApp.Services
{
    public class MockPollQuestionService : IPollQuestionService
    {
        List<PollResponse> responses;
        List<PollQuestion> questions;

        async Task Initialize()
        {
            await Task.Delay(1000);

            if (questions != null)
                return;

            responses = new List<PollResponse>();

            questions = new List<PollQuestion>
            {
                new PollQuestion { Id = "1", Text = "What book would you like to read next?",
                    Answers = "Beginning Entity Framework Core 2.0|Beginning Windows Mixed Reality Programming|Business in Real-Time, Using Azure IoT|Cyber Security on Azure|Angular 5 and .NET Core 2" },
                new PollQuestion { Id = "2", Text = "What is your favorite book category?",
                    Answers = "Apple and iPS|Programming|Machine Learning|Mobile|Microsoft and .NET" },

            };
        }

        public async Task<IEnumerable<PollQuestion>> GetQuestionsAsync()
        {
            await Initialize();
            return await Task.FromResult<IEnumerable<PollQuestion>>(questions);
        }

        public async Task<IEnumerable<PollResponse>> GetResponsesForPollAsync(string questionId)
        {
            await Initialize();
            return await Task.FromResult(
                responses.Where(s => s.PollQuestionId == questionId));
        }

        public async Task<PollResponse> GetResponseForPollAsync(string questionId, string name)
        {
            await Initialize();
            return await Task.FromResult(
                responses.FirstOrDefault(s => s.PollQuestionId == questionId && s.Name == name));
        }

        public async Task AddOrUpdatePollResponseAsync(PollResponse response)
        {
            await Initialize();

            if (response.Id == null)
            {
                response.Id = new Guid ().ToString ();
            }

            var existing = responses.SingleOrDefault (s => s.PollQuestionId == response.PollQuestionId
                                                      && s.Name == response.Name);
            if (existing != null)
                responses.Remove (existing);

            responses.Add (response);
        }

        public async Task DeletePollResponseAsync(PollResponse response)
        {
            await Initialize();
            responses.Remove(response);
        }
    }
}
