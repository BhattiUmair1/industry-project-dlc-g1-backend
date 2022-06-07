namespace Opdracht.Repositories
{
    public interface IUserInfoRepository
    {
        Task<Folder> GetFolderAsync(string docRef);
        Task<Set> GetSetAsync(string DocRef);
        Task<List<Set>> GetSetsAsync(string UserId, bool Condition);
        Task<List<Set>> GetSetsRecentAsync(string UserId);
        Task<Set> AddSetAsync(Set set);
        Task<Set> UpdateSetAsync(Set set);
        Task DeleteSetAsync(string DocRef);
        Task<List<Folder>> GetFoldersAsync(string UserId);
        Task<Folder> AddFolderAsync(Folder folder);
        Task<Folder> UpdateFolderAsync(Folder folder);
        Task DeleteFolderAsync(string DocRef);
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

        public async Task<Folder> GetFolderAsync(string docRef)
        {
            Query querySets = _db.Collection("folders").WhereEqualTo("DocRef", docRef);
            QuerySnapshot querySetsSnapshot = await querySets.GetSnapshotAsync();
            if (querySetsSnapshot.FirstOrDefault() != null)
            {
                return querySetsSnapshot.FirstOrDefault().ConvertTo<Folder>();
            }

            return new Folder();
        }
        public async Task<Set> GetSetAsync(string DocRef)
        {
            Query querySets = _db.Collection("sets").WhereEqualTo("DocRef", DocRef);
            QuerySnapshot querySetsSnapshot = await querySets.GetSnapshotAsync();
            if (querySetsSnapshot.FirstOrDefault() != null)
            {
                return querySetsSnapshot.FirstOrDefault().ConvertTo<Set>();
            }

            return new Set();
        }
        public async Task<List<Set>> GetSetsAsync(string UserId, bool Condition)
        {
            Query querySets = _db.Collection("sets").WhereEqualTo("UserId", UserId);
            if (Condition ==  true) {
                querySets = _db.Collection("sets").WhereEqualTo("UserId", UserId).Limit(6);
            }
            QuerySnapshot querySetsSnapshot = await querySets.GetSnapshotAsync();

            List<Set> userInfo = new List<Set>();
            foreach (var document in querySetsSnapshot.Documents)
            {
                userInfo.Add(document.ConvertTo<Set>());
            }
            return userInfo;
        }

        public async Task<List<Set>> GetSetsRecentAsync(string UserId)
        {
            Query querySets = _db.Collection("sets").WhereEqualTo("UserId", UserId).OrderBy("ReviewedOn");
            QuerySnapshot querySetsSnapshot = await querySets.GetSnapshotAsync();

            List<Set> userInfo = new List<Set>();
            foreach (var document in querySetsSnapshot.Documents)
            {
                userInfo.Add(document.ConvertTo<Set>());
            }
            return userInfo;
        }


        public async Task<Set> AddSetAsync(Set set)
        {
            foreach(var studyset in set.Studyset)
            {
                studyset.Id = Guid.NewGuid().ToString();
            }
            var docRefPath = Guid.NewGuid().ToString();
            set.DocRef = docRefPath;
            DocumentReference docRef = _db.Collection("sets").Document(docRefPath);

            // HELP: hoe kan men weten of de userInfo succesfully toegevoegd is.
            await docRef.SetAsync(set);
            return set;
        }
        public async Task<Set> UpdateSetAsync(Set set)
        {
            // respository
            DocumentReference docRef = _db.Collection("sets").Document(set.DocRef);

            // HELP: hoe kan men weten of de userInfo succesfully toegevoegd is.
            await docRef.SetAsync(set);

            return set;
        }
        public async Task DeleteSetAsync(string DocRef)
        {
            DocumentReference document = _db.Collection("sets").Document(DocRef);
            await document.DeleteAsync();
            return;
        }
        public async Task<List<Folder>> GetFoldersAsync(string UserId)
        {
            // respository
            Query querySets = _db.Collection("folders").WhereEqualTo("UserId", UserId).WhereEqualTo("IsAFolder", true);
            QuerySnapshot querySetsSnapshot = await querySets.GetSnapshotAsync();
            List<Folder> folders = new List<Folder>();
            foreach (var document in querySetsSnapshot.Documents)
            {
                folders.Add(document.ConvertTo<Folder>());
            }
            return folders;
        }
        public async Task<Folder> AddFolderAsync(Folder folder)
        {
            DocumentReference docRef = _db.Collection("folders").Document(folder.DocRef);
            await docRef.SetAsync(folder);
            return folder;
        }
        public async Task<Folder> UpdateFolderAsync(Folder folder)
        {
            DocumentReference docRef = _db.Collection("folders").Document(folder.DocRef);
            await docRef.SetAsync(folder);
            return folder;
        }
        public async Task DeleteFolderAsync(string DocRef)
        {
            DocumentReference document = _db.Collection("folders").Document(DocRef);
            await document.DeleteAsync();
        }
    }

}
