namespace MovieSearch.Core.Generals
{
    public class Video
    {
        public string Id { get; init; }
        /// <summary>
        /// A country code, e.g. US
        /// </summary>
        public string Iso_3166_1 { get; init; }
        /// <summary>
        /// A language code, e.g. en
        /// </summary>
        public string Iso_639_1 { get; init; }
        public string Key { get; init; }
        public string Name { get; init; }
        public string Site { get; init; }
        public int Size { get; init; }
        public string Type { get; init; }
    }
}