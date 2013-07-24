using System;

namespace AspNetUI.Support
{
    public class FormEventArgs<T> : EventArgs
    {
        public T Data { get; set; }

        public FormEventArgs() { }

        public FormEventArgs(T data)
        {
            this.Data = data;
        }
    }
}