using DatabaseAbstractions.Models.Attributes;
using DatabaseShared.HelpModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseShared.DatabaseModels
{
    [Table("Contraindications")]
    [AssignedType(typeof(ContraindicationEntity))]
    public class ContraindicationEntity : PatientRecord
    {

    }
}
