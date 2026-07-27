Session Özeti — Teknik ve Mimari Öğrenimler
1. Güvenlik Temelleri
HTTPS Redirection
app.UseHttpsRedirection() yorumda kalmıştı — production'da HTTP trafiği şifresiz kabul ediliyordu. Bu satırın varlığı, tüm cookie/form verisinin ağ üzerinde açık gitmemesini garanti eder.

Sızan Production Kimlik Bilgileri
appsettings.json'da gerçek bir Azure SQL bağlantı dizesi (sunucu adı, kullanıcı adı, düz metin şifre) ilk commit'ten beri repo'da duruyordu — public bir GitHub reposunda. Öğrenilen temel prensip: git bir kere gördüğü şeyi asla unutmaz — dosyayı sonradan temizlesen bile eski commit'lerde kalıcı olarak durur (git log -p, git show <hash>:<dosya> ile herkes görebilir).

2. Secrets Yönetimi — Üç Katmanlı Model
Bu sorunu çözerken üç farklı mekanizma öğrenildi, her biri farklı bir çalıştırma bağlamı için:

Mekanizma	Nerede durur	Ne zaman kullanılır
dotnet user-secrets	~/.microsoft/usersecrets/<UserSecretsId>/secrets.json — proje klasörünün tamamen dışında	Local dotnet run
.env + .gitignore	Proje klasörünün içinde ama git'in görmezden geldiği bir dosya	Docker Compose
appsettings.json	Git-tracked, herkese açık	Sadece hassas olmayan varsayılan ayarlar
Kritik ayrım: user-secrets'ın koruması yapısal (dosya git'in tarama alanının dışında, unutmaya gerek yok), .env'in koruması disiplin bazlı (.gitignore kuralına bağlı, insan hatasına açık). İkisi de "git'e hiç girmesin" hedefine hizmet eder, farklı güç seviyelerinde.

IConfiguration soyutlaması: Kod (app.Configuration["SeedAdmin:Email"]) hangi kaynaktan okuduğunu hiç bilmez — ASP.NET Core, appsettings.json → appsettings.{Env}.json → user-secrets → environment variables sırasıyla katmanları birleştirir, sonraki öncekini ezer. Bu yüzden aynı kod, hiç değişmeden, hem local'de hem Docker'da farklı kaynaklardan besleniyor.

3. EF Core Migration Sistemi — Gizli Bir Bağımlılık
Bulgu: Projenin 4 migration'ının .Designer.cs eşlik dosyaları hiç commit edilmemişti (ilk commit'ten beri). Bu dosyalar [Migration("id")] attribute'unu taşır — EF Core migration'ları bu attribute'lara bakarak keşfeder, .cs dosyası tek başına yeterli değildir. Sonuç: proje hiçbir zaman sıfırdan kurulamıyordu, sadece geliştiricinin zaten migration'ları elle uygulamış olduğu local veritabanıyla çalışıyordu.

Çözüm ve öğrenilen pattern: Migration'ları silip, mevcut modelden tek temiz bir InitialCreate migration'ı oluşturmak, Docker'da sıfırdan test ederek doğrulamak. "Build başarılı" ≠ "çalışıyor" — gerçek bir kurulum denemesi olmadan bu bug hiç görünmezdi.

Seed data stratejisi — iki farklı mekanizma, iki farklı amaç:

Migration-time seed (HasData): statik, hassas olmayan veri (16 restoran) için — migration bir kez uygulanır, __EFMigrationsHistory tablosu takip eder, bir daha tekrarlanmaz.
Runtime seed (Program.cs'te idempotent kod): hassas veri (admin/client şifreleri) için — her dotnet run'da çalışır ama if (user == null) guard'ı sayesinde gerçek DB yazması sadece ilk seferde olur. Kod çalışma sıklığı ile side effect sıklığı aynı şey değil.
4. Docker & Containerization
Katmanlı mimari: Docker Desktop (local build+run+yönetim) ≠ Kubernetes (çoklu sunucuda orkestrasyon) ≠ CI/CD (docker build otomasyonu). Üçü farklı sorumluluk alanları — Kubernetes asla image build etmez, sadece hazır image'ı çalıştırır.

İki çalıştırma modu tasarımı:


Mod A: docker compose up sqlserver + dotnet run   → geliştirme, hot-reload
Mod B: docker compose up (hepsi container'da)      → paketli, taşınabilir
Data Protection key kalıcılığı — öğrenilen en önemli operasyonel ders: container'ın geçici dosya sistemi ile kalıcı bir volume arasındaki fark. Cookie şifreleme anahtarı container'a gömülüyse, her --build yeni anahtar = herkes çıkışa zorlanır. Named volume (sqldata ile aynı mantık) ile bu kalıcı hale getirildi — gerçek dünyada Kubernetes/çoklu-instance senaryolarında zorunlu olan bir pattern.

LibMan — üçüncü parti frontend kütüphanelerini (Bootstrap, jQuery, Bootstrap Icons) git'e commit etmek yerine bir manifest (libman.json) ile "restore" edilebilir hale getirme; node_modules mantığının .NET karşılığı.

5. Yetkilendirme (Authorization) — Derinlemesine
Sistematik açık taraması metodolojisi öğrenildi: Her CRUD endpoint'ine şu soru soruldu — "bu sayfa [Authorize] ile korunuyor mu, ve eğer bir ID alıyorsa, o ID'nin sahibi doğrulanıyor mu?" Bu tarama sonucu bulunanlar:

Yetkilendirme eksikliği (4 sayfa): Sadece listeleme sayfalarında rol kontrolü vardı, asıl mutasyon yapan (Create/Edit/Delete/AddReservation) sayfalarda hiç yoktu — URL'i bilen herkes doğrudan erişebiliyordu.
IDOR (Insecure Direct Object Reference): DeleteReservationModel, sadece id'ye bakıp siliyordu, "bu senin mi?" kontrolü yoktu — bir client, başka bir client'ın rezervasyon ID'sini tahmin ederek silebilirdi.
GET ile veri mutasyonu (CSRF riski): Silme işlemi OnGetAsync içindeydi — GET istekleri anti-forgery token ile korunamaz, bir <img> etiketiyle bile tetiklenebilir. Standart pattern'e (GET=onay ekranı, POST=gerçek işlem) çevrildi.
Sessiz regresyon: Model validasyonu eklerken ([Required]), [BindProperty] olarak işaretli ama formdan hiç gönderilmeyen bir Restaurant property'si, tüm rezervasyon oluşturma akışını sessizce kırdı — build hatasız geçti, ama gerçek kullanıcı testi olmadan fark edilmezdi.
Simetrik test metodolojisi: "Client admin verisine erişebilir mi?" VE "Admin client verisine erişebilir mi?" — iki yönü de test etmek gerekti, biri diğerini garanti etmiyor.

404 vs 403 farkı: Sahiplik kontrolü başarısız olduğunda NotFound() (404) dönmek, Forbidden (403) dönmekten daha güvenli — 404, "bu kaynak var ama sana yasak" bilgisini bile sızdırmıyor.

6. JWT vs Cookie Tabanlı Kimlik Doğrulama
Ana çıkarım: "Modern" olmak "her zaman daha güvenli" demek değil — doğru araç, mimariye bağlı.

Cookie: HttpOnly ile XSS'e karşı korumalı, iptal edilebilir (şifre değişince anında geçersiz), ama CSRF'e karşı ekstra önlem (anti-forgery token) gerektirir. Sunucu-render web app'ler için doğru seçim.
JWT: CSRF'e doğal bağışık (header'da taşınır), ama iptal edilemez (blacklist tutmadan) — token süresi dolana kadar geçerli kalır. SPA/mobil/mikroservis mimarileri için doğru seçim.
Bu proje (Razor Pages, sunucu-render) cookie tabanlı Identity kullanıyor — mimari olarak doğru, JWT'ye geçmek burada bir gerileme olurdu.

7. Frontend Mimarisi ve Tasarım Kararları
Ölçeğe uygun tasarım prensibi: Backend-odaklı bir öğrenme projesinde frontend'e "yeterli ama abartısız" emek harcamak doğru — Bootstrap + minimal custom CSS, kendi tasarım sistemi icat etmemek.

Admin panel ≠ müşteri gözat sayfası: Aynı veri (restoranlar), iki farklı UX ihtiyacına göre iki farklı pattern gerektirdi — admin için tablo (yoğun veri, hızlı tarama/karşılaştırma), müşteri için kart grid + detail modal (görsel keşif, seçim).

DRY prensibi pratikte: İki sayfada birebir tekrarlanan bir dl bloğu fark edilip _RestaurantDetailList.cshtml partial'ına çıkarıldı — "üç benzer satır sorun değil, ama tam kopya blok bir soyutlamayı hak eder" ayrımı.

Gerçek bug'lar frontend'de de olur: margin-right: 10; (birim eksik, tarayıcı sessizce yok sayıyor), asp-validation-for="X " (trailing space, tag helper eşleşmesini bozuyor), yanlış model property'sine referans (RestaurantDto.ImageFile yerine dosya adı) — bunlar "görünüşte çalışıyor ama aslında hiç işlevsel değil" kategorisinde, sadece kodu okuyarak değil, render edilmiş HTML'i inceleyerek bulundu.

8. Test Metodolojisi — "Build Başarılı" Yeterli Değil
Bu session boyunca tekrar tekrar doğrulanan bir prensip: iddiayı gerçek bir istekle kanıtla.

curl ile gerçek login akışları (CSRF token'ı sayfadan çekip POST'a ekleme)
Docker container'ın loglarını okuyarak "hangi SQL sorgusu çalıştı" seviyesinde doğrulama
Veritabanına doğrudan sqlcmd ile bakıp "gerçekten yazıldı mı" kontrolü
Yetkisiz erişim denemelerini gerçekten çalıştırıp (sadece "olması gerekir" demek değil) sonucun doğru olduğunu (302 → AccessDenied, 404 → ownership fail) teyit etme
Bu yöntemle bulunan en çarpıcı örnek: [Required] validasyonu eklerken kırılan AddReservation akışı — sadece "build geçti" deseydik, bu regresyon production'a kadar fark edilmezdi.

9. Versiyonlama ve Git Disiplini
Semantic Versioning + Keep a Changelog formatı — versiyon numarası tek kaynaktan (csproj'daki <Version>) okunuyor, UI'da reflection ile gösteriliyor, elle iki yerde senkronize etmeye gerek yok.
Mantıksal commit bölme: Tek dev commit yerine, her biri tek bir anlatıya sahip commit'ler (fix:, feat:, security:, style:, chore:, docs:) — bu hem git log'u okunabilir kılıyor hem de gelecekte git bisect ile bug aramayı kolaylaştırıyor.
Tag'ler gerçek commit durumunu yansıtmalı — "v0.1.0" hiç commit edilmediği için (0.2.0'a üzerine yazıldığı için) tag'lenmedi; bu, "her versiyon numarası mutlaka bir tag'e karşılık gelmeyebilir" öğretisi.
En üst seviye çıkarım: Bu proje, "junior seviyesinde CRUD öğren" hedefinden başlayıp, gerçek bir yazılım ekibinin karşılaşacağı sınıftan sorunları (sızan credential, kırık migration sistemi, yetkilendirme açıkları, container'lar arası state yönetimi) uçtan uca bulma → düzeltme → gerçek testle doğrulama → belgeleme döngüsüyle deneyimleyen bir vaka çalışmasına dönüştü