using DatabaseAbstractions.Models.Attributes;
using DatabaseAbstractions.Models.DatabaseModels;
using DatabaseShared.HelpModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseShared.DatabaseModels
{
    [Table("Allergies")]
    [AssignedType(typeof(AllergyEntity))]
    public class AllergyEntity : PatientRecord
    {

    }
}
