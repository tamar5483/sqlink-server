using Microsoft.AspNetCore.Http;
using MyProject.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Services.Interfaces
{
    public interface IUserService
    {
        public  Task<Response<List<User>>> FilterUsers(IFormFile formFile, CancellationToken cancellationToken);
    }
}
