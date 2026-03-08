# **BOOKSTORE MANAGEMENT DESKTOP APPLICATION**
## ***Phần mềm Quản lý Nhà sách*** 

### **About us**
- Đây là đồ án môn học Nhập môn Công nghệ Phần mềm tại Trường Đại học Công nghệ Thông tin, ĐHQG-HCM (UIT)
- Thành viên nhóm

---

### **Tech Stack (Công nghệ sử dụng)**
- 🥚**Backend:** `ASP.NET Core Web API 8.0`  
- 🍔 **Frontend:** `WPF`  
- 🍲 **Model:** `MVVM`
- 🍙 **Database:** `SQL Server`  
- 🧁 **ORM:** `Entity Framework Core`  
- 🍾 **Libraries:** `MaterialDesignThemes`, `CommunityToolkit.Mvvm`, `Swashbuckle.AspNetCore` 

---

### **Project Structure (Cấu trúc dự án)**
```
.
├── Bookstore.API/
│   ├── Controllers/    Chứa các file xử lý API
│   ├── Data/           AppDbContext và Migrations 
│   ├── Models/         Các bảng trong DB
│   ├── DTOs/           Chứa các class DTO
│   └── Services/       Chứa các class và hàm thao tác, phức tạp
├── Bookstore.WPF/
│   ├── Views/          Chứa các file giao diện .xaml
│   ├── ViewModels/     ViewModel của các giao diện
│   ├── Models/         Chứa các class phía client
│   ├── Services/       Các hàm gọi API, http
│   ├── Resources/      Chứa hình ảnh, font và các styles.xaml
│   └── Helpers/        Các hàm tiện ích
└── Bookstore.Share/
    ├── Constants/      Chứa hằng số dùng chung
    ├── Enums/          Chứa enum dùng chung
    └── Requests/       Chứa dữ liệu mẫu gửi từ wpf đến api
```

---

### **Set up project** 
#### 1. Yêu cầu hệ thống
- Visual Studio 2022 (Workload .NET Web & Desktop)  
- SQL Server (LocalDB)  
- .NET 8.0 SDK  
#### 2. Cấu hình Database
- Mở file `appsetting.json` trong `Bookstore.API`
- Chỉnh lại DefaultConnection phù hợp với SQL Server của máy. Thông thường cứ giữ nguyên
- Mở Package Manager Console, chọn default project là `Bookstore.API` 
- Gõ lệnh `Update-Database`
#### 3. Chạy dự án
- Chuột phải vào `Solution`-> `Properties`
- Chọn `Multiple startup projects`
- Set Bookstore.API và Bookstore.WPF thành Start
- Chạy bằng F5 hoặc ấn nút Run
#### 4. Ghi chú
- Dùng lệnh `Add-Migration InitialCreate -OutputDir Data/Migrations` để cập nhật Migrations khi thay đổi code database
- Dùng lệnh 'Update-Database` để cập nhật Migrations vào database trong SQL Server
- Nếu chạy migration bị sai thì dùng lệnh `Remove-Migration` để xóa
---

## **Quy định làm việc**
- Mỗi thành viên khi làm chức năng mới phải tạo nhánh riêng tên feat/ten-chuc-nang
- Trước khi push code, hãy pull về trước
- Nhánh làm việc chính là nhánh dev
- Liên hệ Nguyen Nguyen khi gặp lỗi 
