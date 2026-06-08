/* ============================================================
   Kurdixane E-Ticaret - Örnek Tohum Verisi (Seed Data)
   ------------------------------------------------------------
   NOT: Uygulama ilk çalıştığında Data/DbInitializer.cs
   tohum verisini otomatik olarak ekler. Bu betik, veritabanını
   yalnızca SQL ile kurmak isteyenler için sağlanmıştır.
   schema.sql çalıştırıldıktan SONRA çalıştırın.
   ============================================================ */

SET NOCOUNT ON;
DECLARE @now DATETIME2 = SYSUTCDATETIME();

/* ---------- Site Ayarları ---------- */
INSERT INTO SiteSettings ([Key],[Value],[Description],[Group],CreatedAt) VALUES
 ('SiteTitle','Kurdixane','Site başlığı','Genel',@now),
 ('SiteSlogan','Geleneksel Lezzetler, Doğal Ürünler','Slogan','Genel',@now),
 ('FooterText','© 2026 Kurdixane. Tüm hakları saklıdır.','Footer','Genel',@now),
 ('ContactPhone','+90 555 000 00 00','Telefon','İletişim',@now),
 ('ContactEmail','info@kurdixane.com','E-posta','İletişim',@now),
 ('ContactAddress','İstanbul, Türkiye','Adres','İletişim',@now),
 ('FacebookUrl','#','Facebook','Sosyal',@now),
 ('InstagramUrl','#','Instagram','Sosyal',@now),
 ('TwitterUrl','#','Twitter','Sosyal',@now),
 ('MetaDescription','Kurdixane - geleneksel ve doğal ürünler e-ticaret platformu','SEO','SEO',@now),
 ('MetaKeywords','kurdixane, doğal ürünler, baharat, kuruyemiş','SEO','SEO',@now);

/* ---------- Yönetici Kullanıcı (şifre: Admin123!) ---------- */
INSERT INTO Users (FullName,Email,PasswordHash,Phone,Role,IsActive,CreatedAt) VALUES
 ('Site Yöneticisi','admin@kurdixane.com','100000.CpTnlm89bmmkFHY7VPDT2g==.FN5gq8KXiMwNhQDMG1cahpONVTek9r14H5csge0RP+Y=',NULL,1,1,@now);

/* ---------- Kategoriler ---------- */
INSERT INTO Categories (Name,Slug,Description,ParentId,IsActive,DisplayOrder,CreatedAt) VALUES
 ('Baharatlar','baharatlar','Doğal ve yöresel baharatlar',NULL,1,1,@now),
 ('Kuruyemişler','kuruyemisler','Taze kuruyemiş çeşitleri',NULL,1,2,@now),
 ('Bal & Reçel','bal-recel','Doğal bal ve ev yapımı reçeller',NULL,1,3,@now),
 ('Zeytin & Zeytinyağı','zeytin-zeytinyagi','Naturel sızma zeytinyağı ve zeytin',NULL,1,4,@now);

/* ---------- Slider ---------- */
INSERT INTO Sliders (Title,Subtitle,ImageUrl,Link,ButtonText,IsActive,DisplayOrder,CreatedAt) VALUES
 ('Geleneksel Lezzetler','Doğanın en taze ürünleri kapınızda','/img/sliders/slide-1.svg','/Product','Ürünleri Keşfet',1,1,@now),
 ('Yöresel Baharatlar','Mutfağınıza eşsiz bir dokunuş','/img/sliders/slide-2.svg','/Category/baharatlar','İncele',1,2,@now),
 ('Doğal Kuruyemişler','%100 doğal, katkısız lezzet','/img/sliders/slide-3.svg','/Category/kuruyemisler','İncele',1,3,@now);

/* ---------- Menü (Header) ---------- */
INSERT INTO MenuItems (Title,Url,ParentId,Location,IsActive,DisplayOrder,CreatedAt) VALUES
 ('Anasayfa','/',NULL,0,1,1,@now),
 ('Ürünler','/Product',NULL,0,1,2,@now),
 ('Blog','/Blog',NULL,0,1,3,@now),
 ('Hakkımızda','/Page/hakkimizda',NULL,0,1,4,@now),
 ('İletişim','/Contact',NULL,0,1,5,@now);

