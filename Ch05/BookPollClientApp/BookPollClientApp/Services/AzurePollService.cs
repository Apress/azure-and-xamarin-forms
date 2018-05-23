using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BookPollClientApp.Interfaces;
using BookPollClientApp.Models;
using Microsoft.WindowsAzure.MobileServices;
using System.Linq;


namespace BookPollClientApp.Services
{
    public class AzurePollService : IPollQuestionService
    {
        const string AzureUrl = @"http://bookpollapp.azurewebsites.net";
        MobileServiceClient client;
        IMobileServiceTable<PollResponse> responseTable;
        IMobileServiceTable<PollQuestion> questionsTable;

        void Initialize()
        {
            if (client != null)
                return;

            client = new MobileServiceClient(AzureUrl);
            questionsTable = client.GetTable<PollQuestion>();
            responseTable = client.GetTable<PollResponse>();

        }


        public Task AddOrUpdatePollResponseAsync(PollResponse response)
        {
            Initialize();

            if (string.IsNullOrEmpty(response.Id))
            {
                return responseTable.InsertAsync(response);
            }
            return responseTable.UpdateAsync(response);

        }

        public async Task DeletePollResponseAsync(PollResponse response)
        {
            Initialize();
            await responseTable.DeleteAsync(response);
        }

        public Task<IEnumerable<PollQuestion>> GetQuestionsAsync()
        {
            Initialize();
            return questionsTable.ReadAsync();
        }

        public async Task<PollResponse> GetResponseForPollAsync(string questionId, string name)
        {
            Initialize();
            return (await responseTable.Where(r => r.PollQuestionId == questionId && r.Name == name)
    .ToEnumerableAsync()).FirstOrDefault();

        }

        public async Task<IEnumerable<PollResponse>> GetResponsesForPollAsync(string questionId)
        {
            Initialize();
            return await responseTable
              .OrderByDescending(r => r.UpdatedAt)
              .Take(100).ToEnumerableAsync();

        }
    }
}
