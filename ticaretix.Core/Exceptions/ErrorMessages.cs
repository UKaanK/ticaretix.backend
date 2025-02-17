namespace ticaretix.Core.Exceptions
{
    public static class ErrorMessages
    {
        // Kullanıcı ile ilgili hata mesajları
        public const string UserNotFound = "Kullanıcı bulunamadı!";
        public const string IncorrectPassword = "Şifre yanlış!";
        public const string UserNotActive = "Kullanıcı aktif değil!";
        public const string UsernameAlreadyExists = "Kullanıcı adı zaten mevcut!";
        public const string EmailAlreadyExists = "E-posta adresi zaten kayıtlı!";

        // Yetkilendirme ile ilgili hata mesajları
        public const string AccessDenied = "Erişim reddedildi!";
        public const string InvalidToken = "Token geçersiz!";
        public const string TokenExpired = "Token süresi dolmuş!";

        // Veri doğrulama hataları
        public const string InvalidEmailFormat = "Geçersiz e-posta formatı!";
        public const string InvalidPasswordFormat = "Geçersiz şifre formatı!";
        public const string MissingFields = "Gerekli alanlar eksik!";
        public const string OperationSuccessfulNoContent = "İşlem başarılı, ancak içerik yok!";

        // Sunucu hataları
        public const string InternalServerError = "Bir hata oluştu, lütfen daha sonra tekrar deneyiniz!";
        public const string DatabaseError = "Veri tabanı hatası oluştu!";
        public const string ConnectionError = "Bağlantı hatası!";
        public const string ServiceUnavailable = "Hizmet geçici olarak kullanılamaz!";

        // İşlem hataları
        public const string OperationFailed = "İşlem başarısız oldu!";
        public const string OperationAlreadyCompleted = "İşlem zaten gerçekleşmiş!";
        public const string OperationCannotBeCancelled = "İşlem iptal edilemez!";
        public const string InvalidPaymentInfo = "Geçersiz ödeme bilgileri!";

        // Diğer mesajlar
        public const string UnknownError = "Bilinmeyen bir hata oluştu!";
    }
}
