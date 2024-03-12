using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using Azure;

namespace Dominio
{
    public class AzureFileShareService
    {
        private readonly ShareDirectoryClient _directoryClient;

        public AzureFileShareService(string connectionString, string shareName, string directoryName)
        {
            ShareClient shareClient = new ShareClient(connectionString, shareName);
            _directoryClient = shareClient.GetDirectoryClient(directoryName);
        }

        public async Task UploadFileAsync(string fileName, Stream fileStream)
        {
            ShareFileClient fileClient = _directoryClient.GetFileClient(fileName);
            await fileClient.CreateAsync(fileStream.Length);
            await fileClient.UploadRangeAsync(new HttpRange(0, fileStream.Length), fileStream);
        }



    }
}
