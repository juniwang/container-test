// See https://aka.ms/new-console-template for more information
using JWTest;

var client = new KeyVaultTestClient();
await client.Test();