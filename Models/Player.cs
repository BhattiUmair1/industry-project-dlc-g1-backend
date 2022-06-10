namespace DLC.Models;

// UserInfo myDeserializedClass = JsonConvert.DeserializeObject<UserInfo>(myJsonResponse);
[FirestoreData]
[AuthorizeAttribute]
public class Player
{
  [FirestoreProperty]
  public string? PlayerID { get; set; }

  [FirestoreProperty]
  public string Location { get; set; }

  [FirestoreProperty]
  public bool IsPlayer { get; set; }
  [FirestoreProperty]
  public string poopie { get; set; }
}




