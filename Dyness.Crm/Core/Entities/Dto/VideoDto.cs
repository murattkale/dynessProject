using Core.Properties;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Core.Entities.Dto
{
    public class VideoDto
    {
        public int VideoId { get; set; }

        public int DersId { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Baslik")]
        public string Baslik { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "DersAd")]
        public string DersAd { get; set; }

        public string Url { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Durum")]
        public string Durum => EtkinMi ? FieldNameResources.Etkin : FieldNameResources.EtkinDegil;

        [Display(ResourceType = typeof(FieldNameResources), Name = "Etkinmi")]
        public bool EtkinMi { get; set; }

        public string VideoKategoriler { get; set; }

        public string VideoKonular { get; set; }

        public List<string> VideoKategoriList
        {
            get
            {
                var kategoriler = new List<string>();

                if (!string.IsNullOrEmpty(VideoKategoriler))
                {
                    for (int i = 0; i < VideoKategoriler.Split(',').Length; i++)
                    {
                        var videoKategori = VideoKategoriler.Split(',')[i].Trim();

                        if (string.IsNullOrEmpty(videoKategori) || kategoriler.Count(x => string.Equals(x, videoKategori)) > 0)
                            continue;

                        kategoriler.Add(videoKategori);
                    }
                }

                return kategoriler;
            }
        }

        public List<string> VideoKonuList
        {
            get
            {
                var konular = new List<string>();

                if (!string.IsNullOrEmpty(VideoKonular))
                {
                    for (int i = 0; i < VideoKonular.Split(',').Length; i++)
                    {
                        var videoKonu = VideoKonular.Split(',')[i].Trim();

                        if (string.IsNullOrEmpty(videoKonu) || konular.Count(x => string.Equals(x, videoKonu)) > 0)
                            continue;

                        konular.Add(videoKonu);
                    }
                }

                return konular;
            }
        }
    }
}
