---
name: testing-ecommerce
description: Kurdixane ASP.NET Core MVC + EF Core + MSSQL e-ticaret uygulamasını uçtan uca test etme rehberi. Public alışveriş akışı, checkout, admin panel CRUD ve yetkilendirmeyi doğrularken kullan.
---

# Kurdixane E-Ticaret Test Rehberi

## Uygulamayı çalıştırma
- .NET 8 SDK: `/home/ubuntu/.dotnet` (PATH'e ekli olmalı).
- MSSQL Docker container: `docker start kurdixane-sql` (yoksa `docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Kurdixane!2026" -p 1433:1433 --name kurdixane-sql -d mcr.microsoft.com/mssql/server:2022-latest`).
- Sunucu: `cd src/Kurdixane.Web && ASPNETCORE_URLS=http://localhost:5080 dotnet run`.
- DB başlangıçta otomatik migrate + seed edilir.

## Test kimlik bilgileri (Devin Secrets gerekmez — seed'li)
- Admin: `admin@kurdixane.com` / `Admin123!`
- Admin panel: `/Admin` veya `/Admin/Dashboard`.

## Önemli rotalar
- Anasayfa: `/` (DB tabanlı slider + kategori + öne çıkan ürünler)
- Ürün listesi: `/Product` ; kategori: `/Category/{slug}`
- Ürün detay: `/urun/{slug}` (örn. `/urun/antep-fistigi`)
- Sepet: `/Cart` ; Checkout: `/Checkout` → başarı `/Checkout/Success?orderNumber=...`
- Giriş: `/Account/Login` ; Admin ürün ekleme: `/Admin/Products/Create`

## Golden-path test sırası
1. Anasayfada slider + öne çıkan ürünlerin DB'den geldiğini doğrula.
2. Bir ürün detayına git → Sepete Ekle → sepette satır + kargo dahil toplamı kontrol et.
3. Checkout formunu doldur → gönder → `/Checkout/Success`'te sipariş numarası üretilmeli (format `KX<tarih><hex>`).
4. Admin'e giriş → `/Admin/Orders`'da siparişin kalıcı olduğunu doğrula → durum güncelle (Pending→Confirmed).
5. **Adversarial dinamik içerik testi:** Admin'de yeni ürün oluştur → public `/urun/<slug>` adresinde canlı göründüğünü doğrula. Slug isimden otomatik üretilir (`SlugHelper`).
6. Çıkış yap → `/Admin/Products`'a anonim eriş → `/Account/Login`'e yönlenmeli (yetkilendirme).

## Slug / Türkçe karakter doğrulaması
- `SlugHelper` Türkçe karakterleri çevirir: ç→c, ğ→g, ı→i, ö→o, ş→s, ü→u, İ→i.
- Seed verisi bunu kanıtlar: "Antep Fıstığı"→`antep-fistigi`, "İsot Biberi"→`isot-biberi`.

## Bilinen test kısıtları (workaround'lar)
- **xdotool Türkçe karakter düşürme:** Tarayıcıya `type` ile yazarken `ı`, `ğ`, `ş` gibi karakterler düşebilir (örn. "Yılmaz"→"Ylmaz"). Bu bir uygulama hatası DEĞİL. Form testlerinde ASCII isimler kullanmak güvenli; Türkçe slug dönüşümünü seed verisi üzerinden doğrula.
- **Adres çubuğuna URL yazarken karakter düşmesi:** `ctrl+l` sonrası `type` bazen ilk/bir karakteri düşürebilir (örn. `/Admin/Orders`→`/Admin/rders`). URL yazımından sonra kısa bir `wait` ekleyip ekranı doğrula, gerekirse yeniden yaz.
- CKEditor CDN'den yüklenir; ürün/blog/sayfa formlarında `textarea.ckeditor` üzerine zengin editör render edilir. Araç çubuğunun göründüğünü doğrula.

## CI
- Repoda CI yapılandırması yok (0 check). CI doğrulaması beklenmemeli.
