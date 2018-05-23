using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.Mobile.Server;
using System.ComponentModel.DataAnnotations.Schema;


namespace BookPollAppService.DataObjects
{
    [Table("questions")]
    public class PollQuestion : EntityData
    {
        public string Text { get; set; }
        public string Answers { get; set; }
    }

}