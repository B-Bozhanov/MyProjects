namespace StartUp
{
    public class DownTown
    {
        public DownTown(string name)
        {
            Name = name;
            Districts = new();
        }

        public string? Name { get; set; }

        public HashSet<string> Districts { get; private set; } = null!;

        public void Add(string district)
        {
            if (!Districts.Contains(district))
            {
                Districts.Add(district);
            }
        }

        public void AddRange(IEnumerable<string> districts)
        {
            foreach (var district in districts)
            {
                Add(district);
            }
        }
    }
}