namespace DLC.Models;

[FirestoreData]
[AuthorizeAttribute]
public class Sponsor
{
  [FirestoreProperty]
  public string SponsorId { get; set; }

  [FirestoreProperty]
  public string Source { get; set; }

}