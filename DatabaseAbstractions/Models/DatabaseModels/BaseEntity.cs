using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DatabaseAbstractions.Models.DatabaseModels
{
    public class BaseEntity
    {
        [Column("id"), Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public virtual void Update(BaseEntity source)
        {
            throw new NotImplementedException($"Attempt to update entity of type {GetType()}, that has no overriden method for Update");
        }
    }
}
