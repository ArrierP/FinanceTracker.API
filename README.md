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

## 🧑‍💻 Phân công nhiệm vụ

* Phạm Tuấn Huy - **Backend Developer**:  Thiết kế cơ sở dữ liệu, quản lí ví và tài khoản cá nhân.
* Nguyễn Thị Kim Thoa - **Backend Developer**:  Quản lí danh mục global, user và báo cáo thống kê.
* Nguyễn Huỳnh Bảo Châu - **Backend Developer**:  Quản lí giao dịch và danh mục cá nhân.
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
