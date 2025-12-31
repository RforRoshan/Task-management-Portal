using System.Security.Cryptography;
using System.Text;

namespace ARAS
{
    public class RequestEncryptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _encryptionKey = "MySecretKey@123";

        public RequestEncryptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Handle preflight CORS
            if (context.Request.Method == HttpMethods.Options)
            {
                context.Response.StatusCode = StatusCodes.Status200OK;
                await context.Response.CompleteAsync();
                return;
            }

            var isEncrypted = context.Request.Headers.TryGetValue("X-Encrypted", out var encryptedHeader) &&
                              encryptedHeader.ToString().ToLower() == "true";

            if (isEncrypted)
            {
                // Decrypt Request Body
                context.Request.EnableBuffering();
                using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
                var encryptedBody = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;

                var decryptedJson = Decrypt(encryptedBody, _encryptionKey);
                var bytes = Encoding.UTF8.GetBytes(decryptedJson);
                context.Request.Body = new MemoryStream(bytes);
                context.Request.ContentLength = bytes.Length;
            }

            // Replace response stream
            var originalBodyStream = context.Response.Body;
            using var newBodyStream = new MemoryStream();
            context.Response.Body = newBodyStream;

            await _next(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            if (isEncrypted)
            {
                var encryptedResponse = Encrypt(responseBody, _encryptionKey);
                var encryptedBytes = Encoding.UTF8.GetBytes(encryptedResponse);
                context.Response.ContentLength = encryptedBytes.Length;

                context.Response.Body = originalBodyStream;
                await context.Response.Body.WriteAsync(encryptedBytes, 0, encryptedBytes.Length);
            }
            else
            {
                context.Response.Body = originalBodyStream;
                await context.Response.WriteAsync(responseBody);
            }
        }

        private string Encrypt(string plainText, string key)
        {
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.GenerateIV();
            var iv = aes.IV;

            using var encryptor = aes.CreateEncryptor();
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            var encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            var result = new byte[iv.Length + encryptedBytes.Length];
            Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
            Buffer.BlockCopy(encryptedBytes, 0, result, iv.Length, encryptedBytes.Length);

            return Convert.ToBase64String(result);
        }

        private string Decrypt(string encryptedText, string key)
        {
            var fullCipher = Convert.FromBase64String(encryptedText);
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);

            var iv = new byte[16];
            var cipher = new byte[fullCipher.Length - iv.Length];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, cipher.Length);
            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor();
            var decryptedBytes = decryptor.TransformFinalBlock(cipher, 0, cipher.Length);
            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }

}