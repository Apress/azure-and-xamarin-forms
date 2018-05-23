using Newtonsoft.Json;
using Microsoft.WindowsAzure.MobileServices;
using System;

namespace BookPollClientApp.Models
{
    [JsonObject(Title = "responses")]
    public class PollResponse
    {
        public string Id { get; set; }
        [JsonProperty("questionId")]
        public string PollQuestionId { get; set; }
        public string Name { get; set; }
        [JsonProperty("answer")]
        public int ResponseIndex { get; set; }
        [UpdatedAt]
        public DateTimeOffset UpdatedAt { get; set; }

    }

}
