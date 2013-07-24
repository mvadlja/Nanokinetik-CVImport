using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EVMessage.Xevprm
{
    public class XevprmValidationTreeNode
    {
        private List<XevprmValidationException> _xevprmValidationExceptions; 

        public object ReadyEntity { get; set; }

        public List<XevprmValidationException> XevprmValidationExceptions
        {
            get { return _xevprmValidationExceptions ?? (_xevprmValidationExceptions = new List<XevprmValidationException>()); }
            set { _xevprmValidationExceptions = value; }
        }
    }
}
