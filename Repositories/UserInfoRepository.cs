namespace DLC.Repositories
{
  public interface IUserInfoRepository
  {
    Task<List<Player>> GetPlayers();

  }
  public class UserInfoRepository : IUserInfoRepository
  {
    private readonly FirestoreDb _db;
    private readonly string _path = @"./Firebase/project-dlc-firebase-adminsdk.json";
    // dit nog bekijken
    //public static Precondition MustExist { get; }
    public UserInfoRepository()
    {
      Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", _path);
      _db = FirestoreDb.Create("test");
    }

    #region GET

    public async Task<List<Player>> GetPlayers()
    {
      List<Player> playerlist = new List<Player>();
      Query playerQuery = _db.Collection("players").WhereEqualTo("IsPlayer", true);
      QuerySnapshot playerQuerySnapshot = await playerQuery.GetSnapshotAsync();
      foreach (var documentSnapshot in playerQuerySnapshot.Documents)
      {
        Console.WriteLine("Document data for {0} document:", documentSnapshot.Id);
        playerlist.Add(documentSnapshot.ConvertTo<Player>());

        Dictionary<string, object> player = documentSnapshot.ToDictionary();
        foreach (KeyValuePair<string, object> pair in player)
        {
          Console.WriteLine("{0}: {1}", pair.Key, pair.Value);
        }
      }
      return playerlist;
    }

    // public async Task<List<Set>> GetSetsAsync(string UserId, bool Condition)
    // {
    //   Query querySets = _db.Collection("sets").WhereEqualTo("UserId", UserId);
    //   QuerySnapshot querySetsSnapshot = await querySets.GetSnapshotAsync();

    //   List<Set> userInfo = new List<Set>();
    //   foreach (var document in querySetsSnapshot.Documents)
    //   {
    //     userInfo.Add(document.ConvertTo<Set>());
    //   }
    //   return userInfo;
    // }
    #endregion

    #region POST

    #endregion

    #region UPDATE

    #endregion

    #region DELETE

    #endregion

    // public async Task<Set> GetSetAsync(string DocRef)
    // {
    //   Query querySets = _db.Collection("sets").WhereEqualTo("DocRef", DocRef);
    //   QuerySnapshot querySetsSnapshot = await querySets.GetSnapshotAsync();
    //   if (querySetsSnapshot.FirstOrDefault() != null)
    //   {
    //     return querySetsSnapshot.FirstOrDefault().ConvertTo<Set>();
    //   }

    //   return new Set();
    // }
    // public async Task<Set> AddSetAsync(Set set)
    // {
    //    foreach(var studyset in set.Studyset)
    //    {
    //        studyset.Id = Guid.NewGuid().ToString();
    //    }
    //    var docRefPath = Guid.NewGuid().ToString();
    //    set.DocRef = docRefPath;
    //    DocumentReference docRef = _db.Collection("sets").Document(docRefPath);

    //    // HELP: hoe kan men weten of de userInfo succesfully toegevoegd is.
    //    await docRef.SetAsync(set);
    //    return set;
    // }
    //public async Task<Set> UpdateSetAsync(Set set)
    //{
    //    // respository
    //    DocumentReference docRef = _db.Collection("sets").Document(set.DocRef);

    //    // HELP: hoe kan men weten of de userInfo succesfully toegevoegd is.
    //    await docRef.SetAsync(set);

    //    return set;
    //}
    //public async Task DeleteSetAsync(string DocRef)
    //{
    //    DocumentReference document = _db.Collection("sets").Document(DocRef);
    //    await document.DeleteAsync();
    //    return;
    //}
  }

}
