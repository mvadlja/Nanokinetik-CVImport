using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EVMessage.Xevprm
{
    public class ValidationResult
    {
        #region Declarations

        private object _result;
        private bool _isSuccess;
        private string _description;
        private List<XevprmValidationException> _xevprmValidationExceptions;
        private List<Exception> _exceptions;
        private Tree<XevprmValidationTreeNode> _xevprmValidationTree;
        #endregion

        #region Properties

        public object Result
        {
            get { return _result; }
            set { _result = value; }
        }

        public bool IsSuccess
        {
            get { return _isSuccess; }
            set { _isSuccess = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public List<XevprmValidationException> XevprmValidationExceptions
        {
            get { return _xevprmValidationExceptions ?? (_xevprmValidationExceptions = new List<XevprmValidationException>()); }
            set { _xevprmValidationExceptions = value; }
        }

        public List<Exception> Exceptions
        {
            get { return _exceptions ?? (_exceptions = new List<Exception>()); }
            set { _exceptions = value; }
        }

        public Tree<XevprmValidationTreeNode> XevprmValidationTree
        {
            get { return _xevprmValidationTree ?? (_xevprmValidationTree = new Tree<XevprmValidationTreeNode>()); }
            set { _xevprmValidationTree = value; }
        }

        #endregion

        #region Constructors

        public ValidationResult(bool isSuccess, string description = null, List<XevprmValidationException> xevprmValidationExceptions = null, List<Exception> exceptions = null, Tree<XevprmValidationTreeNode> xevprmValidationTree = null)
        {
            this.IsSuccess = isSuccess;
            this.Description = description;
            this.XevprmValidationExceptions = xevprmValidationExceptions;
            this.Exceptions = exceptions;
            this.XevprmValidationTree = xevprmValidationTree;
        }

        public ValidationResult(Exception exception, string description = null)
        {
            this.IsSuccess = false;
            this.Description = description;
            this.Exceptions.Add(exception);
        }

        #endregion

        #region Methods

        public string GetExceptionDescription(Exception exception)
        {
            if (exception != null)
            {
                return string.Format("Message: {0} | InnerException: {1}  | StackTrace: {2}", exception.Message, exception.InnerException != null ? exception.InnerException.Message : string.Empty, exception.StackTrace);
            }

            return null;
        }

        public string GetExceptionDescription(int index)
        {
            var exception = Exceptions[index];
            if (exception != null)
            {
                return string.Format("Message: {0} | InnerException: {1}  | StackTrace: {2}", exception.Message, exception.InnerException != null ? exception.InnerException.Message : string.Empty, exception.StackTrace);
            }

            return null;
        }
        #endregion
    }
}
