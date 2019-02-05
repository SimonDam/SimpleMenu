namespace Menu
{
    public interface IMenuable
    {
        string Title
        {
            get;
            set;
        }

        /// <summary>
        /// Runs the algorithm.
        /// </summary>
        void Start();
    }
}