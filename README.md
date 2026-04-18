# Finance Tracker API

Dự án **Finance Tracker** là một hệ thống quản lý tài chính cá nhân được xây dựng trên nền tảng **ASP.NET Core**. Ứng dụng cho phép người dùng theo dõi ví, quản lý danh mục thu chi, ghi lại giao dịch và xem báo cáo thống kê trực quan thông qua kiến trúc **Service-Layer**.

---

## 🏗 Kiến trúc Hệ thống (Architecture)
Dự án được triển khai dưới dạng project mang tên `FinanceTracker.Api`. Toàn bộ logic nghiệp vụ được tách biệt vào các lớp Service để đảm bảo tính module hóa và dễ bảo trì.

## 🛠 Công nghệ & Thư viện Sử dụng
* **Framework:** .NET Core API.
* **Database:** SQL Server qua `Microsoft.EntityFrameworkCore.SqlServer`.
* **Xác thực:** JWT (JSON Web Token) qua `Microsoft.AspNetCore.Authentication.JwtBearer`.
* **Bảo mật:** `BCrypt.Net-Next` dùng để băm mật khẩu người dùng.

---

## 📂 Cấu trúc Thành phần Chính

### 1. Cấu hình Hệ thống (Infrastructure)
* **`appsettings.json`**: Lưu trữ chuỗi kết nối cơ sở dữ liệu và cấu hình bí mật cho JWT.
* **`Program.cs`**: Đăng ký các dịch vụ (Dependency Injection) và thiết lập Pipeline cho Authentication/Authorization.

### 2. Tầng Domain (Core)
Thư mục `Domain/` chứa các thành phần nền tảng của ứng dụng:
* **Entities**: Định nghĩa các bảng như `User`, `Wallet`, `Category`, và `Transaction`.
* **Audit**: Lớp `BaseAuditableEntity` giúp tự động theo dõi thời gian và người tạo/chỉnh sửa dữ liệu.
* **Enums**: Định nghĩa các hằng số cho vai trò người dùng (`UserRole`), loại giao dịch, và trạng thái.

### 3. Tầng Nghiệp vụ (Business Logic)
Toàn bộ logic xử lý dữ liệu nằm ở các dịch vụ được đăng ký dưới dạng **Scoped**:
* **`AuthService`**: Quản lý đăng ký, đăng nhập và tạo JWT token.
* **`CurrentUserService`**: Lấy thông tin người dùng đang đăng nhập từ `IHttpContextAccessor`.
* **Quản lý Tài chính**: `IWalletService`, `ICategoryService`, và `ITransactionService` xử lý CRUD và kiểm tra quyền sở hữu dữ liệu.
* **Thống kê**: `IDashboardService` tính toán tổng thu chi và dữ liệu biểu đồ.
* **Quản trị**: `IAdminService` dành riêng cho việc quản lý người dùng hệ thống.

### 4. Tầng Giao tiếp (API & DTOs)
* **DTOs (Data Transfer Objects)**: Các đối tượng chuyển đổi dữ liệu cho Auth, Ví, Danh mục và Giao dịch để tối ưu hóa dữ liệu truyền tải.
* **Controllers**: Chịu trách nhiệm nhận yêu cầu HTTP và điều hướng logic.
    * Sử dụng attribute `[Authorize]` để bảo mật các đầu cuối (endpoints).
    * `AdminController` yêu cầu quyền `Admin` để truy cập.

---

## 🚀 Hướng dẫn Cài đặt

1.  **Cập nhật cấu hình:**
    Mở file `appsettings.json` và điền thông tin vào `DefaultConnection` cùng các tham số `Jwt`.

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
    Sử dụng Entity Framework để cập nhật cấu trúc bảng.
    ```bash
    dotnet ef database update
    ```

4.  **Chạy ứng dụng:**
    ```bash
    dotnet run
    ```
