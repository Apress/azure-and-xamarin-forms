using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Azure.Mobile.Server;
using Newtonsoft.Json;


namespace BookPollAppService.DataObjects
{
    [Table("responses")]
    public class PollResponse : EntityData
    {
        [JsonProperty("questionId")]
        public string QuestionId { get; set; }

        public string Name { get; set; }
        [JsonProperty("answer")]
        public int AnswerIndex { get; set; }
    }

}