using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TrafficDataBackEndAPI.BackEndApi.Util;

namespace TrafficDataBackEndAPI.BackEndApi.Extraction 
{
    public class FileDownloader
    {
        private readonly IConfiguration configuration;
        private readonly HttpClientDownloader client;

        public FileDownloader(IConfiguration configuration, HttpClient client)
        {
            this.configuration = configuration;
            this.client = new HttpClientDownloader(client);
        }

        // Methods which downloads and extracts the compressed data files
        // Assuming that the file is compressed 
        // If is not, throws exception
        // The destination file path and source URLs are found in the Appsettings file

        public async Task<bool> DownloadTrafficDataFileAsync()
        {
            try
            {
                await client.DownloadFile(configuration.GetSection("URLs:TrafficDataURL").Value, configuration.GetSection("FilePaths:TrafficDataCompressedFilePath").Value);

                FileInfo fileToDecompress = new FileInfo(configuration.GetSection("FilePaths:TrafficDataCompressedFilePath").Value);

                return ExtractDataFile(fileToDecompress);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }
            
        }

        public async Task<bool> DownloadTrafficDataFileAsync(string fileDestination)
        {
            try
            {
                await client.DownloadFile(configuration.GetSection("URLs:TrafficDataURL").Value, fileDestination);

                FileInfo fileToDecompress = new FileInfo(fileDestination);

                return ExtractDataFile(fileToDecompress);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }

        }

        public async Task<bool> DownloadMeasurementPointsFileAsync()
        {
            await client.DownloadFile(configuration.GetSection("URLs:MeasurementPointsURL").Value, configuration.GetSection("FilePaths:MeasurementPointCompressedFilePath").Value);
                
            FileInfo fileToDecompress = new FileInfo(configuration.GetSection("FilePaths:MeasurementPointCompressedFilePath").Value);

            return ExtractDataFile(fileToDecompress);
        }

        public async Task<bool> DownloadMeasurementPointsFileAsync(string fileDestination)
        {
            await client.DownloadFile(configuration.GetSection("URLs:MeasurementPointsURL").Value, fileDestination);

            FileInfo fileToDecompress = new FileInfo(fileDestination);

            return ExtractDataFile(fileToDecompress);
        }

        private bool ExtractDataFile(FileInfo fileToDecompress)
        {
            try
            {
                using (FileStream originalFileStream = fileToDecompress.OpenRead())
                {
                    string currentFileName = fileToDecompress.FullName;
                    string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

                    using (FileStream decompressedFileStream = File.Create(newFileName))
                    {
                        using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                        {
                            decompressionStream.CopyTo(decompressedFileStream);
                        }
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }
    }
}