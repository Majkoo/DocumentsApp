namespace DocumentsApp.Data.Services.Interfaces;

public interface IAesCipher
{ 
    byte[] EncryptString(string plainText, byte[] key, byte[] iv);
    string DecryptString(byte[] cipherText, byte[] key, byte[] iv);
}