using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EVMessage.Xevprm
{
    //public class OperationResult<T> 
    //{
    //    #region Declarations

    //    private T _result;
    //    private bool _isSuccess;
    //    private string _description;
    //    private Exception _exception;

    //    #endregion

    //    #region Properties

    //    public T Result
    //    {
    //        get { return _result; }
    //        set { _result = value; }
    //    }

    //    public bool IsSuccess
    //    {
    //        get { return _isSuccess; }
    //        set { _isSuccess = value; }
    //    }

    //    public string Description
    //    {
    //        get { return _description; }
    //        set { _description = value; }
    //    }

    //    public Exception Exception
    //    {
    //        get { return _exception; }
    //        set { _exception = value; }
    //    }

    //    #endregion

    //    #region Constructors

    //    public OperationResult(bool isSuccess, string description = null, T result = default(T))
    //    {
    //        this.IsSuccess = isSuccess;
    //        this.Description = description;
    //        this.Result = result;
    //    }

    //    public OperationResult(Exception exception, string description = null, T result = default(T))
    //    {
    //        this.IsSuccess = false;
    //        this.Exception = exception;
    //        this.Description = description;s
    //        this.Result = result;
    //    }

    //    #endregion

    //    #region Methods

    //    public string GetExceptionDescription()
    //    {
    //        if (_exception != null)
    //        {
    //            return string.Format("Message: {0} | InnerException: {1}  | StackTrace: {2}", _exception.Message, _exception.InnerException != null ? _exception.InnerException.Message : string.Empty, _exception.StackTrace);
    //        }

    //        return null;
    //    }

    //    #endregion
    //}
}
