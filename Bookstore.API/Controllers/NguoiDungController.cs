using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bookstore.API.Data;
using Bookstore.API.Models;
using Bookstore.API.Interfaces;

namespace Bookstore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NguoiDungController : BaseController<NguoiDung, string>
    {
        public NguoiDungController(IGenericRepository<NguoiDung, string> repo) : base(repo)
        {
        }
    }
}
