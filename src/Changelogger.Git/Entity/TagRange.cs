namespace Changelogger.Git.Entity
{
    public class TagRange
    {
        public string TagFrom { get; set; }
        public string TagTo { get; set; }

        public bool FromToAreEqual
        {
            get
            {
                return TagFrom == TagTo;
            }
        }
    }
}