/* ---------- Sayfalar ---------- */
INSERT INTO Pages (Title,Slug,Content,IsActive,CreatedAt) VALUES
 ('Hakkımızda','hakkimizda','<p>Kurdixane, geleneksel ve doğal ürünleri sizlerle buluşturmak için kurulmuştur.</p>',1,@now),
 ('Gizlilik Politikası','gizlilik-politikasi','<p>Kişisel verileriniz gizlilik politikamız kapsamında korunmaktadır.</p>',1,@now);

/* ---------- Blog ---------- */
INSERT INTO BlogPosts (Title,Slug,Summary,Content,ImageUrl,Author,IsPublished,PublishedAt,CreatedAt) VALUES
 ('Baharatların Faydaları','baharatlarin-faydalari','Doğal baharatların sağlığa katkıları','<p>Baharatlar yemeklere lezzet katmanın yanı sıra birçok sağlık faydası sunar.</p>','/img/blog/post-1.svg','Kurdixane',1,@now,@now),
 ('Kuruyemiş Saklama Yöntemleri','kuruyemis-saklama','Kuruyemişleri taze tutmanın yolları','<p>Kuruyemişleri serin ve kuru ortamda saklamak tazeliğini korur.</p>','/img/blog/post-2.svg','Kurdixane',1,@now,@now),
 ('Doğal Balın Önemi','dogal-balin-onemi','Gerçek bal nasıl anlaşılır?','<p>Doğal bal hem besleyici hem de şifa kaynağıdır.</p>','/img/blog/post-3.svg','Kurdixane',1,@now,@now);

/* ---------- Örnek Ürünler ---------- */
INSERT INTO Products (Name,Slug,Sku,ShortDescription,Description,Price,OldPrice,Stock,ImageUrl,CategoryId,IsActive,IsFeatured,DisplayOrder,CreatedAt) VALUES
 ('Pul Biber','pul-biber','SPC-001','Yöresel acı pul biber','<p>Geleneksel yöntemlerle hazırlanmış pul biber.</p>',75.00,90.00,120,'/img/products/placeholder-1.svg',1,1,1,1,@now),
 ('Kekik','kekik','SPC-002','Doğal dağ kekiği','<p>Yüksek aromalı dağ kekiği.</p>',60.00,NULL,80,'/img/products/placeholder-2.svg',1,1,0,2,@now),
 ('Antep Fıstığı','antep-fistigi','NUT-001','Taze Antep fıstığı','<p>Birinci kalite Antep fıstığı.</p>',320.00,360.00,45,'/img/products/placeholder-3.svg',2,1,1,3,@now),
 ('Çiğ Badem','cig-badem','NUT-002','Doğal çiğ badem','<p>Katkısız çiğ badem.</p>',180.00,NULL,60,'/img/products/placeholder-4.svg',2,1,0,4,@now),
 ('Çiçek Balı','cicek-bali','HNY-001','Doğal süzme çiçek balı','<p>Yaylalardan doğal çiçek balı.</p>',250.00,NULL,30,'/img/products/placeholder-5.svg',3,1,1,5,@now),
 ('Vişne Reçeli','visne-receli','HNY-002','Ev yapımı vişne reçeli','<p>Geleneksel ev yapımı vişne reçeli.</p>',95.00,120.00,40,'/img/products/placeholder-6.svg',3,1,0,6,@now),
 ('Sızma Zeytinyağı','sizma-zeytinyagi','OIL-001','Naturel sızma zeytinyağı','<p>Soğuk sıkım naturel sızma zeytinyağı.</p>',400.00,NULL,25,'/img/products/placeholder-7.svg',4,1,1,7,@now),
 ('Yeşil Zeytin','yesil-zeytin','OIL-002','Kırma yeşil zeytin','<p>Doğal fermente kırma yeşil zeytin.</p>',140.00,NULL,55,'/img/products/placeholder-8.svg',4,1,0,8,@now);

PRINT 'Seed data başarıyla eklendi.';
