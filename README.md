# Finance Tracker API

[cite_start]Dự án **Finance Tracker** là một hệ thống quản lý tài chính cá nhân được xây dựng trên nền tảng **ASP.NET Core**[cite: 1, 2]. [cite_start]Ứng dụng cho phép người dùng theo dõi ví, quản lý danh mục thu chi, ghi lại giao dịch và xem báo cáo thống kê trực quan thông qua kiến trúc **Service-Layer**[cite: 2, 88, 59].

---

## 🏗 Kiến trúc Hệ thống (Architecture)
[cite_start]Dự án được triển khai dưới dạng một project duy nhất mang tên `FinanceTracker.Api`[cite: 2]. [cite_start]Toàn bộ logic nghiệp vụ được tách biệt vào các lớp Service để đảm bảo tính module hóa và dễ bảo trì[cite: 35, 77].

## 🛠 Công nghệ & Thư viện Sử dụng
* [cite_start]**Framework:** .NET Core API[cite: 2].
* [cite_start]**Database:** SQL Server qua `Microsoft.EntityFrameworkCore.SqlServer`[cite: 19, 67].
* [cite_start]**Xác thực:** JWT (JSON Web Token) qua `Microsoft.AspNetCore.Authentication.JwtBearer`[cite: 7, 20].
* [cite_start]**Bảo mật:** `BCrypt.Net-Next` dùng để băm mật khẩu người dùng[cite: 21, 43].

---

## 📂 Cấu trúc Thành phần Chính

### 1. Cấu hình Hệ thống (Infrastructure)
* [cite_start]**`appsettings.json`**: Lưu trữ chuỗi kết nối cơ sở dữ liệu và cấu hình bí mật cho JWT[cite: 4, 6, 7].
* [cite_start]**`Program.cs`**: Đăng ký các dịch vụ (Dependency Injection) và thiết lập Pipeline cho Authentication/Authorization[cite: 8, 10, 15].

### 2. Tầng Domain (Core)
[cite_start]Thư mục `Domain/` chứa các thành phần nền tảng của ứng dụng[cite: 23]:
* [cite_start]**Entities**: Định nghĩa các bảng như `User`, `Wallet`, `Category`, và `Transaction`[cite: 30].
* [cite_start]**Audit**: Lớp `BaseAuditableEntity` giúp tự động theo dõi thời gian và người tạo/chỉnh sửa dữ liệu[cite: 28, 29].
* [cite_start]**Enums**: Định nghĩa các hằng số cho vai trò người dùng (`UserRole`), loại giao dịch, và trạng thái[cite: 24, 27].

### 3. Tầng Nghiệp vụ (Business Logic)
[cite_start]Toàn bộ logic xử lý dữ liệu nằm ở các dịch vụ được đăng ký dưới dạng **Scoped**[cite: 11, 12]:
* [cite_start]**`AuthService`**: Quản lý đăng ký, đăng nhập và tạo JWT token[cite: 41, 45].
* [cite_start]**`CurrentUserService`**: Lấy thông tin người dùng đang đăng nhập từ `IHttpContextAccessor`[cite: 39, 40].
* [cite_start]**Quản lý Tài chính**: `IWalletService`, `ICategoryService`, và `ITransactionService` xử lý CRUD và kiểm tra quyền sở hữu dữ liệu[cite: 46, 50, 54, 58].
* [cite_start]**Thống kê**: `IDashboardService` tính toán tổng thu chi và dữ liệu biểu đồ[cite: 59, 61].
* [cite_start]**Quản trị**: `IAdminService` dành riêng cho việc quản lý người dùng hệ thống[cite: 63, 65].

### 4. Tầng Giao tiếp (API & DTOs)
* [cite_start]**DTOs (Data Transfer Objects)**: Các đối tượng chuyển đổi dữ liệu cho Auth, Ví, Danh mục và Giao dịch để tối ưu hóa dữ liệu truyền tải[cite: 73, 75].
* [cite_start]**Controllers**: Chịu trách nhiệm nhận yêu cầu HTTP và điều hướng logic[cite: 76, 77].
    * [cite_start]Sử dụng attribute `[Authorize]` để bảo mật các đầu cuối (endpoints)[cite: 86, 90].
    * [cite_start]`AdminController` yêu cầu quyền `Admin` để truy cập[cite: 80, 82].

---

## 🚀 Hướng dẫn Cài đặt

1.  **Cập nhật cấu hình:**
    [cite_start]Mở file `appsettings.json` và điền thông tin vào `DefaultConnection` cùng các tham số `Jwt`[cite: 4, 6, 7].

2.  **Cài đặt NuGet Packages:**
    ```bash
    dotnet add package Microsoft.EntityFrameworkCore.SqlServer
    dotnet add package Microsoft.EntityFrameworkCore
    dotnet add package Microsoft.EntityFrameworkCore.Design
    dotnet add package Microsoft.EntityFrameworkCore.Tools
    dotnet add package Swashbuckle.AspNetCore
    dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
    dotnet add package BCrypt.Net-Next
    ```

3.  **Khởi tạo Database:**
    Sử dụng Entity Framework để cập nhật cấu trúc bảng[cite: 67, 70].
    ```bash
    dotnet ef database update
    ```

4.  **Chạy ứng dụng:**
    ```bash
    dotnet run
    ```
