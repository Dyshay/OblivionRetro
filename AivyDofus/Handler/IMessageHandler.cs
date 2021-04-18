using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AivyDofus.Handler
{
    public interface IMessageHandler
    {
        Task Handle();
        void EndHandle();
        void Error(Exception e);
    }
}
