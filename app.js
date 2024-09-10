const jwt = require("jsonwebtoken");
const fs = require("fs");
const crypto = require("crypto");

// Load the passphrase from the environment variable
const passphrase = process.env.PRIVATE_KEY_PASSPHRASE;

// Function to generate the license data
// Feel free to add whatever kind of information here
function generateLicense() {
  const licenseData = {
    uniqueIdentifier: crypto.randomUUID(),
    licenseType: "Trial",
    maximumUtilization: 5,
    productFeatures: {
      "Sales Module": "yes",
      "Purchase Module": "yes",
      "Maximum Transactions": "10000",
    },
    licensedTo: {
      name: "John Doe",
      email: "john.doe@example.com",
    },
  };

  return licenseData;
}

// Function to sign the license with JWT using the encrypted private key
function signLicenseWithJwt(licenseData) {
  // Load the encrypted private key
  const privateKey = fs.readFileSync("private_key.pem", "utf8");

  // Decrypt the private key using the passphrase
  const decryptedKey = {
    key: privateKey,
    passphrase: passphrase,
  };

  // Sign the licenseData as JWT
  const token = jwt.sign(licenseData, decryptedKey, {
    algorithm: "ES256", // Elliptic Curve, SHA256
    expiresIn: "30d", // Set JWT expiration to 30 days
  });

  return token;
}

// Function to create the license file
function createLicenseFile() {
  const licenseData = generateLicense();
  const token = signLicenseWithJwt(licenseData);

  // Write the token to a file (this becomes your license file)
  fs.writeFileSync("license.jwt", token);
  console.log("License file created:", token);
}

createLicenseFile();
