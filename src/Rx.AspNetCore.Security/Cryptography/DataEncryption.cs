using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rx.AspNetCore.Security.Cryptography
{
    public class DataEncryption : IDataEncryption
    {
        public DataEncryption(IDataProtectionProvider provider)
        {
            Protector = provider.CreateProtector("#@!$1^&*");
        }

        private IDataProtector Protector { get; set; }

        public string Decrypt(string encryptedValue)
        {
            return Protector.Unprotect(encryptedValue);
        }

        public string Encrypt(string value)
        {
            return Protector.Protect(value);
        }
    }

    public interface IDataEncryption {
        string Encrypt(string value);

        string Decrypt(string encryptedValue);
    }
}
