using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BookPollClientApp.Interfaces;
using BookPollClientApp.Models;
using Microsoft.WindowsAzure.MobileServices;
using System.Linq;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System.Diagnostics;

namespace BookPollClientApp.Services
{
    public class AzurePollService : IPollQuestionService
    {
        const string AzureUrl = @"http://bookpollapp.azurewebsites.net";
        MobileServiceClient client;

            IMobileServiceSyncTable<PollQuestion> questionsTable;
            IMobileServiceSyncTable<PollResponse> responseTable;

        async Task InitializeAsync()
        {
            if (client != null)
                return;
            var store = new MobileServiceSQLiteStore("Poll.db");
            store.DefineTable<PollQuestion>();
            store.DefineTable<PollResponse>();


            client = new MobileServiceClient(AzureUrl);

            await client.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler());
            questionsTable = client.GetSyncTable<PollQuestion>();
            responseTable = client.GetSyncTable<PollResponse>();
            try
            {
                await client.SyncContext.PushAsync();
                await questionsTable.PullAsync(
                    "allQuestions", questionsTable.CreateQuery());
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Got exception: {0}", ex.Message);
            }
        }
        async Task SynchronizeResponsesAsync(string questionId)
        {
            try
            {
                await responseTable.PullAsync("syncResponses" + questionId,
                                                  responseTable.Where(
                                                  r => r.PollQuestionId == questionId));
            }
            catch (Exception ex)
            {
                // TODO: handle error
                Debug.WriteLine("Got exception: {0}", ex.Message);
            }
        }


        public async Task AddOrUpdatePollResponseAsync(PollResponse response)
        {
            await InitializeAsync();

            if (string.IsNullOrEmpty(response.Id))
            {
                await responseTable.InsertAsync(response);
            }
           await responseTable.UpdateAsync(response);
           await SynchronizeResponsesAsync(response.PollQuestionId);

        }

        public async Task DeletePollResponseAsync(PollResponse response)
        {
            await InitializeAsync();
            await responseTable.DeleteAsync(response);
            await SynchronizeResponsesAsync(response.PollQuestionId);
        }

        public async Task<IEnumerable<PollQuestion>> GetQuestionsAsync()
        {
            await InitializeAsync();
            return await questionsTable.ReadAsync();
        }
        string lastQuestionId;
        public async Task<PollResponse> GetResponseForPollAsync(string questionId, string name)
        {
            await InitializeAsync();

            if (lastQuestionId != questionId)
            {
                // Get the latest responses for this question.
                await SynchronizeResponsesAsync(questionId);
                lastQuestionId = questionId;
            }

            return (await responseTable.Where(r => r.PollQuestionId == questionId && r.Name == name)
    .ToEnumerableAsync()).FirstOrDefault();

        }

        public async Task<IEnumerable<PollResponse>> GetResponsesForPollAsync(string questionId)
        {
            await InitializeAsync();
            return await responseTable
              .OrderByDescending(r => r.UpdatedAt)
              .Take(100).ToEnumerableAsync();

        }
    }
}
