using Microsoft.AspNetCore.Http;
using System.IO;

public class ByteArrayFormFile : IFormFile
{
    private readonly string _fileName;
    private readonly byte[] _fileContent;

    public ByteArrayFormFile(string fileName, byte[] fileContent)
    {
        _fileName = fileName;
        _fileContent = fileContent;
    }

    public string ContentType => "application/octet-stream";

    public string ContentDisposition => $"inline; filename=\"{_fileName}\"";

    public IHeaderDictionary Headers => new HeaderDictionary();

    public long Length => _fileContent.Length;

    public string Name => "file"; // El nombre que recibirá el archivo en el servidor, puedes ajustarlo según tus necesidades.

    public string FileName => throw new NotImplementedException();

    public void CopyTo(Stream target)
    {
        using (var stream = new MemoryStream(_fileContent))
        {
            stream.CopyTo(target);
        }
    }

    public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Stream OpenReadStream()
    {
        return new MemoryStream(_fileContent);
    }
}