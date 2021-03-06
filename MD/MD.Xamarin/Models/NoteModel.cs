﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MD.Xamarin.Models
{
    public class NoteModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("photos")]
        public PhotoModel[] Photos { get; set; }

        [JsonProperty("userid")]
        public string UserId { get; set; }
    }
}
