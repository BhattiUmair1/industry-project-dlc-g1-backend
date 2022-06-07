namespace Opdracht.Models
{
    // UserInfo myDeserializedClass = JsonConvert.DeserializeObject<UserInfo>(myJsonResponse);
    [FirestoreData]
    [AuthorizeAttribute]
    public class Score
    {
        [FirestoreProperty]
        public string? PlayerID { get; set; }
        [FirestoreProperty]
        public string? DocRef { get; set; }
        [FirestoreProperty]
        public string? FolderTitle { get; set; }
        [FirestoreProperty]
        public bool IsAFolder { get; set; }
        [FirestoreProperty]
        public List<SetRef>? SetRefs { get; set; }
    }

}


