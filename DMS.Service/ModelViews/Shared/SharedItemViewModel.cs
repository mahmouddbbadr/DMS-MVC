using DMS.Domain.ENums;

namespace DMS.Service.ModelViews.Shared
{
    public class SharedItemViewModel
    {
        public string Id { get; set; }=string.Empty ;
        public string? FolderName { get; set; }
        public string? DocumentName { get; set; }
        public string? FolderId { get; set; }
        public string? DocumentId { get; set; }
        public string? FilePath { get; set; }
        public string SharedByUserName { get; set; } = string.Empty;
        public string SharedWithUserName { get; set; } = string.Empty;
        public PermissionLevel Permission { get; set; } = PermissionLevel.Read;
        public DateTime SharedDate { get; set; }
        public bool IsSharedByMe { get; set; }

    }
}
