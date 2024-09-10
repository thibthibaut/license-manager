using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.IO;

namespace LicenseChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to the JWT license file
            string licenseFilePath = "../license.jwt";

            // Load the JWT token from the license file
            string jwtToken = File.ReadAllText(licenseFilePath);

            // Load the public key for verifying the JWT signature (ECC)
            string publicKeyPath = "../public_key.pem";
            string publicKeyText = File.ReadAllText(publicKeyPath);

            // Create ECDsa key from public key PEM
            var ecdsa = ECDsa.Create();
            ecdsa.ImportFromPem(publicKeyText.ToCharArray());

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true, // Check for token expiration
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new ECDsaSecurityKey(ecdsa), // Using ECDsa key to validate signature
                ClockSkew = TimeSpan.Zero // No clock skew, exact expiration check
            };

            try
            {
                // Try to validate the token
                var principal = tokenHandler.ValidateToken(jwtToken, validationParameters, out SecurityToken validatedToken);
                Console.WriteLine("License is valid!");
                Console.WriteLine("Decoded license data:");

                // Print out license claims (data)
                foreach (var claim in principal.Claims)
                {
                    Console.WriteLine($"{claim.Type}: {claim.Value}");
                }
            }
            catch (SecurityTokenExpiredException)
            {
                Console.WriteLine("License has expired.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"License validation failed: {ex.Message}");
            }
        }
    }
}
