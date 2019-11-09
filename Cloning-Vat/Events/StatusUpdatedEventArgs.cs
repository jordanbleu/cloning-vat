using System;
using System.Collections.Generic;
using System.Text;

namespace Cloning_Vat.Events
{
    public class StatusUpdatedEventArgs : EventArgs
    {
        public StatusUpdatedEventArgs(string status)
        {
            Status = status;
        }
        public string Status { get; set; }

    }
}
