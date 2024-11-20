namespace Universal.Events
{
    public interface IRequestExecutor
    {
        /// <summary>
        /// To receive messages, you should manually subscribe/unsubscribe to <see cref="RequestController"/>. <br></br>
        /// </summary>
        public bool TryExecuteRequest(ExecutableRequest request);
    }
}