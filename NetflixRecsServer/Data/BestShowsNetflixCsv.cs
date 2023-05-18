namespace NetflixRecsServer.Data {
    public class BestShowsNetflixCsv {
        public string title { get; set; } = null!;
        public decimal score { get; set; }
        public int votes { get; set; }
        public string genre { get; set; } = null!;
    }
}
