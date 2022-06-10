namespace DLC.Models;

[FirestoreData]
[AuthorizeAttribute]
public class User
{
  [FirestoreProperty]
  public string UserId { get; set; }

  [FirestoreProperty]
  public int Password { get; set; }

}