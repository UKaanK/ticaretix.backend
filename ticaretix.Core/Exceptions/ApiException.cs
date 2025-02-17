namespace ticaretix.Core.Exceptions
{
    public class ApiException : Exception
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public ApiException(int errorCode) : base(GetErrorMessage(errorCode)) // Message yerine ErrorMessage kullanılır
        {
            ErrorCode = errorCode;
            ErrorMessage = GetErrorMessage(errorCode);
        }

        public override string Message => ErrorMessage; // Base Exception'ın mesajını override et
    


    private static string GetErrorMessage(int errorCode)
        {
            return errorCode switch
            {
                ErrorCodes.U101 => ErrorMessages.UserNotFound,
                ErrorCodes.U102 => ErrorMessages.IncorrectPassword,
                ErrorCodes.U103 => ErrorMessages.UserNotActive,
                ErrorCodes.U104 => ErrorMessages.UsernameAlreadyExists,
                ErrorCodes.U105 => ErrorMessages.EmailAlreadyExists,
                ErrorCodes.U201 => ErrorMessages.AccessDenied,
                ErrorCodes.U202 => ErrorMessages.InvalidToken,
                ErrorCodes.U204 => ErrorMessages.OperationSuccessfulNoContent,
                ErrorCodes.U203 => ErrorMessages.TokenExpired,
                ErrorCodes.V301 => ErrorMessages.InvalidEmailFormat,
                ErrorCodes.V302 => ErrorMessages.InvalidPasswordFormat,
                ErrorCodes.V303 => ErrorMessages.MissingFields,
                ErrorCodes.S500 => ErrorMessages.InternalServerError,
                ErrorCodes.S501 => ErrorMessages.DatabaseError,
                ErrorCodes.S502 => ErrorMessages.ConnectionError,
                ErrorCodes.S503 => ErrorMessages.ServiceUnavailable,
                ErrorCodes.I601 => ErrorMessages.OperationFailed,
                ErrorCodes.I602 => ErrorMessages.OperationAlreadyCompleted,
                ErrorCodes.I603 => ErrorMessages.OperationCannotBeCancelled,
                ErrorCodes.I604 => ErrorMessages.InvalidPaymentInfo,
                _ => ErrorMessages.UnknownError
            };
        }
    }
}
