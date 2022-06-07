namespace Opdracht.Repositories
{
    public interface IUserInfoRepository
    {

        //Task<Set> GetSetAsync(string DocRef);
        //Task<Set> AddSetAsync(Set set);
        //Task<Set> UpdateSetAsync(Set set);
        //Task DeleteSetAsync(string DocRef);
    }
    public class UserInfoRepository : IUserInfoRepository
    {
        private readonly FirestoreDb _db;
        private readonly string _path = @"./Firebase/Firebasestudyapp-343918-firebase-adminsdk-cm4kr-2069e8f542.json";
        // dit nog bekijken
        //public static Precondition MustExist { get; }
        public UserInfoRepository()
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", _path);
            _db = FirestoreDb.Create("studyapp-343918");
        }


        //public async Task<Set> GetSetAsync(string DocRef)
        //{
        //    Query querySets = _db.Collection("sets").WhereEqualTo("DocRef", DocRef);
        //    QuerySnapshot querySetsSnapshot = await querySets.GetSnapshotAsync();
        //    if (querySetsSnapshot.FirstOrDefault() != null)
        //    {
        //        return querySetsSnapshot.FirstOrDefault().ConvertTo<Set>();
        //    }

        //    return new Set();
        //}
        //public async Task<Set> AddSetAsync(Set set)
        //{
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
        //}
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
