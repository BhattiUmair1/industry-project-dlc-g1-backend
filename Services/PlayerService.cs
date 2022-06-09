namespace Opdracht.Services
{
    public interface IPlayerService
    {
        Task<int> GetScoreAsync(string team, string playerID);
        //    Task<Folder> GetScoreAsync(string docRef);
        //    Task<List<Folder>> GetFoldersAsync(string UserId);
        //    Task<Folder> AddFolderAsync(Folder folder);
        //    Task<Folder> UpdateFolderAsync(Folder folder);
        //    Task DeleteFolderAsync(string UserId);
    }
    public class PlayerService : IPlayerService
    {
        private readonly IUserInfoRepository _userInfoRepository;
        public PlayerService(IUserInfoRepository userInfoRepository)
        {
            _userInfoRepository = userInfoRepository;
        }

        public async Task<int> GetScoreAsync(string team, string playerID)
        {
            int score = await _userInfoRepository.GetScoreAsync(team, playerID);
            return score;
        }

        //public async Task<Folder> GetFolderAsync(string docRef)
        //{
        //    Folder folder = await _userInfoRepository.GetFolderAsync(docRef);
        //    return folder;
        //}
        //public async Task<List<Folder>> GetFoldersAsync(string UserId)
        //{
        //    List<Folder> folders = await _userInfoRepository.GetFoldersAsync(UserId);
        //    return folders;
        //}
        //public async Task<Folder> AddFolderAsync(Folder folder)
        //{
        //    folder.DocRef = Guid.NewGuid().ToString();
        //    Folder addedFolder = await _userInfoRepository.AddFolderAsync(folder);
        //    return addedFolder;
        //}
        //public async Task<Folder> UpdateFolderAsync(Folder editedFolder)
        //{
        //    Folder folder = await _userInfoRepository.UpdateFolderAsync(editedFolder);
        //    return folder;
        //}
        //public async Task DeleteFolderAsync(string DocRef)
        //{
        //    await _userInfoRepository.DeleteFolderAsync(DocRef);
        //    return;
        //}
    }
}
