using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebChatApi.Contract.Interface
{
    public interface IIdentifiable
    {
        Guid Key { get; set; }
    }
}
