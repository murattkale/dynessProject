namespace Core.Entities.Dto
{
    public class OgrenciSinavKontrolPuanTurPuanDto
    {
        public int OgrenciSinavKontrolId { get; set; }

        public int PuanTurId { get; set; }

        public int SubeId { get; set; }

        public int GenelSira { get; set; }

        public int SubeSira { get; set; }

        public int SinifSira { get; set; }

        public double ToplamPuan { get; set; }

        public string Sinif { get; set; }
    }
}
