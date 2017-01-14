using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tritalk.Core
{
    class InvalidReceiveObjectException : Exception
    {
        private object m_wrong_obj;
        public InvalidReceiveObjectException(object obj)
        {
            m_wrong_obj = obj;
        }

        public override string Message
        {
            get
            {
                return String.Format("Invalid object: {0}.\nThe received object must be inherited from Tritalk.Libs.Trace", m_wrong_obj.GetType().ToString());
            }
        }
    }
}
