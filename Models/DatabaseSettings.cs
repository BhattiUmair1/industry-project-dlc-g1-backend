namespace DLC.Models;

// UserInfo myDeserializedClass = JsonConvert.DeserializeObject<UserInfo>(myJsonResponse);
[FirestoreData]
[AuthorizeAttribute]
public class DataBaseSettings
{
  [FirestoreProperty]
  public string? CollectionId { get; set; }
  [FirestoreProperty]
  public string? DocumentId { get; set; }
  [FirestoreProperty]
  public string? FolderId { get; set; }
}




