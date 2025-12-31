import { Injectable } from '@angular/core';
import CryptoJS from 'crypto-js';

@Injectable({
  providedIn: 'root'
})
export class EncryptionService {
  private readonly SECRET_KEY = CryptoJS.enc.Utf8.parse("12345678901234567890123456789012"); // 32 chars (256-bit)
  private readonly IV = CryptoJS.enc.Utf8.parse("1234567890123456"); // 16 chars (128-bit)

  encrypt(plainText: string): string {
    const encrypted = CryptoJS.AES.encrypt(
      CryptoJS.enc.Utf8.parse(plainText),
      this.SECRET_KEY,
      {
        keySize: 256 / 32,
        iv: this.IV,
        mode: CryptoJS.mode.CBC,
        padding: CryptoJS.pad.Pkcs7
      }
    );
    return encrypted.toString(); // Base64 string
  }

  decrypt(cipherText: string): string {
    const decrypted = CryptoJS.AES.decrypt(
      cipherText,
      this.SECRET_KEY,
      {
        keySize: 256 / 32,
        iv: this.IV,
        mode: CryptoJS.mode.CBC,
        padding: CryptoJS.pad.Pkcs7
      }
    );
    return decrypted.toString(CryptoJS.enc.Utf8);
  }
}

// import AES from 'crypto-js/aes';
// import Utf8 from 'crypto-js/enc-utf8';

// @Injectable({
//   providedIn: 'root'
// })
// export class EncryptionService {
//   private readonly SECRET_KEY = CryptoJS.enc.Utf8.parse("12345678901234567890123456789012"); // 32 chars (256-bit)
//   private readonly IV = CryptoJS.enc.Utf8.parse("1234567890123456"); // 16 chars (128-bit)

//   encrypt(plainText: string): string {
//     const encrypted = AES.encrypt(Utf8.parse(plainText), this.SECRET_KEY, {
//       keySize: 256 / 32,
//       iv: this.IV
//     });

//     return encrypted.toString(); // Base64 string
//   }

//   decrypt(cipherText: string): string {
//     const decrypted = AES.decrypt(cipherText, this.SECRET_KEY, {
//       keySize: 256 / 32,
//       iv: this.IV
//     });

//     return decrypted.toString(Utf8);
//   }
// }
