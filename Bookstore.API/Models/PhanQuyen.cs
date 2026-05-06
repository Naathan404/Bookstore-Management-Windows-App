namespace Bookstore.API.Models
{
    public class PhanQuyen
        //PHANQUYEN
    {
        public int MaNhomNguoiDung { get; set; }
        public int MaChucNang { get; set; }

        public virtual NhomNguoiDung NhomNguoiDung { get; set; }
        public virtual ChucNang ChucNang { get; set; }
    }
}
