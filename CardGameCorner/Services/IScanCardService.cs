using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameCorner.Services
{
    public interface IScanCardService
    {
       public Task UploadImageAsync(Stream imageStream);
    }
}
