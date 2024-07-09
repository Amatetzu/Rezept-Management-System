using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;

namespace RZM_MVVM_.MVVM
{
    

    public class UpdateHeaderMessage : MessageBase
    {
        public string NewHeader { get; private set; }

        public UpdateHeaderMessage(string newHeader)
        {
            NewHeader = newHeader;
        }
    }
    public class UpdateZutatMessage : MessageBase
    {
        public string NewZutat { get; private set; }

        public UpdateZutatMessage(string newZutat)
        {
            NewZutat = newZutat;
        }
    }
}
