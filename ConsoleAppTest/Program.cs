using System;
using System.Threading.Tasks;
using BlizztCSharpSDK;

namespace ConsoleAppTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            BlizztSDK sdk = new BlizztSDK("0xeA20Bc3Da6136A28967BFCCA9990Fe3a4aA3466B");
            var user1 = sdk.IdentityUserBySignatureQR("I confirm my user account in Blizzt is 0xea4ca1270cc91ac072c3b1d273da95ce8d024fb5#0xb8134632e9b30b36315574f9dbd696aa0f53b1a4ccad10dc3a78810e49dffab41b79487423af8370c31aac0ad8da4c466416919a69ae55a439bc8e08aaa949991b");
            var user2 = sdk.IdentifyUserBySignature("I confirm my user account in Blizzt is 0xea4ca1270cc91ac072c3b1d273da95ce8d024fb5", "0xb8134632e9b30b36315574f9dbd696aa0f53b1a4ccad10dc3a78810e49dffab41b79487423af8370c31aac0ad8da4c466416919a69ae55a439bc8e08aaa949991b");
            var nfts = await sdk.GetAllGameNFTs();
            var playerNfts = await sdk.GetPlayerNFTs("0xeA4ca1270Cc91aC072c3b1d273dA95Ce8d024fB5");
        }
    }
}
