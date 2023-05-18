namespace NetflixRecsServer.Dtos {
    public class Shows {
        public int Id { get; set; }
        public string? Title { get; set; }
        public decimal Score { get; set; }
        public int Votes { get; set; }
        public int GenreID { get; set; }
    }
}
