using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Share.DTOResponses
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty; // Chứa chuỗi mã hóa JWT
        public string Username { get; set; } = string.Empty;
        public int Role { get; set; } // Trả về role để Frontend biết đường ẩn/hiện menu
    }
}
