# 🎓 Student-Course API – ASP.NET Core Backend

Bu proje, öğrenci ve ders yönetimi sağlayan bir web uygulamasının **Backend (API)** kısmıdır.  
RESTful servis mimarisi ile geliştirilmiş olup, kimlik doğrulama, rol bazlı yetkilendirme ve temel CRUD işlemlerini destekler.

---

## ⚙️ Teknolojiler

- ASP.NET Core 8 Web API
- Entity Framework Core (Code-First)
- SQL Server
- JWT (JSON Web Token) Authentication
- Role-based Authorization (Admin, Student, Teacher)
- Katmanlı Mimari:
  - **Domain** (varlıklar ve ortak modeller)
  - **Infrastructure** (veri erişimi, dış servisler)
  - **Application** (iş kuralları ve servisler)
  - **WebApi** (controller ve endpoint katmanı)

---

## 🚀 Kurulum Adımları

### 1. Projeyi Klonlayın

```bash
git clone https://github.com/kullaniciAdi/student-course-backend.git
cd student-course-backend

### 2. Docker ile bağlantı kurun

docker compose up --build
update database 
docker compose up --build

### 3. Uygulamayı Başlat
dotnet run --project WebApi
