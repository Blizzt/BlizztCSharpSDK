using BlizztCSharpSDK.entities;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Signer;
using Nethereum.Util;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Ipfs.Http;
using BlizztCSharpSDK.infrastructure;

namespace BlizztCSharpSDK
{
    class NFTApi
    {
        public string nftId { get; set; }
        public string metadata { get; set; }
    }

    public class BlizztSDK
    {
        public string gameAddress { get; set; }

        public BlizztSDK(string _gameAddress)
        {
            gameAddress = _gameAddress;
        }

        public string IdentityUserBySignatureQR(string _qr)
        {
            int index = _qr.IndexOf("#");
            string _message = _qr.Substring(0, index);
            string _signature = _qr.Substring(index + 1, _qr.Length - index - 1);

            return IdentifyUserBySignature(_message, _signature);
        }

        public string IdentifyUserBySignature(string _message, string _signature)
        {
            var hasher = new Sha3Keccack();
            var textBytePrefix = Encoding.UTF8.GetBytes("\x19" + "Ethereum Signed Message:\n" + _message.Length + _message);

            var byteList = new List<byte>(textBytePrefix);
            var hashPrefix2 = hasher.CalculateHash(byteList.ToArray()).ToHex();
            var signer = new MessageSigner();
            return signer.EcRecover(hashPrefix2.HexToByteArray(), _signature);
        }

        public async Task<List<NFT>> GetAllGameNFTs()
        {
            var nftList = new List<NFT>();
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"http://localhost:5000/nfts/{gameAddress}");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<NFTApi>>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var ipfs = new IpfsClient("https://ipfs.infura.io:5001/");
            foreach (var item in result)
            {
                var data = await ipfs.FileSystem.ReadAllTextAsync(item.metadata.Replace("ipfs://", ""));
                var nft = JsonSerializer.Deserialize<NFT>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                nft.Id = Int32.Parse(item.nftId);
                nft.Image = nft.Image.Replace("ipfs://", "https://ipfs.io/ipfs/");

                nftList.Add(nft);
            }

            return nftList;
        }

        public async Task<List<NFTUser>> GetPlayerNFTs(string _playerAddress)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"http://localhost:5000/nfts/user/{gameAddress}/{_playerAddress}");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            
            var opts = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            opts.Converters.Add(new JsonStringToNumberConverter());
            
            var nftList = JsonSerializer.Deserialize<List<NFTUser>>(responseBody, opts);

            return nftList;
        }
    }
}
