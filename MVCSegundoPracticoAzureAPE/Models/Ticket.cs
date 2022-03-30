using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace APISEgundoPracticoAzureAPE.Model
{
    [Table("TICKETS")]
    public class Ticket
    {
        [Key]
        [Column("IDTICKET")]
        public int IdTicket { get; set; }
        [Column("IDUSUARIO")]
        public int IdUsuario { get; set; }
        [Column("FECHA")]
        public DateTime Fecha { get; set; }
        [Column("IMPORTE")]
        public string Importe { get; set; }
        [Column("PRODUCTO")]
        public string Producto { get; set; }
        [Column("FILENAME")]
        public string FileName { get; set; }
        [Column("STORAGEPATH")]
        public string StoragePath { get; set; }
    }
}
