using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiKiaFunc.Models
{
    internal class Note
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("category")]
        public int Category { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    internal class CreateNote
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    internal class UpdateNote
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}