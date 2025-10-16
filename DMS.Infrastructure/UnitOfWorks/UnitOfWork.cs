using DMS.Infrastructure.DataContext;
using DMS.Infrastructure.IRepositories;
using DMS.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Infrastructure.UnitOfWorks
{
    public class UnitOfWork
    {
        private readonly DMSContext context;

        public IAppUserRepository appUserRepository;
        public IFolderRepository folderRepository;
        public IDocumentRepository documentRepository;
        public ISharedItemRepository sharedItemRepository;

        public UnitOfWork(DMSContext context)
        {
            this.context = context;
        }

        public IAppUserRepository AppUserRepository 
        {
            get
            {
                if (appUserRepository == null)
                    appUserRepository = new AppUserRepository(context);
                return appUserRepository;
            }
        }
        public IFolderRepository FolderRepository
        {
            get
            {
                if (folderRepository == null)
                    folderRepository = new FolderRepository(context);
                return folderRepository;
            }
        }
        public IDocumentRepository DocumentRepository
        {
            get
            {
                if (documentRepository == null)
                    documentRepository = new DocumentRepository(context);
                return documentRepository;
            }
        }
        public ISharedItemRepository SharedItemRepository
        {
            get
            {
                if (sharedItemRepository == null)
                    sharedItemRepository = new SharedItemRepository(context);
                return sharedItemRepository;
            }
        }
        public void Save()
        {
            context.SaveChanges();
        }

    }
}
