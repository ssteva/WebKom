using System;

namespace webkom.Models.DTO
{
    public class PorudzbenicaDto
    {
        public int Id { get; set; }
        public Int64 Rbr { get; set; }
        public string Tip { get; set; }
        public int Broj { get; set; }
        public int KupacId {get;set;}
        public string Kupac { get; set; }
        public int MestoIsporukeId { get; set; }
        public string MestoIsporuke { get; set; }
        public DateTime Datum { get; set; }
        public DateTime DatumVazenja { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
    }
}
