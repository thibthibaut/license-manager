# Open License Generator 

## Generate a Public/Private keypair

> Note: you need to have openssl installed on your machine

Generate a private key once using this command, choose and strong passphrase and don't forget it.
Keep the `private_key.pem` somehwere safe

```sh
openssl ec -in private_key.pem -aes256 -out private_key.pem
```

Generate the corresponding public key, you can share `public_key.pem` with your software

```sh
openssl ec -in private_key.pem -pubout -out public_key.pem
```

Ideally the passphrase will be read from the environment, so on your server make sure to 

```sh
export PRIVATE_KEY_PASSPHRASE="your-strong-passphrase"
```

## Server side: License generation

You need to install jwt with `npm install jsonwebtoken`

Run the code with `node app.js` to generate a license. 

Modify the code accordingly to integrate with your business logic.

## Client side: License validation

Inside `LicenseChecker/`, run `dotnet build && dotnet run` to validate the license and inspect its content. 

Note that the public key must be shared with the client application (or embbeded inside).

N.B the dotnet application depends on JWT which can be installed with `dotnet add package System.IdentityModel.Tokens.Jwt`
