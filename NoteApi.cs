using ApiKiaFunc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ApiKiaFunc
{
    public class NoteApi
    {
        internal List<Note> notes = new List<Note>();
        
        private readonly ILogger<NoteApi> _logger;
        CosmosClient _cosmosClient;
        private Container documentContainer;

        public NoteApi(ILogger<NoteApi> logger, CosmosClient cosmosClient)
        {
            _logger = logger;
            _cosmosClient = cosmosClient;
            documentContainer = _cosmosClient.GetContainer("memodb", "note");
        }
        
        [Function("CreateNote")]
        public async Task<IActionResult> CreateNote([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "note")] HttpRequest req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Note? note = JsonConvert.DeserializeObject<Note?>(requestBody);

            if (note == null)
            {
                return new BadRequestResult();
            }

            note.CreatedAt = DateTime.Now;
            note.UpdatedAt = DateTime.Now;

            note.Id = Guid.NewGuid().ToString();
            note.Category = 7;

            await documentContainer.CreateItemAsync(note, new Microsoft.Azure.Cosmos.PartitionKey(note.Category));

            return new OkObjectResult(note);
        }

        [Function("GetNotes")]
        public async Task<IActionResult> GetNotes([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "note")] HttpRequest req)
        {
            var items = documentContainer.GetItemQueryIterator<Note>();
            return new OkObjectResult((await items.ReadNextAsync()).ToList());
        }
    }
}
