using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardGameCorner.ViewModels;
using CardGameCorner.Models;

namespace CardGameCorner.Services
{
    public interface IScanCardService
    {
        Task<MemoryStream> CompressImageAsync(Stream inputStream, long maxSize);
        Task<ApiResponse_Card> UploadImageAsync(Stream imageStream);
        Task<CardSearchResponseViewModel> SearchCardAsync(CardSearchRequest cardRequest);
    }
}
