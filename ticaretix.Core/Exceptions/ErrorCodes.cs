namespace ticaretix.Core.Exceptions
{
    public static class ErrorCodes
    {
        // Kullanıcı ile ilgili hata kodları
        public const int U101 = 101; // Kullanıcı bulunamadı
        public const int U102 = 102; // Şifre yanlış
        public const int U103 = 103; // Kullanıcı aktif değil
        public const int U104 = 104; // Kullanıcı adı zaten mevcut
        public const int U105 = 105; // Email adresi zaten kayıtlı

        // Yetkilendirme ile ilgili hata kodları
        public const int U201 = 201; // Erişim reddedildi
        public const int U202 = 202; // Token geçersiz
        public const int U203 = 203; // Token süresi dolmuş
        public const int U204 = 204; // İşlem başarılı, ancak içerik yok

        // Veri doğrulama hataları
        public const int V301 = 301; // Geçersiz e-posta formatı
        public const int V302 = 302; // Geçersiz şifre formatı
        public const int V303 = 303; // Alanlar eksik

        // Sunucu hataları
        public const int S500 = 500; // Genel sunucu hatası
        public const int S501 = 501; // Veri tabanı hatası
        public const int S502 = 502; // Bağlantı hatası
        public const int S503 = 503; // Hizmet geçici olarak kullanılamaz

        // İşlem hataları
        public const int I601 = 601; // İşlem başarısız
        public const int I602 = 602; // İşlem zaten gerçekleşmiş
        public const int I603 = 603; // İşlem iptal edilemez
        public const int I604 = 604; // Geçersiz ödeme bilgileri

        // Diğer hata kodları
        public const int G999 = 999; // Bilinmeyen hata
    }
}
