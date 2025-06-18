using DatabaseAbstractions.Models.Attributes;
using DatabaseShared.HelpModels.ForDatabase;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseShared.DatabaseModels
{
    [Table("Contraindications")]
    [AssignedType(typeof(ContraindicationEntity))]
    public class ContraindicationEntity : PatientRecordEntity
    {

    }
}
