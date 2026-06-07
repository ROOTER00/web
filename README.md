# Kurdixane E-Ticaret Platformu

ASP.NET Core 8 MVC + Entity Framework Core + MSSQL ile geliştirilmiş, tam dinamik
ve yönetilebilir bir e-ticaret altyapısı. Tüm içerikler (ürün, kategori, slider,
blog, sayfa, menü, ayarlar) veritabanından yönetilir ve Admin Panel üzerinden
düzenlenebilir.

## Teknolojiler

- **ASP.NET Core 8 MVC** (Razor View Engine)
- **Entity Framework Core 8** (Code-First, Migrations)
- **MSSQL** (SQL Server)
- **Repository Pattern + Service Layer** mimarisi
- **Cookie tabanlı kimlik doğrulama** (PBKDF2-SHA256 şifre hashleme)
- **Session tabanlı sepet**
- **CKEditor** (zengin içerik editörü)
- **Bootstrap 5** (responsive arayüz)

## Proje Yapısı

```
src/Kurdixane.Web/
├── Areas/Admin/            # Yönetim paneli (Controllers + Views)
├── Controllers/            # Genel site controller'ları
├── Data/                   # DbContext, Migrations, DbInitializer (seed)
├── Helpers/                # SlugHelper, ClaimsPrincipal uzantıları
├── Models/                 # EF entity'leri
├── Repositories/           # Generic Repository Pattern
├── Services/               # İş mantığı (Service Layer)
├── ViewComponents/         # Header / Footer view component'leri
├── ViewModels/             # Sunum katmanı modelleri
├── Views/                  # Razor view'ları (Shared layout dahil)
└── wwwroot/                # Statik dosyalar (css, js, img, lib)
database/
├── schema.sql              # CREATE TABLE + Foreign Key betikleri
└── seed.sql                # Örnek tohum verisi
```

## Sayfalar

**Genel:** Anasayfa (slider + öne çıkan/yeni ürünler + blog), Ürün Listeleme,
Ürün Detay, Kategori Sayfaları, Sepet, Checkout, Giriş/Kayıt, Profil, Blog,
Dinamik Sayfalar (Hakkımızda vb.), İletişim.

**Admin Panel:** Gösterge Paneli, Ürünler, Kategoriler, Siparişler, Slider,
Blog, Sayfalar, Menü, Mesajlar, Kullanıcılar, Sistem Ayarları.

## Kurulum

### 1. Gereksinimler
- .NET 8 SDK
- SQL Server (yerel veya Docker)

Docker ile hızlı SQL Server:
```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Kurdixane!2026" \
  -p 1433:1433 --name kurdixane-sql -d mcr.microsoft.com/mssql/server:2022-latest
```

### 2. Bağlantı dizesi
`src/Kurdixane.Web/appsettings.json` içindeki `DefaultConnection` değerini kendi
ortamınıza göre düzenleyin.

### 3. Çalıştırma
```bash
cd src/Kurdixane.Web
dotnet run
```

Uygulama ilk açılışta migration'ları uygular ve `DbInitializer` ile tohum
verisini otomatik ekler. Alternatif olarak `database/schema.sql` ve
`database/seed.sql` betiklerini elle çalıştırabilirsiniz.

### 4. Yönetici Girişi
```
E-posta : admin@kurdixane.com
Şifre   : Admin123!
```
Admin panel: `/Admin/Dashboard`

## Mimari Notlar

- **Repository Pattern:** `IRepository<T>` generic arayüzü tüm temel veri
  erişimini soyutlar; `Repository<T>` EF Core uygulamasıdır.
- **Service Layer:** `CatalogService`, `ContentService`, `OrderService`,
  `UserService`, `SettingService`, `CartService` iş mantığını barındırır ve
  controller'lar yalnızca servislerle konuşur.
- **Slug üretimi:** `SlugHelper`, Türkçe karakterleri (ç, ğ, ı, ö, ş, ü)
  destekleyerek SEO dostu URL üretir.
- **ViewComponent:** Header ve Footer, veritabanından gelen menü/kategori/ayar
  verisiyle dinamik olarak render edilir.
