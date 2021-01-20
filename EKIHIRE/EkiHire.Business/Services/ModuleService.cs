using EkiHire.Core.Domain.Entities;
using EkiHire.Data.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace EkiHire.Business.Services
{
    public interface IModuleService
    {

    }
    public class ModuleService: IModuleService
    {
        readonly IRepository<Module> _repo;
        private readonly IServiceHelper _serviceHelper;
        public ModuleService(
            IRepository<Module> repo,
            IServiceHelper serviceHelper
            )
        {
            _repo = repo;
            _serviceHelper = serviceHelper;
        }
    }
}
