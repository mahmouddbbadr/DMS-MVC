namespace DMS.Service.ModelViews.Folder
{
    public class CreateFolderResult
    {
        public bool Success { get; set; }
        public bool NameExists { get; set; }
        public string ErrorMessage { get; set; }
    }

}
