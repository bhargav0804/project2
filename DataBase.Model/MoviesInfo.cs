using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Model
{
    public class MoviesInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MovieId { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Plot { get; set; }
        public float Ratting { get; set; }
        public virtual ICollection<StarCast> StarCasts { get; set; }
        public virtual ICollection<Genre> Genre { get; set; }
    }
}
