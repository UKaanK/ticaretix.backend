using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ticaretix.Core.Interfaces
{
    public interface ILoggerService
    {
        Task LogInfoAsync(string message);
        Task LogErrorAsync(string message);
        Task LogWarningAsync(string message);
    }
}
